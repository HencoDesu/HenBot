using System.Linq.Expressions;
using System.Reflection;

namespace HenBot.Core.Commands.Parsing;

public class DataPropertyBuilder<TData, TProperty> : IDataPropertyBuilder<TData, TProperty>
{
	private readonly List<(Func<TProperty, bool>, string)> _validators = new();
	private readonly Dictionary<Type, string> _exceptionHandlers = new();
	private readonly Expression<Func<TData, TProperty>> _expression;
	private Func<string[], TProperty> _mapFunc = default!;
	private TProperty? _defaultValue;

	public DataPropertyBuilder(Expression<Func<TData, TProperty>> expression)
	{
		_expression = expression;
	}

	public IDataPropertyBuilder<TData, TProperty> HasDefaultValue(TProperty defaultValue)
	{
		_defaultValue = defaultValue;
		return this;
	}

	public IDataPropertyBuilder<TData, TProperty> Validate(Func<TProperty, bool> validationFunc, string failMessage)
	{
		_validators.Add((validationFunc, failMessage));
		return this;
	}

	public IDataPropertyBuilder<TData, TProperty> HandleException<TException>(string message) where TException : Exception
	{
		_exceptionHandlers.Add(typeof(TException), message);
		return this;
	}

	public IDataPropertyBuilder<TData, TProperty> MapFrom(Func<string[], TProperty> mapFunc)
	{
		_mapFunc = mapFunc;
		return this;
	}

	public IEnumerable<string> FillData(TData data, string[] args)
	{
		var errors = new List<string>();

		TProperty? value = default;
		try
		{
			value = _mapFunc(args);
		} catch (Exception e)
		{
			if (_exceptionHandlers.TryGetValue(e.GetType(), out var message))
			{
				errors.Add(message);
			}

			value = _defaultValue;
		}

		if (value is not null)
		{
			var validated = true;
			foreach (var (validator, message) in _validators)
			{
				var validationResult = validator(value);

				if (!validationResult)
				{
					validated = false;
					errors.Add(message);
				}
			}

			if (validated)
			{
				var memberExpression = _expression.Body as MemberExpression;
				var property = memberExpression!.Member as PropertyInfo;
				property!.SetValue(data, value);
			}
		}

		return errors;
	}
}