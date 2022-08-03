using System.Linq.Expressions;
using FluentResults;
using HenBot.Core.Extensions;
using HenBot.Core.Messaging.Messages;

namespace HenBot.Core.Commands.Parsing;

public abstract class CommandDataParser<TData>
	: ICommandDataParser<TData> 
	where TData : ICommandData
{
	private readonly Func<TData> _creatorFunc;
	private readonly List<IPropertyParser<TData>> _propertyParsers = new();

	protected CommandDataParser(
		Func<TData> creatorFunc)
	{
		_creatorFunc = creatorFunc;
	}
	
	Task<Result<ICommandData>> ICommandDataParser.ParseData(InputMessage input)
	{
		return ParseData(input).ThenAsync(result => result.ToResult<ICommandData>(v => v));
	}

	public async Task<Result<TData>> ParseData(InputMessage input)
	{
		var data = _creatorFunc();
		var errors = new List<string>();

		foreach (var propertyParser in _propertyParsers)
		{
			var propertyErrors = await propertyParser.TryParse(data, input);
			
			if (propertyErrors.Count > 0)
			{
				errors.AddRange(propertyErrors);
			}
		}

		return errors.Count == 0 ? Result.Ok(data) : Result.Fail(errors);
	}

	protected void HasProperty<TProperty>(
		Expression<Func<TData, TProperty>> propertyExpression,
		Action<IPropertyParser<TData, TProperty>> configureParser)
	{
		var parser = new PropertyParser<TData, TProperty>(propertyExpression);
		configureParser(parser);
		_propertyParsers.Add(parser);
	}
}