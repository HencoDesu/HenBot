using System.Text;
using System.Xml;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace HenBot.Modules.Genshin.Services.CurrencyProvider;

[UsedImplicitly]
public class CentralBankOfRussia : ICurrencyProvider
{
	private readonly IMemoryCache _currencyCache;

	public CentralBankOfRussia(IMemoryCache currencyCache)
	{
		_currencyCache = currencyCache;
	}

	public async Task<List<CurrencyInfo>> GetCurrencies()
	{
		using var httpClient = new HttpClient();
		var rates = await httpClient.GetAsync("https://www.cbr.ru/scripts/XML_daily.asp");
		var stream = await rates.Content.ReadAsStreamAsync();
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		using var reader = new StreamReader(stream, Encoding.GetEncoding(1251));
		var xml = await reader.ReadToEndAsync();
		var xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(xml);
		var json = JsonConvert.SerializeXmlNode(xmlDoc);
		var dailyInfo = JsonConvert.DeserializeAnonymousType(json, new
		{
			ValCurs = new
			{
				Valute = new List<CurrencyInfo>()
			}
		});

		var currencies = dailyInfo!.ValCurs.Valute;
		foreach (var currency in currencies)
		{
			currency.Value /= 10000 * currency.Nominal;
			currency.Nominal = 1;
			_currencyCache.Set(currency.CharCode, currency, TimeSpan.FromHours(60));
		}
		
		currencies.Add(new CurrencyInfo
		{
			CharCode = "RUB",
			Name = "Российский рубль",
			Nominal = 1,
			NumCode = 0,
			Value = 1
		});

		return currencies;
	}

	public async Task<CurrencyInfo> GetCurrency(string name)
	{
		if (_currencyCache.TryGetValue(name, out CurrencyInfo currency))
		{
			return currency;
		}

		var currencies = await GetCurrencies();
		return currencies.Single(c => c.CharCode == name);
	}
}