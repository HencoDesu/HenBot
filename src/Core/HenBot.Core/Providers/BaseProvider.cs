using System.Diagnostics.CodeAnalysis;
using HenBot.Core.Commands;
using HenBot.Core.Input;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HenBot.Core.Providers;

[SuppressMessage("ReSharper", "ContextualLoggerProblem")]
public abstract class BaseProvider<TProvider>
	: IHostedService,
	  IProvider
	where TProvider : BaseProvider<TProvider>
{
	private readonly IInputHandler _inputHandler;
	private readonly ILogger<TProvider> _logger;

	protected BaseProvider(
		ILogger<TProvider> logger, 
		IInputHandler inputHandler)
	{
		_logger = logger;
		_inputHandler = inputHandler;
	}
	
	protected TimeSpan CheckDelay { get; } = TimeSpan.FromMilliseconds(100);
	
	public virtual Task StartAsync(CancellationToken cancellationToken)
	{
		Task.Run(async () =>
		{
			while (true)
			{
				try
				{
					await CheckForInput();
					await Task.Delay(CheckDelay, cancellationToken);
				} catch (Exception e)
				{
					_logger.LogError(e, "Error in input loop");
					await HandleException(e);
				}
			}
		}, cancellationToken);
		return Task.CompletedTask;
	}

	public virtual Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	protected abstract Task CheckForInput();
	
	protected Task HandleInput(BotInput botInput) 
		=> _inputHandler.HandleInput(botInput);

	protected abstract Task HandleException(Exception exception);

	public abstract Task SendResult(CommandResult commandResult);
}