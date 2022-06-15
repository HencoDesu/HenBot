namespace HenBot.Core.Commands.Parsing;

/// <summary>
/// Парсер для данных команды <see cref="BaseCommand{TData}"/>
/// </summary>
/// <typeparam name="TData">Тип данных команды</typeparam>
public interface ICommandDataParser<TData>
	where TData : ICommandData, new()
{
	/// <summary>
	/// Парсит данные
	/// </summary>
	/// <param name="rawData">Массив аргументов для команды</param>
	/// <returns>Результат парсинга данных</returns>
	ParsingResult<TData> ParseData(string[] rawData);
}