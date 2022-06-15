namespace HenBot.Modules.Genshin.Services.CurrencyProvider;

public interface ICurrencyProvider
{
	Task<List<CurrencyInfo>> GetCurrencies();

	Task<CurrencyInfo> GetCurrency(string name);
}