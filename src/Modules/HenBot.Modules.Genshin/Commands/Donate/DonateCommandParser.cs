using HenBot.Core.Commands.Parsing;
using HenBot.Core.Extensions;
using JetBrains.Annotations;

namespace HenBot.Modules.Genshin.Commands.Donate;

[UsedImplicitly]
public class DonateCommandParser : CommandDataParser<DonateCommandData>
{
	public DonateCommandParser()
	{
		HasProperty(d => d.Amount,
					p => p.MapFrom(args => int.Parse(args[0]))
						  .Mandatory("А сколько гемов то тебе надо, додик?")
						  .HandleException<OverflowException>("А не слишком ли дохуя тебе надо?")
						  .HandleException<FormatException>("Ты не выебывайся, а цифру напиши, еблан")
						  .ShouldBeGreaterOrEqual(0, "Дебил, нельзя купить отрицательное количество гемов"));
		
		HasProperty(d => d.Currency, 
					p => p.HasDefaultValue("RUB")
						  .HaveAllowedValues(new[] { "RUB", "USD", "UAH" }, 
											 "В чем ты там донатить собрался, дурачек?")
						  .MapFrom(args => args[1].ToUpper()));
	}
}