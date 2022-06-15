using HenBot.Core.Modules;
using HenBot.Modules.Genshin.Commands.Donate;
using HenBot.Modules.Genshin.Commands.Flex;
using HenBot.Modules.Genshin.Services.CurrencyProvider;
using HenBot.Modules.Genshin.Services.GenshinDonateProvider;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HenBot.Modules.Genshin;

[UsedImplicitly]
public class GenshinModule : IModule
{
	public static void Init(IModuleBuilder moduleBuilder)
	{
		moduleBuilder.RegisterCommand<DonateCommand, DonateCommandData, DonateCommandParser>("!donate")
					 .RegisterCommand<FlexCommand, FlexCommandData, FlexCommandParser>("!flex")
					 .ConfigureServices(services =>
					 {
						 services.TryAddTransient<IMemoryCache, MemoryCache>();

						 services.AddSingleton<ICurrencyProvider, CentralBankOfRussia>()
								 .AddSingleton<IGenshinDonateProvider, GenshinDonateProvider>();
					 });
	}
}