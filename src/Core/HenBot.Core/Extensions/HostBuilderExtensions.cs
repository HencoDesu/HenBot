using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HenBot.Core.Extensions;

public static class HostBuilderExtensions
{
	[Obsolete]
	public static IHostBuilder UseHenBot(this IHostBuilder hostBuilder, Action<IBotBuilder> configure)
	{
		hostBuilder.ConfigureServices((_, services) =>
		{
			var botBuilder = new BotBuilder(services);
			configure(botBuilder);

			services.AddHostedService<HenBot>();
		});
		
		return hostBuilder;
	}

	public static IServiceCollection UseHenBot(
		this IServiceCollection serviceCollection,
		Action<IBotBuilder> configure)
	{
		var botBuilder = new BotBuilder(serviceCollection);
		configure(botBuilder);

		serviceCollection.AddHostedService<HenBot>();

		return serviceCollection;
	}
}