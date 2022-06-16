using System.Linq.Expressions;
using FluentResults;
using HenBot.Core.Commands;
using HenBot.Core.Extensions;

namespace HenBot.Core.Input.Parsing;

public class PropertyParser<TData, TProperty>
	: IPropertyParser<TData, TProperty>
	where TData : ICommandData
{
	private readonly Expression<Func<TData, TProperty>> _propertyExpression;
	private readonly List<(Func<TProperty, bool>, string)> _validators = new();
	private readonly Dictionary<Type, string> _exceptionHandlers = new();
	private Func<IParsableCommandData, Task<TProperty>>? _mapFunc;
	private Func<TData, bool>? _mapCondition;
	private TProperty? _defaultValue;

	public PropertyParser(Expression<Func<TData, TProperty>> propertyExpression)
	{
		_propertyExpression = propertyExpression;
	}

	public IPropertyParser<TData, TProperty> MapIf(
		Func<TData, bool> condition,
		Func<IParsableCommandData, Task<TProperty>> mapFunc)
	{
		_mapCondition = condition;
		_mapFunc = mapFunc;
		return this;
	}

	public IPropertyParser<TData, TProperty> DefaultValue(TProperty value)
	{
		_defaultValue = value;
		return this;
	}

	public IPropertyParser<TData, TProperty> Validate(
		Func<TProperty, bool> validator,
		string errorMessage)
	{
		_validators.Add((validator, errorMessage));
		return this;
	}

	public IPropertyParser<TData, TProperty> HandleException<TException>(string message)
		where TException : Exception
	{
		_exceptionHandlers.Add(typeof(TException), message);
		return this;
	}

	public async Task<Result<TData>> Parse(Result<TData> previousResult, IParsableCommandData input)
	{
		var result = previousResult;
		var data = previousResult.ValueOrDefault;
		var value = _defaultValue;

		if (_mapCondition is not null &&
			_mapFunc is not null &&
			_mapCondition(data))
		{
			try
			{
				value = await _mapFunc(input);
			} catch (Exception e)
			{
				if (_exceptionHandlers.TryGetValue(e.GetType(), out var message))
				{
					result = previousResult.WithError(message);
				}
			}
		}

		var validated = true;
		foreach (var (validator, message) in _validators)
		{
			if (!validator(value!))
			{
				result = result.WithError(message);
				validated = false;
			}
		}

		if (validated)
		{
			data.SetPropertyValue(_propertyExpression, value!);
		}

		return result;
	}
}