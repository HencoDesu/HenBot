using HenBot.Core.Commands;
using HenBot.Core.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace HenBot.Hosts.Console;

[UsedImplicitly]
public class ConsoleProvider 
	: BaseProvider<ConsoleProvider>
{
	public ConsoleProvider(
		ICommandExecutor commandExecutor, 
		ILogger<ConsoleProvider> logger) 
		: base(commandExecutor, logger)
	{
	}

	protected override async Task CheckForInput()
	{
		var input = System.Console.ReadLine();
		if (input is not null)
		{
			var result = await HandleInput(input);
			System.Console.WriteLine(result.Text);
		}
	}

	protected override Task HandleException(Exception exception)
	{
		return Task.CompletedTask;
	}
}