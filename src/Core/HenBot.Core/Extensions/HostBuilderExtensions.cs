using Microsoft.Extensions.Hosting;

namespace HenBot.Core.Extensions;

public static class HostBuilderExtensions
{
	public static IHostBuilder UseHenBot(this IHostBuilder hostBuilder, Action<IBotBuilder> configure)
	{
		hostBuilder.ConfigureServices((_, services) =>
		{
			var botBuilder = new BotBuilder(services);
			configure(botBuilder);
		});
		
		return hostBuilder;
	}
}