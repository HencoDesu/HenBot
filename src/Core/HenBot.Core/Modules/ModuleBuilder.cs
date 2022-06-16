using HenBot.Core.Commands;
using HenBot.Core.Input.Parsing;
using Microsoft.Extensions.DependencyInjection;

namespace HenBot.Core.Modules;

public class ModuleBuilder : IModuleBuilder
{
	private readonly IServiceCollection _serviceCollection;
	private readonly IDictionary<string, (Type, Type)> _commands;

	public ModuleBuilder(
		IServiceCollection serviceCollection, 
		IDictionary<string, (Type, Type)> commands)
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
		where TDataParser : class, IInputParser<TData>
	{
		_commands.Add(commandName, (typeof(TDataParser), typeof(TCommand)));

		return ConfigureServices(s => s.AddTransient<TCommand>()
									   .AddTransient<TDataParser>());
	}

	public IModuleBuilder ConfigureServices(Action<IServiceCollection> configure)
	{
		configure(_serviceCollection);
		return this;
	}
}