using HenBot.Core.Messaging.Messages;
using OptionalSharp;

namespace HenBot.Core.Messaging.Handling;

public interface IInputMessageHandler
{
	Task<Optional<OutputMessage>> HandleMessage(InputMessage inputMessage);
}