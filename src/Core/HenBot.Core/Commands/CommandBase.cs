namespace HenBot.Core.Commands;

public abstract class CommandBase
{
	public abstract Task<CommandResult> ExecuteAsync(ICommandData commandData);
}

public abstract class CommandBase<TData> 
	: CommandBase
	where TData : ICommandData
{
	public override Task<CommandResult> ExecuteAsync(ICommandData commandData)
	{
		if (commandData is not TData data)
		{
			throw new InvalidCastException("Wrong type of command data");
		}
		
		return ExecuteAsync(data);
	}

	public abstract Task<CommandResult> ExecuteAsync(TData commandData);
}