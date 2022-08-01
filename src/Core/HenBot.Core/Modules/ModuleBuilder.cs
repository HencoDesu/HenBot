using HenBot.Core.Commands;
using HenBot.Core.Commands.Parsing;
using HenBot.Core.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace HenBot.Core.Modules;

public class ModuleBuilder : IModuleBuilder
{
	private readonly IServiceCollection _serviceCollection;
	private readonly IDictionary<string, Type> _parsers;
	private readonly IDictionary<string, Type> _commands;

	public ModuleBuilder(
		IServiceCollection serviceCollection,
		IDictionary<string, Type> parsers,
		IDictionary<string, Type> commands)
	{
		_serviceCollection = serviceCollection;
		_parsers = parsers;
		_commands = commands;
	}

	public IModuleBuilder RegisterCommand<TCommand>()
		where TCommand : CommandBase
		=> ConfigureServices(s => s.AddTransient<CommandBase, TCommand>());

	public IModuleBuilder RegisterCommand<TCommand, TData, TDataParser>(string commandName) 
		where TCommand : CommandBase<TData> 
		where TData : ICommandData, new() 
		where TDataParser : class, ICommandDataParser<TData>
	{
		_parsers.Add(commandName, typeof(TDataParser));
		_commands.Add(commandName, typeof(TCommand));

		return ConfigureServices(s => s.AddTransient<TCommand>()
									   .AddTransient<TDataParser>());
	}

	public IModuleBuilder RegisterProvider<TProvider>()
		where TProvider : class, IMessagesProvider
	{
		return ConfigureServices(s => s.AddSingleton<IMessagesProvider, TProvider>());
	}

	public IModuleBuilder ConfigureServices(Action<IServiceCollection> configure)
	{
		configure(_serviceCollection);
		return this;
	}
}