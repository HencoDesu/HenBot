using HenBot.Core.Input;

namespace HenBot.Core.Commands;

public class CommandResult
{
	internal CommandResult(BotInput input, string text, List<FileInfo> attachments)
	{
		InputData = input;
		Text = text;
		AttachmentFiles = attachments;
	}
	
	public BotInput? InputData { get; }

	public string Text { get; }

	public List<FileInfo>? AttachmentFiles { get; }

	
	public bool IsEmpty => string.IsNullOrEmpty(Text) && 
						   (AttachmentFiles is null || AttachmentFiles.Count == 0);
}