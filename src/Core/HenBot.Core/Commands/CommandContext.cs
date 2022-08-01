using HenBot.Core.Commands.Result;
using HenBot.Core.Input;

namespace HenBot.Core.Commands;

public class CommandContext
{
	public CommandContext(BotInput input, ICommandData commandData)
	{
		Data = commandData;
		ExecutionResult = new CommandResultBuilder(input);
	}

	public ICommandData Data { get; }
	public CommandResultBuilder ExecutionResult { get; }
}

public class CommandContext<TData> : CommandContext
	where TData : ICommandData
{
	public CommandContext(BotInput input, TData commandData) 
		: base(input, commandData)
	{
	}

	public new TData Data => (TData) base.Data;
}