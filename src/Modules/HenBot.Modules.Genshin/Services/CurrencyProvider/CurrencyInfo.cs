namespace HenBot.Modules.Genshin.Services.CurrencyProvider;

public class CurrencyInfo
{
	public ushort NumCode { get; init; }

	public string CharCode { get; init; } = string.Empty;
	
	public uint Nominal { get; set; }

	public string Name { get; init; } = string.Empty;

	public decimal Value { get; set; }
}