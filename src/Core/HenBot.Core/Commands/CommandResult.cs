namespace HenBot.Core.Commands;

public class CommandResult
{
	public string Text { get; init; } = string.Empty;

	public List<FileInfo> AttachmentFiles { get; init; } = new();

	public bool IsEmpty => string.IsNullOrEmpty(Text) && AttachmentFiles.Count == 0;
}