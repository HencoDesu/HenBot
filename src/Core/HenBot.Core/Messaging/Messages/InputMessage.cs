namespace HenBot.Core.Messaging.Messages;

public class InputMessage
{
	public IMessagesProvider? Provider { get; init; }

	public virtual string MessageText { get; init; } = string.Empty;
}