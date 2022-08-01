using HenBot.Core.Commands;
using HenBot.Core.Messaging.Messages;
using Microsoft.Extensions.Logging;
using OptionalSharp;

namespace HenBot.Core.Messaging.Handling;

public class InputMessageHandler : IInputMessageHandler
{
	private readonly ILogger<InputMessageHandler> _logger;
	private readonly CommandRegistry _commandRegistry;

	public InputMessageHandler(
		ILogger<InputMessageHandler> logger,
		CommandRegistry commandRegistry)
	{
		_logger = logger;
		_commandRegistry = commandRegistry;
	}

	public async Task<Optional<OutputMessage>> HandleMessage(InputMessage inputMessage)
	{
		var isCommand = inputMessage.MessageText.StartsWith('!');
		if (isCommand)
		{
			var commandResult = await HandleCommand(inputMessage);
			return new OutputMessage()
			{
				OriginalMessage = inputMessage,
				MessageText = commandResult.Text,
				Attachments = commandResult.Attachments
			};
		}

		return Optional.None();
	}

	private async Task<CommandResult> HandleCommand(InputMessage commandMessage)
	{
		var commandName = commandMessage.MessageText.Split().First();

		if (!_commandRegistry.Registered(commandName))
		{
			return CommandResult.New.AppendMessage("Command wasn't found");
		}

		var parser = _commandRegistry.GetParser(commandName);
		var dataParsingResult = await parser.ParseData(commandMessage);

		if (dataParsingResult.IsFailed)
		{
			return CommandResult.New.AppendMessages(dataParsingResult.Errors.Select(e => e.Message));
		}

		var command = _commandRegistry.GetCommand(commandName);
		
		try
		{
			return await command.ExecuteAsync(dataParsingResult.Value);
		} catch (Exception e)
		{
			_logger.LogError(e, "Error while handling {CommandName}", commandName);
			return CommandResult.New.AppendMessage(e.Message);
		}
	}
}