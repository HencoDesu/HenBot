using HenBot.Core.Commands;
using HenBot.Core.Messaging.Handling;
using HenBot.Core.Modules;
using HenBot.Core.Providers.FileProvider;
using Microsoft.Extensions.DependencyInjection;
using IModule = HenBot.Core.Modules.IModule;

namespace HenBot.Core;

public class BotBuilder : IBotBuilder
{
	private readonly IServiceCollection _serviceCollection;
	private readonly IDictionary<string, Type> _parsers;
	private readonly IDictionary<string, Type> _commands;

	public BotBuilder(IServiceCollection serviceCollection)
	{
		_parsers = new Dictionary<string, Type>();
		_commands = new Dictionary<string, Type>();
		
		_serviceCollection = serviceCollection;
		_serviceCollection.AddSingleton<IInputMessageHandler, InputMessageHandler>()
						  .AddSingleton<ICommandRegistry>(p => new CommandRegistry(p.CreateScope(), _parsers, _commands))
						  .AddSingleton<IFileProvider, FileProvider>();
	}

	public IBotBuilder UseModule<TModule>() 
		where TModule : class, IModule
	{
		var moduleBuilder = new ModuleBuilder(_serviceCollection, _parsers, _commands);
		TModule.Init(moduleBuilder);
		return this;
	}
}