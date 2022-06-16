using HenBot.Core.Input;
using HenBot.Core.Providers;

namespace HenBot.Hosts.Console;

public class ConsoleInput : BotInput
{
	public ConsoleInput(
		IProvider provider,
		string message) 
		: base(provider)
	{
		MessageText = message;
	}

	public override string MessageText { get; }
}