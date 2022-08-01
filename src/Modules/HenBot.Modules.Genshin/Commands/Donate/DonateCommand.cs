using System.Text;
using HenBot.Core.Commands;
using HenBot.Modules.Genshin.Services.CurrencyProvider;
using HenBot.Modules.Genshin.Services.GenshinDonateProvider;
using JetBrains.Annotations;

namespace HenBot.Modules.Genshin.Commands.Donate;

[UsedImplicitly]
public class DonateCommand : CommandBase<DonateCommandData>
{
	private readonly IGenshinDonateProvider _donateProvider;
	private readonly ICurrencyProvider _currencyProvider;
	
	public DonateCommand(
		IGenshinDonateProvider donateProvider,
		ICurrencyProvider currencyProvider)
	{
		_currencyProvider = currencyProvider;
		_donateProvider = donateProvider;
	}

	public override async Task<CommandResult> ExecuteAsync(DonateCommandData commandData)
	{
		var currency = await _currencyProvider.GetCurrency(commandData.Currency);

		var donateItems = _donateProvider.GetItems();
		donateItems.Reverse();

		decimal totalCost = 0;
		var totalAmount = 0L;
		var sb = new StringBuilder().AppendLine($"Чтобы получить {commandData.Amount:N0} примогемов необходимо купить:");
		for (var i = 0; i < donateItems.Count; i++)
		{
			var item = donateItems[i];
			var itemAmount = item.BaseAmount + item.BonusAmount;
			var itemCount = 0;
			
			while (commandData.Amount - totalAmount >= itemAmount 
				|| (i < donateItems.Count - 1 && commandData.Amount - totalAmount > donateItems[i + 1].BaseAmount + item.BonusAmount)
				|| (totalAmount < commandData.Amount && i == donateItems.Count - 1))
			{
				totalAmount += itemAmount;
				itemCount++;
			}
			
			if (itemCount >= 1)
			{
				var itemCost = itemCount * item.Prices[commandData.Currency] * currency.Value;
				totalCost += itemCost;
				sb.AppendLine($"{itemCount}x {item.BaseAmount} + {item.BonusAmount} гемов (~{itemCost:N2} RUB)");
			}
		}

		sb.AppendLine($"Это будет стоить ~{totalCost:N2} RUB. Останется {totalAmount - commandData.Amount:N0} дополнительных гемов");
		return CommandResult.New.AppendMessage(sb.ToString());
	}
}