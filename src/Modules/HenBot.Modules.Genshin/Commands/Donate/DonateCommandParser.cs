using HenBot.Core.Commands.Parsing;
using JetBrains.Annotations;

namespace HenBot.Modules.Genshin.Commands.Donate;

[UsedImplicitly]
public class DonateCommandParser : CommandDataParser<DonateCommandData>
{
	public DonateCommandParser() : base(() => new DonateCommandData())
	{
		HasProperty(d => d.Amount,
					p => p.Map(input => int.Parse(input.Arguments[0]))
						  .Mandatory("А сколько гемов то тебе надо, додик?")
						  .HandleException<OverflowException>("А не слишком ли дохуя тебе надо?")
						  .HandleException<FormatException>("Ты не выебывайся, а цифру напиши, еблан")
						  .ShouldBeGreaterOrEqual(0, "Дебил, нельзя купить отрицательное количество гемов"));
		
		HasProperty(d => d.Currency, 
					p => p.DefaultValue("RUB")
						  .AllowedValues(new[] { "RUB", "USD", "UAH" }, 
											 "В чем ты там донатить собрался, дурачек?")
						  .Map(input => input.Arguments[1].ToUpper()));
	}
}