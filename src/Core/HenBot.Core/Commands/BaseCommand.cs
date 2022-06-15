using System.Text;
using HenBot.Core.Commands.Parsing;

namespace HenBot.Core.Commands;

public abstract class BaseCommand
{
	public abstract Task<CommandResult> Execute(string[] args);
}

public abstract class BaseCommand<TData> : BaseCommand
	where TData : ICommandData, new()
{
	private readonly ICommandDataParser<TData> _dataParser;

	protected BaseCommand(ICommandDataParser<TData> dataParser)
	{
		_dataParser = dataParser;
	}

	public sealed override async Task<CommandResult> Execute(string[] args)
	{
		var parsingResult = _dataParser.ParseData(args);

		if (parsingResult.HasErrors)
		{
			var sb = new StringBuilder();
			foreach (var error in parsingResult.Errors)
			{
				sb.AppendLine(error);
			}

			return new CommandResult()
			{
				Text = sb.ToString()
			};
		}
		
		return await Execute(parsingResult.Value);
	}

	protected abstract Task<CommandResult> Execute(TData commandData);
}