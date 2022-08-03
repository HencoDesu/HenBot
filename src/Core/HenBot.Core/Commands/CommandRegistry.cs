using HenBot.Core.Commands.Parsing;
using Microsoft.Extensions.DependencyInjection;

namespace HenBot.Core.Commands;

public class CommandRegistry : ICommandRegistry
{
	private readonly IDictionary<string, Type> _parserMapping;
	private readonly IDictionary<string, Type> _commandMapping;
	private readonly IServiceScope _scope;

	public CommandRegistry(
		IServiceScope serviceScope, 
		IDictionary<string, Type> parserMapping,
		IDictionary<string, Type> commandMapping)
	{
		_parserMapping = parserMapping;
		_commandMapping = commandMapping;
		_scope = serviceScope;
	}

	public bool Registered(string commandName)
		=> _parserMapping.ContainsKey(commandName) && _commandMapping.ContainsKey(commandName);

	public ICommandDataParser GetParser(string commandName)
	{
		if (!_parserMapping.ContainsKey(commandName))
		{
			throw new ArgumentException("Data parser for that command wasn't registered");
		}

		var parserType = _parserMapping[commandName];
		return (ICommandDataParser) _scope.ServiceProvider.GetRequiredService(parserType);
	}
	
	public CommandBase GetCommand(string commandName)
	{
		if (!_commandMapping.ContainsKey(commandName))
		{
			throw new ArgumentException("Command wasn't registered");
		}

		var commandType = _commandMapping[commandName];
		return (CommandBase) _scope.ServiceProvider.GetRequiredService(commandType);
	}
}