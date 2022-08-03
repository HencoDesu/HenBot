using HenBot.Core.Modules;
using HenBot.Modules.Vk.Messaging;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.Abstractions;

namespace HenBot.Modules.Vk;

[UsedImplicitly]
public class VkModule : IModule
{
	public static void Init(IModuleBuilder moduleBuilder)
	{
		moduleBuilder.RegisterProvider<VkProvider>().ConfigureServices(services =>
		{
			services.AddTransient<IVkApi>(p => new VkApi(p.GetRequiredService<ILogger<VkApi>>()));
		});
	}
}