using System.Diagnostics.CodeAnalysis;
using HenBot.Core.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HenBot.Core.Providers;

[SuppressMessage("ReSharper", "ContextualLoggerProblem")]
public abstract class BaseProvider<TProvider>
	: IHostedService
	where TProvider : BaseProvider<TProvider>
{
	private readonly ICommandExecutor _commandExecutor;
	private readonly ILogger<TProvider> _logger;

	protected BaseProvider(
		ICommandExecutor commandExecutor, 
		ILogger<TProvider> logger)
	{
		_commandExecutor = commandExecutor;
		_logger = logger;
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

	protected abstract Task HandleException(Exception exception);
	
	protected async Task<CommandResult> HandleInput(string input)
	{
		var words = input.Split();
		var command = words.First();
		var args = words.Skip(1).ToArray();
		return await _commandExecutor.Execute(command, args);
	}
}