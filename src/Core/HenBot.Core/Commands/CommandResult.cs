using System.Text;
using FluentResults;
using HenBot.Core.Input;

namespace HenBot.Core.Commands;

public class CommandResult
{
	private CommandResult(string text)
	{
		Text = text;
	}
	
	public string Text { get; }

	public List<FileInfo>? AttachmentFiles { get; private set; }

	public bool IsEmpty => string.IsNullOrEmpty(Text) && 
						   (AttachmentFiles is null || AttachmentFiles.Count == 0);
	
	public BotInput? InputData { get; private set; }

	public static CommandResult Ok(string message = "")
		=> new(message);

	public static CommandResult Error(Exception e)
		=> new(e.Message);

	public static CommandResult Error<TResult>(Result<TResult> result)
	{
		var sb = new StringBuilder();
		foreach (var error in result.Errors)
		{
			sb.AppendLine(error.Message);
		}

		return new CommandResult(sb.ToString());
	}
	
	public static implicit operator Task<CommandResult>(CommandResult result) 
		=> Task.FromResult(result);

	public CommandResult WithInputData(BotInput inputData)
	{
		InputData = inputData;
		return this;
	}

	public CommandResult WithAttachments(List<FileInfo> attachments)
	{
		AttachmentFiles = attachments;
		return this;
	}
}