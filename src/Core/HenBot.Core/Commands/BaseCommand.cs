namespace HenBot.Core.Commands;

public abstract class BaseCommand
{
	public abstract Task<CommandResult> Execute(ICommandData data);
}

public abstract class BaseCommand<TData> : BaseCommand
	where TData : ICommandData, new()
{
	public sealed override Task<CommandResult> Execute(ICommandData data)
		=> Execute((TData) data);

	protected abstract Task<CommandResult> Execute(TData commandData);
}