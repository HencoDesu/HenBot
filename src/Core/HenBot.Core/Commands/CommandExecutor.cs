using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HenBot.Core.Commands;

public class CommandExecutor : ICommandExecutor
{
	private readonly IDictionary<string, Type> _commands;
	private readonly ILogger<CommandExecutor> _logger;
	private readonly IServiceScope _scope;

	public CommandExecutor(
		IDictionary<string, Type> commands,
		ILogger<CommandExecutor> logger,
		IServiceScope scope)
	{
		_scope = scope;
		_commands = commands;
		_logger = logger;
	}

	public async Task<CommandResult> Execute(string command)
	{
		return await Execute(command, Array.Empty<string>());
	}

	public async Task<CommandResult> Execute(string commandName, string[] args)
	{
		if (_commands.TryGetValue(commandName, out var commandType))
		{
			if (_scope.ServiceProvider.GetRequiredService(commandType) is not BaseCommand command)
			{
				return new CommandResult();
			}
			
			try
			{
				return await command.Execute(args);
			} catch (Exception e)
			{
				_logger.LogError(e, "Error while handling command {CommandName}", commandName);
				throw;
			}
		}

		return new CommandResult();
	}
}