using HenBot.Core.Modules;
using HenBot.Modules.Vk.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.Abstractions;

namespace HenBot.Modules.Vk;

[UsedImplicitly]
public class VkModule : IModule
{
	public static void Init(IModuleBuilder moduleBuilder)
	{
		moduleBuilder.ConfigureServices(services =>
		{
			services.AddTransient<IVkApi>(_ => new VkApi(services));
		#if RELEASE
			services.AddHostedService<VkProvider>();
		#endif
		});
	}
}