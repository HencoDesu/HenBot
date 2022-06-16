using FluentResults;
using HenBot.Core.Commands;

namespace HenBot.Core.Input.Parsing;

public interface IInputParser
{
	Task<Result<ICommandData>> ParseData(BotInput input);
}

public interface IInputParser<TData>
	: IInputParser
	where TData : ICommandData
{
	new Task<Result<TData>> ParseData(BotInput input);
}