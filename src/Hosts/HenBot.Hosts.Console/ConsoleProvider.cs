using HenBot.Core.Commands;
using HenBot.Core.Input;
using HenBot.Core.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace HenBot.Hosts.Console;

[UsedImplicitly]
public class ConsoleProvider 
	: BaseProvider<ConsoleProvider>
{
	public ConsoleProvider(
		ILogger<ConsoleProvider> logger,
		IInputHandler inputHandler) 
		: base(logger, inputHandler)
	{ }
	
	protected override async Task CheckForInput()
	{
		var input = ReadInput(System.Console.ReadLine()!);
		await HandleInput(input);
	}

	protected override Task HandleException(Exception exception)
	{
		return Task.CompletedTask;
	}

	public override Task SendResult(CommandResult commandResult)
	{
		System.Console.WriteLine(commandResult.Text);
		return Task.CompletedTask;
	}

	private BotInput ReadInput(string consoleMessage)
	{
		return new ConsoleInput(this, consoleMessage);
	}
}