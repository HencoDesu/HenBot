using HenBot.Core.Input.Parsing;
using HenBot.Core.Providers;

namespace HenBot.Core.Input;

public abstract class BotInput : IParsableCommandData
{
	private string[]? _arguments;
	
	protected BotInput(
		IProvider provider)
	{
		Provider = provider;
	}

	public IProvider Provider { get; }
	
	public abstract string MessageText { get; }

	public string[] Arguments 
		=> _arguments ??= MessageText.Split().Skip(1).ToArray();
}