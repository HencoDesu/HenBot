using HenBot.Core.Commands.Parsing;

namespace HenBot.Core.Commands;

public interface ICommandRegistry
{
	bool Registered(string commandName);
	ICommandDataParser GetParser(string commandName);
	CommandBase GetCommand(string commandName);
}