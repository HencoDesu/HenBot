using HenBot.Core.Input;
using HenBot.Core.Providers;
using VkNet.Model;

namespace HenBot.Modules.Vk;

public class VkInput : BotInput
{
	public VkInput(
		IProvider provider,
		Message message)
		: base(provider)
	{
		Message = message;
	}

	public Message Message { get; }

	public override string MessageText => Message.Text;
}