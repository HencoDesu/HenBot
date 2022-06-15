using JetBrains.Annotations;

namespace HenBot.Modules.Genshin.Services.GenshinDonateProvider;

[UsedImplicitly]
public class GenshinDonateProvider : IGenshinDonateProvider
{
	public List<DonateItemInfo> GetItems()
	{
		return new List<DonateItemInfo>
		{
			CreateItem(60, 0, Prices(99, 0.99, 27.3)),
			CreateItem(300, 30, Prices(499, 4.99, 125.3)),
			CreateItem(980, 110, Prices(1390, 14.99, 384.3)),
			CreateItem(1980, 260, Prices(2790, 29.99, 706.3)),
			CreateItem(3280, 600, Prices(4690, 49.99, 1259.3)),
			CreateItem(6480, 1600, Prices(9490, 99.99, 2519.3)),
		};
	}

	private DonateItemInfo CreateItem(int baseAmount, int bonusAmount, Dictionary<string, decimal> prices)
	{
		return new DonateItemInfo
		{
			BaseAmount = baseAmount,
			BonusAmount = bonusAmount,
			Prices = prices
		};
	}

	private Dictionary<string, decimal> Prices(double rub, double usd, double uah)
	{
		return new Dictionary<string, decimal>
		{
			["RUB"] = (decimal) rub,
			["USD"] = (decimal) usd,
			["UAH"] = (decimal) uah
		};
	}
}