using HenBot.Core.Messaging.Messages;
using VkNet.Model;

namespace HenBot.Modules.Vk.Messaging;

public class VkInputMessage : InputMessage
{
	public Message? Message { get; init; }

	public override string MessageText => Message?.Text ?? string.Empty;
}