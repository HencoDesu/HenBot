using HenBot.Core.Messaging;
using HenBot.Core.Messaging.Handling;
using HenBot.Core.Messaging.Messages;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace HenBot.Hosts.Console;

[UsedImplicitly]
public class ConsoleProvider
	: MessageProviderBase
{
	public ConsoleProvider(
		ILogger<ConsoleProvider> logger,
		IInputMessageHandler inputHandler)
		: base(logger, inputHandler)
	{ }

	/// <inheritdoc />
	protected override Task CheckForInput()
	{
		var input = System.Console.ReadLine();

		if (!string.IsNullOrEmpty(input))
		{
			var message = new InputMessage
			{
				Provider = this,
				MessageText = input
			};
			OnInputReceived(message);
		}
		
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	protected override Task HandleInputException(Exception e)
	{
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	protected override Task SendOutput(OutputMessage outputMessage)
	{
		System.Console.WriteLine(outputMessage.MessageText);
		return Task.CompletedTask;
	}
}