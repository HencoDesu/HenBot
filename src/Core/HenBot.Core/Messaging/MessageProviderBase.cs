using FluentDate;
using HenBot.Core.Extensions;
using HenBot.Core.Messaging.Handling;
using HenBot.Core.Messaging.Messages;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace HenBot.Core.Messaging;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class MessageProviderBase
	: IMessagesProvider
{
	private readonly ILogger _logger;
	private readonly IInputMessageHandler _inputHandler;

	protected MessageProviderBase(
		ILogger logger, 
		IInputMessageHandler inputHandler)
	{
		_inputHandler = inputHandler;
		_logger = logger;
	}

	public bool Enabled { get; private set; }

	protected TimeSpan CheckInputDelay { get; set; } = 100.Milliseconds();
	
	public virtual Task EnableAsync()
	{
		Enabled = true;

		Task.Run(InputLoop).Start();

		return Task.CompletedTask;
	}

	public virtual Task DisableAsync()
	{
		Enabled = false;
		
		return Task.CompletedTask;
	}

	protected abstract Task CheckForInput();

	protected abstract Task HandleInputException(Exception e);

	protected void OnInputReceived(InputMessage inputMessage)
	{
		_inputHandler.HandleMessage(inputMessage)
					 .ThenAsync(result => result.HasValue ? SendOutput(result.Value) : Task.CompletedTask)
					 .Start();
	}

	protected abstract Task SendOutput(OutputMessage outputMessage);

	private async Task InputLoop()
	{
		while (Enabled)
		{
			try
			{
				await Task.Delay(CheckInputDelay);
				await CheckForInput();
			} catch (Exception e)
			{
				_logger.LogError(e, "Error in input loop");
				await HandleInputException(e);
			}
		}
	}
}