using System.Text;

namespace HenBot.Core.Extensions;

public static class StringBuilderExtensions
{
	public static StringBuilder AppendLines(this StringBuilder builder, IEnumerable<string> lines)
	{
		foreach (var line in lines)
		{
			builder.AppendLine(line);
		}

		return builder;
	}
}