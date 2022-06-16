using FluentResults;
using HenBot.Core.Commands;
using HenBot.Core.Extensions;
using HenBot.Core.Input.Parsing;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HenBot.Core.Input;

[UsedImplicitly]
public class InputHandler : IInputHandler
{
	private readonly IDictionary<string, (Type, Type)> _commands;
	private readonly IServiceScope _scope;
	private readonly ILogger<InputHandler> _logger;

	public InputHandler(
		IDictionary<string, (Type, Type)> commands,
		IServiceScope scope,
		ILogger<InputHandler> logger)
	{
		_commands = commands;
		_scope = scope;
		_logger = logger;
	}

	public Task HandleInput(BotInput input)
	{
		var commandName = input.MessageText.Split().First();
		if (_commands.TryGetValue(commandName, out var item))
		{
			var (parserType, commandType) = item;

			Task.Run(() => ParseData(parserType, input))
				.ContinueWith(result => ExecuteCommand(result, commandType))
				.ContinueWith(result => input.Provider.SendResult(result));
		}
		return Task.CompletedTask;
	}

	private Task<Result<ICommandData>> ParseData(Type parserType, BotInput input)
	{
		var parser = (IInputParser) _scope.ServiceProvider.GetRequiredService(parserType);
		return parser.ParseData(input);
	}

	private Task<CommandResult> ExecuteCommand(Result<ICommandData> dataParsingResult, Type commandType)
	{
		if (dataParsingResult.IsFailed)
		{
			return CommandResult.Error(dataParsingResult);
		}

		var data = dataParsingResult.Value;
		var command = (BaseCommand) _scope.ServiceProvider.GetRequiredService(commandType);
		try
		{
			return command.Execute(data);
		} catch (Exception e)
		{
			_logger.LogError(e, "Error while handling {CommandName}", commandType.Name);
			return CommandResult.Error(e);
		}
	}
}