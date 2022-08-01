using HenBot.Core.Messaging.Messages;

namespace HenBot.Core.Commands.Parsing;

public class ParsableCommandData
{
	private ParsableCommandData() {}

	public string[] Arguments { get; private init; } = Array.Empty<string>();

	public static implicit operator ParsableCommandData(InputMessage message)
		=> new()
		{
			Arguments = message.MessageText.Split().Skip(1).ToArray()
		};
}