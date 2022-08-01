using HenBot.Core.Messaging;
using Microsoft.Extensions.Hosting;

namespace HenBot.Core;

public class HenBot : IHostedService
{
	private readonly IEnumerable<IMessagesProvider> _messagesProviders;

	public HenBot(IEnumerable<IMessagesProvider> messagesProviders)
	{
		_messagesProviders = messagesProviders;
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		foreach (var messagesProvider in _messagesProviders)
		{
			messagesProvider.EnableAsync();
		}
		
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		foreach (var messagesProvider in _messagesProviders)
		{
			messagesProvider.DisableAsync();
		}
		
		return Task.CompletedTask;
	}
}