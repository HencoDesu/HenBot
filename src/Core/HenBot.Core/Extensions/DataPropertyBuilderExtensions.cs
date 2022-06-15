using HenBot.Core.Commands.Parsing;

namespace HenBot.Core.Extensions;

public static class DataPropertyBuilderExtensions
{
	private const string DefaultErrorMessage = "";
	
	public static IDataPropertyBuilder<TData, TProperty> Mandatory<TData, TProperty>(
		this IDataPropertyBuilder<TData, TProperty> propertyBuilder,
		string message = DefaultErrorMessage) 
		=> propertyBuilder.HandleException<IndexOutOfRangeException>(message);

	public static IDataPropertyBuilder<TData, TProperty> HaveAllowedValues<TData, TProperty>(
		this IDataPropertyBuilder<TData, TProperty> propertyBuilder,
		IEnumerable<TProperty> allowedValues, 
		string message = DefaultErrorMessage)
		=> propertyBuilder.Validate(allowedValues.Contains, message);

	public static IDataPropertyBuilder<TData, TProperty> ShouldBeGreater<TData, TProperty>(
		this IDataPropertyBuilder<TData, TProperty> propertyBuilder,
		TProperty value,
		string message = DefaultErrorMessage)
		where TProperty : IComparisonOperators<TProperty, TProperty> 
		=> propertyBuilder.Validate(v => v > value, message);
	
	public static IDataPropertyBuilder<TData, TProperty> ShouldBeGreaterOrEqual<TData, TProperty>(
		this IDataPropertyBuilder<TData, TProperty> propertyBuilder,
		TProperty value,
		string message = DefaultErrorMessage)
		where TProperty : IComparisonOperators<TProperty, TProperty> 
		=> propertyBuilder.Validate(v => v >= value, message);

	public static IDataPropertyBuilder<TData, TProperty> ShouldBeLess<TData, TProperty>(
		this IDataPropertyBuilder<TData, TProperty> propertyBuilder,
		TProperty value,
		string message = DefaultErrorMessage)
		where TProperty : IComparisonOperators<TProperty, TProperty> 
		=> propertyBuilder.Validate(v => value > v, message);
	
	public static IDataPropertyBuilder<TData, TProperty> ShouldBeLessOrEqual<TData, TProperty>(
		this IDataPropertyBuilder<TData, TProperty> propertyBuilder,
		TProperty value,
		string message = DefaultErrorMessage)
		where TProperty : IComparisonOperators<TProperty, TProperty> 
		=> propertyBuilder.Validate(v => value >= v, message);

	public static IDataPropertyBuilder<TData, TProperty> ShouldBeBetween<TData, TProperty>(
		this IDataPropertyBuilder<TData, TProperty> propertyBuilder,
		TProperty lesserValue,
		TProperty greaterValue,
		string message = "")
		where TProperty : IComparisonOperators<TProperty, TProperty> 
		=> propertyBuilder.Validate(v => greaterValue > v && v > lesserValue, message);
}