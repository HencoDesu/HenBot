namespace HenBot.Core.Input;

public interface IInputHandler
{
	Task HandleInput(BotInput input);
}