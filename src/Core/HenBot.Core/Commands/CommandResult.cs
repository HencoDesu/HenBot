using System.Text;
using HenBot.Core.Extensions;

namespace HenBot.Core.Commands;

public class CommandResult
{
	private readonly StringBuilder _textBuilder;
	private readonly List<FileInfo> _attachments;

	private CommandResult()
	{
		_textBuilder = new StringBuilder();
		_attachments = new List<FileInfo>();
	}

	public static CommandResult New => new ();

	public string Text 
		=> _textBuilder.ToString();

	public IReadOnlyList<FileInfo> Attachments 
		=> _attachments;

	public CommandResult AppendMessage(string message)
		=> this.Do(r => r._textBuilder.AppendLine(message));
	
	public CommandResult AppendMessages(IEnumerable<string> messages)
		=> this.Do(r => r._textBuilder.AppendLines(messages));

	public CommandResult AddAttachment(FileInfo attachment)
		=> this.Do(r => r._attachments.Add(attachment));

	public CommandResult AddAttachments(IEnumerable<FileInfo> attachments)
		=> this.Do(r => r._attachments.AddRange(attachments));
}