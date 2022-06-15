using HenBot.Core.Commands.Parsing;
using HenBot.Core.Extensions;
using JetBrains.Annotations;

namespace HenBot.Modules.Genshin.Commands.Flex;

[UsedImplicitly]
public class FlexCommandParser : CommandDataParser<FlexCommandData>
{
	public FlexCommandParser()
	{
		HasProperty(d => d.Uid,
					p => p.MapFrom(args => uint.Parse(args[0]))
						  .HasDefaultValue(716222639)
						  .ShouldBeGreater(100000000u, "Хуйня какая-то а не юид"));
	}
}