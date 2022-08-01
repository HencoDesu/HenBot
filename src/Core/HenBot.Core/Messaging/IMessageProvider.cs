namespace HenBot.Core.Messaging;

public interface IMessagesProvider
{
	bool Enabled { get; }

	Task EnableAsync();

	Task DisableAsync();
}