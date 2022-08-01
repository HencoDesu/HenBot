namespace HenBot.Core.Commands.Parsing;

public interface IPropertyParser<in TData>
	where TData : ICommandData
{
	Task<List<string>> TryParse(TData data, ParsableCommandData input);
}

public interface IPropertyParser<TData, TProperty> 
	: IPropertyParser<TData> 
	where TData : ICommandData
{
	IPropertyParser<TData, TProperty> MapIf(
		Func<TData, bool> condition,
		Func<ParsableCommandData, Task<TProperty>> mapFunc);
	
	IPropertyParser<TData, TProperty> DefaultValue(TProperty value);

	IPropertyParser<TData, TProperty> Validate(
		Func<TProperty, bool> validator, string errorMessage);

	IPropertyParser<TData, TProperty> HandleException<TException>(string message)
		where TException : Exception;
}

public static class PropertyParserExtensions
{
	public static IPropertyParser<TData, TProperty> Map<TData, TProperty>(
		this IPropertyParser<TData, TProperty> propertyParser,
		Func<ParsableCommandData, TProperty> mapFunc)
		where TData : ICommandData
		=> propertyParser.MapIf(_ => true, data => Task.FromResult(mapFunc(data)));
	
	public static IPropertyParser<TData, TProperty> Mandatory<TData, TProperty>(
		this IPropertyParser<TData, TProperty> propertyParser,
		string message)
		where TData : ICommandData
		=> propertyParser.Validate(v => v is not null, message);
	
	public static IPropertyParser<TData, TProperty> AllowedValues<TData, TProperty>(
		this IPropertyParser<TData, TProperty> propertyParser,
		IEnumerable<TProperty> allowedValues,
		string message)
		where TData : ICommandData
		=> propertyParser.Validate(allowedValues.Contains, message);
	
	public static IPropertyParser<TData, TProperty> ShouldBeGreaterOrEqual<TData, TProperty>(
		this IPropertyParser<TData, TProperty> propertyParser,
		TProperty value,
		string message)
		where TData : ICommandData
		where TProperty : IComparisonOperators<TProperty, TProperty>
		=> propertyParser.Validate(v => v >= value, message);
	
	public static IPropertyParser<TData, TProperty> ShouldBeGreater<TData, TProperty>(
		this IPropertyParser<TData, TProperty> propertyParser,
		TProperty value,
		string message)
		where TData : ICommandData
		where TProperty : IComparisonOperators<TProperty, TProperty>
		=> propertyParser.Validate(v => v > value, message);
}