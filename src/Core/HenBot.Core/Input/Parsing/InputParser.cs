using System.Linq.Expressions;
using FluentResults;
using HenBot.Core.Commands;
using HenBot.Core.Extensions;

namespace HenBot.Core.Input.Parsing;

public abstract class InputParser<TData>
	: IInputParser<TData> 
	where TData : ICommandData
{
	private readonly Func<TData> _creatorFunc;
	private readonly List<IPropertyParser<TData>> _propertyParsers = new();

	protected InputParser(
		Func<TData> creatorFunc)
	{
		_creatorFunc = creatorFunc;
	}
	
	Task<Result<ICommandData>> IInputParser.ParseData(BotInput input)
		=> ParseData(input).ContinueWith(result => result.ToResult<ICommandData>(v => v));
	
	public async Task<Result<TData>> ParseData(BotInput input)
	{
		var data = _creatorFunc();
		var result = Result.Ok(data);

		foreach (var propertyParser in _propertyParsers)
		{
			result = await propertyParser.Parse(result, input);
		}

		return result;
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