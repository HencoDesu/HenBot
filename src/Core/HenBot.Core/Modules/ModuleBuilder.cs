using HenBot.Core.Commands;
using HenBot.Core.Commands.Parsing;
using Microsoft.Extensions.DependencyInjection;

namespace HenBot.Core.Modules;

public class ModuleBuilder : IModuleBuilder
{
	private readonly IServiceCollection _serviceCollection;
	private readonly IDictionary<string, Type> _commands;

	public ModuleBuilder(
		IServiceCollection serviceCollection, 
		IDictionary<string, Type> commands)
	{
		_serviceCollection = serviceCollection;
		_commands = commands;
	}

	public IModuleBuilder RegisterCommand<TCommand>()
		where TCommand : BaseCommand
		=> ConfigureServices(s => s.AddTransient<BaseCommand, TCommand>());

	public IModuleBuilder RegisterCommand<TCommand, TData, TDataParser>(string commandName) 
		where TCommand : BaseCommand<TData> 
		where TData : ICommandData, new() 
		where TDataParser : class, ICommandDataParser<TData>
	{
		_commands.Add(commandName, typeof(TCommand));

		return ConfigureServices(s => s.AddTransient<TCommand>()
									   .AddTransient<TDataParser>());
	}

	public IModuleBuilder ConfigureServices(Action<IServiceCollection> configure)
	{
		configure(_serviceCollection);
		return this;
	}
}