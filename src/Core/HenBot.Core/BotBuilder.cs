using System.Collections.Concurrent;
using HenBot.Core.Commands;
using HenBot.Core.Modules;
using HenBot.Core.Providers.FileProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IModule = HenBot.Core.Modules.IModule;

namespace HenBot.Core;

public class BotBuilder : IBotBuilder
{
	private readonly IServiceCollection _serviceCollection;
	private readonly IDictionary<string, Type> _commands;

	public BotBuilder(IServiceCollection serviceCollection)
	{
		_commands = new ConcurrentDictionary<string, Type>();
		
		_serviceCollection = serviceCollection;
		_serviceCollection.AddSingleton<ICommandExecutor>(CreateCommandExecutor)
						  .AddSingleton<IFileProvider, FileProvider>();
	}

	public IBotBuilder UseModule<TModule>() 
		where TModule : class, IModule
	{
		var moduleBuilder = new ModuleBuilder(_serviceCollection, _commands);
		TModule.Init(moduleBuilder);
		return this;
	}

	private CommandExecutor CreateCommandExecutor(IServiceProvider provider)
	{
		return new CommandExecutor(
			_commands,
			provider.GetRequiredService<ILogger<CommandExecutor>>(),
			provider.CreateScope());
	}
}