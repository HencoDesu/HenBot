using System.Collections.Concurrent;
using HenBot.Core.Input;
using HenBot.Core.Modules;
using HenBot.Core.Providers.FileProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IModule = HenBot.Core.Modules.IModule;

namespace HenBot.Core;

public class BotBuilder : IBotBuilder
{
	private readonly IServiceCollection _serviceCollection;
	private readonly IDictionary<string, (Type, Type)> _commands;

	public BotBuilder(IServiceCollection serviceCollection)
	{
		_commands = new ConcurrentDictionary<string, (Type, Type)>();
		
		_serviceCollection = serviceCollection;
		_serviceCollection.AddSingleton<IInputHandler>(CreateInputHandler)
						  .AddSingleton<IFileProvider, FileProvider>();
	}

	public IBotBuilder UseModule<TModule>() 
		where TModule : class, IModule
	{
		var moduleBuilder = new ModuleBuilder(_serviceCollection, _commands);
		TModule.Init(moduleBuilder);
		return this;
	}
	
	private InputHandler CreateInputHandler(IServiceProvider provider)
	{
		return new InputHandler(_commands,
								provider.CreateScope(),
								provider.GetRequiredService<ILogger<InputHandler>>());
	}
}