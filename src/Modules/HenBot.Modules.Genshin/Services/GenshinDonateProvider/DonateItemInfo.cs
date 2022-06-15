namespace HenBot.Modules.Genshin.Services.GenshinDonateProvider;

public class DonateItemInfo
{
	public int BaseAmount { get; init; } = 0;
	public int BonusAmount { get; init; } = 0;
	public Dictionary<string, decimal> Prices { get; init; } = default!;
}