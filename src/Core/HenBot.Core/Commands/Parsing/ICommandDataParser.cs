using FluentResults;
using HenBot.Core.Extensions;
using HenBot.Core.Messaging.Messages;

namespace HenBot.Core.Commands.Parsing;

public interface ICommandDataParser
{
	Task<Result<ICommandData>> ParseData(InputMessage input);
}

public interface ICommandDataParser<TData>
	: ICommandDataParser
	where TData : ICommandData
{
	new Task<Result<TData>> ParseData(InputMessage input)
		=> ParseData(input).ThenAsync(result => result.ToResult<TData>());
}