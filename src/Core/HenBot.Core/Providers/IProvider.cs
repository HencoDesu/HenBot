using HenBot.Core.Commands;

namespace HenBot.Core.Providers;

public interface IProvider
{
	Task SendResult(CommandResult commandResult);
}