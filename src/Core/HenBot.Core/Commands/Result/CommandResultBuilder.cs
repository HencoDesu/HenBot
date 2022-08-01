using System.Text;
using HenBot.Core.Extensions;
using HenBot.Core.Input;

namespace HenBot.Core.Commands.Result;

public class CommandResultBuilder
{
	private readonly BotInput _input;
	private readonly StringBuilder _messageBuilder = new();
	private readonly List<FileInfo> _attachments = new();

	public CommandResultBuilder(BotInput input)
	{
		_input = input;
	}

	public CommandResultBuilder WithMessage(string message)
		=> this.FluentAction(b => b._messageBuilder.AppendLine(message));

	public CommandResultBuilder WithMessages(IEnumerable<string> messages)
		=> this.FluentAction(b => b._messageBuilder.AppendLines(messages));
	
	public CommandResultBuilder WithAttachment(FileInfo attachment)
		=> this.FluentAction(b => b._attachments.Add(attachment));
	
	public CommandResultBuilder WithAttachments(IEnumerable<FileInfo> attachments)
		=> this.FluentAction(b => b._attachments.AddRange(attachments));

	public static implicit operator CommandResult(CommandResultBuilder b) => b.Build();

	internal CommandResult Build()
		=> new(_input, _messageBuilder.ToString(), _attachments);
}