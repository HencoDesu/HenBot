using HenBot.Core.Commands;
using HenBot.Core.Input.Parsing;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace HenBot.Core.Modules;

/// <summary>
/// Билдер, позволяющий настроить модуль
/// </summary>
[PublicAPI]
public interface IModuleBuilder
{
	/// <summary>
	/// Регистрирует в боте команду
	/// </summary>
	/// <typeparam name="TCommand">Тип команды</typeparam>
	/// <returns>Текущий экземпляр <see cref="IModuleBuilder"/></returns>
	IModuleBuilder RegisterCommand<TCommand>() 
		where TCommand : BaseCommand;

	/// <summary>
	/// Регистрирует в боте команду cо сложными аргументами и парсером для них
	/// </summary>
	/// <typeparam name="TCommand">Тип команды</typeparam>
	/// <typeparam name="TData">Тип данных</typeparam>
	/// <typeparam name="TDataParser">Тип парсера</typeparam>
	/// <returns></returns>
	IModuleBuilder RegisterCommand<TCommand, TData, TDataParser>(string commandName)
		where TCommand : BaseCommand<TData>
		where TData : ICommandData, new()
		where TDataParser : class, IInputParser<TData>;


	/// <summary>
	/// Позволяет зарегистрировать в используемом DI-контейнере необходимые для модуля сервисы
	/// </summary>
	/// <param name="configure">Метод работающий с коллекцией сервисов</param>
	/// <returns>Текущий экземпляр <see cref="IModuleBuilder"/></returns>
	IModuleBuilder ConfigureServices(Action<IServiceCollection> configure);
}