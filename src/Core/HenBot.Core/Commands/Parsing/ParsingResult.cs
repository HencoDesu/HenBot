namespace HenBot.Core.Commands.Parsing;

/// <summary>
/// Результат парсинга данных для выполнения <see cref="BaseCommand{TData}"/>
/// </summary>
/// <typeparam name="TData">Тип данных</typeparam>
public class ParsingResult<TData>
{
	public ParsingResult(TData value, IList<string> errors)
	{
		Value = value;
		Errors = errors;
	}

	public TData Value { get; }

	public bool HasErrors => Errors.Count > 0;
	
	public IList<string> Errors { get; }
}