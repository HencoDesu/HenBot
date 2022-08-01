using System.Linq.Expressions;
using HenBot.Core.Extensions;

namespace HenBot.Core.Commands.Parsing;

public class PropertyParser<TData, TProperty>
	: IPropertyParser<TData, TProperty>
	where TData : ICommandData
{
	private readonly Expression<Func<TData, TProperty>> _propertyExpression;
	private readonly List<(Func<TProperty, bool>, string)> _validators = new();
	private readonly Dictionary<Type, string> _exceptionHandlers = new();
	private Func<ParsableCommandData, Task<TProperty>>? _mapFunc;
	private Func<TData, bool>? _mapCondition;
	private TProperty? _defaultValue;

	public PropertyParser(Expression<Func<TData, TProperty>> propertyExpression)
	{
		_propertyExpression = propertyExpression;
	}

	public IPropertyParser<TData, TProperty> MapIf(
		Func<TData, bool> condition,
		Func<ParsableCommandData, Task<TProperty>> mapFunc)
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

	public async Task<List<string>> TryParse(TData data, ParsableCommandData input)
	{
		var errors = new List<string>();
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
					errors.Add(message);
				}
			}
		}

		if (Validate(value!, errors))
		{
			data.SetPropertyValue(_propertyExpression, value!);
		}

		return errors;
	}

	private bool Validate(TProperty value, ICollection<string> errors)
	{
		var valid = true;
		foreach (var (validator, message) in _validators)
		{
			if (!validator(value))
			{
				errors.Add(message);
				valid = false;
			}
		}

		return valid;
	}
}