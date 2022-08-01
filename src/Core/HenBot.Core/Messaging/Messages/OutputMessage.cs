namespace HenBot.Core.Messaging.Messages;

public class OutputMessage
{
	public InputMessage? OriginalMessage { get; init; }

	public string MessageText { get; init; } = string.Empty;

	public IReadOnlyList<FileInfo> Attachments { get; init; } = new List<FileInfo>();

	public bool IsEmpty
		=> string.IsNullOrEmpty(MessageText) && Attachments.Count == 0;
}