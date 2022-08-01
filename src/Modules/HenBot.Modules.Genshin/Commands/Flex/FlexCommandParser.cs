using HenBot.Core.Commands.Parsing;
using JetBrains.Annotations;

namespace HenBot.Modules.Genshin.Commands.Flex;

[UsedImplicitly]
public class FlexCommandParser : CommandDataParser<FlexCommandData>
{
	public FlexCommandParser() : base(() => new FlexCommandData())
	{
		HasProperty(d => d.Uid,
					p => p.Map(input => uint.Parse(input.Arguments[0]))
						  .DefaultValue(716222639)
						  .ShouldBeGreater(100000000u, "Хуйня какая-то а не юид"));
	}
}