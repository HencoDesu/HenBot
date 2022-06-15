using System.Linq.Expressions;

namespace HenBot.Core.Commands.Parsing;

public abstract class CommandDataParser<TData> : ICommandDataParser<TData> 
	where TData : ICommandData, new()
{
	private readonly List<IDataPropertyBuilder<TData>> _propertyBuilders = new();

	protected ICommandDataParser<TData> HasProperty<TProperty>(
		Expression<Func<TData, TProperty>> propertyExpression, 
		Action<IDataPropertyBuilder<TData, TProperty>> configureProperty)
	{
		var propertyBuilder = new DataPropertyBuilder<TData, TProperty>(propertyExpression);
		configureProperty(propertyBuilder);
		_propertyBuilders.Add(propertyBuilder);

		return this;
	}

	public ParsingResult<TData> ParseData(string[] rawData)
	{
		var data = new TData();
		var errors = new List<string>();

		foreach (var propertyBuilder in _propertyBuilders)
		{
			errors.AddRange(propertyBuilder.FillData(data, rawData));
		}

		return new ParsingResult<TData>(data, errors);
	}
}