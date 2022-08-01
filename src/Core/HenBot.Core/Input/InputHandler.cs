using FluentResults;
using HenBot.Core.Commands;
using HenBot.Core.Commands.Result;
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
				.ContinueWith(result => ExecuteCommand(input, result, commandType))
				.ContinueWith(result => input.Provider.SendResult(result));
		}
		return Task.CompletedTask;
	}

	private Task<Result<ICommandData>> ParseData(Type parserType, BotInput input)
	{
		var parser = (IInputParser) _scope.ServiceProvider.GetRequiredService(parserType);
		return parser.ParseData(input);
	}

	private async Task<CommandResult> ExecuteCommand(BotInput input, IResult<ICommandData> dataParsingResult, Type commandType)
	{
		var resultBuilder = new CommandResultBuilder(input);

		if (dataParsingResult.IsFailed)
		{
			foreach (var error in dataParsingResult.Errors)
			{
				resultBuilder.WithMessage(error.Message);
			}

			return resultBuilder.Build();
		}

		if (_scope.ServiceProvider.GetRequiredService(commandType) is not BaseCommand command)
		{
			return resultBuilder.WithMessage("Command not found")
								.Build();
		}
		
		var context = command.GenerateContext(input, dataParsingResult.Value);
		try
		{
			return await command.Execute(context);
		} catch (Exception e)
		{
			var message = $"Error while handling {commandType.Name}";
			_logger.LogError(e, message);
			return resultBuilder.WithMessage(message).Build();
		}
	}
}