using HenBot.Core.Input;

namespace HenBot.Core.Commands;

public abstract class BaseCommand
{
	public abstract Task<CommandResult> Execute(CommandContext context);

	internal abstract CommandContext GenerateContext(BotInput input, ICommandData commandData);
}

public abstract class BaseCommand<TData> : BaseCommand
	where TData : ICommandData, new()
{
	public sealed override Task<CommandResult> Execute(CommandContext context)
		=> Execute((CommandContext<TData>) context);

	protected abstract Task<CommandResult> Execute(CommandContext<TData> commandContext);

	internal sealed override CommandContext GenerateContext(BotInput input, ICommandData commandData)
	{
		return new CommandContext<TData>(input, (TData) commandData);
	}
}