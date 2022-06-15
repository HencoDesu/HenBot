using JetBrains.Annotations;

namespace HenBot.Core.Commands;

/// <summary>
/// Обработчик выполнения команд
/// </summary>
[PublicAPI]
public interface ICommandExecutor
{
	/// <summary>
	/// Выполнить команду
	/// </summary>
	/// <param name="commandName">Название команды или её алиас</param>
	/// <returns>Результат выполнения команды</returns>
	Task<CommandResult> Execute(string commandName);

	/// <summary>
	/// Выполнить команду
	/// </summary>
	/// <param name="commandName">Название команды или её алиас</param>
	/// <param name="args">Аргументы для команды</param>
	/// <returns>Результат выполнения команды</returns>
	Task<CommandResult> Execute(string commandName, string[] args);
}