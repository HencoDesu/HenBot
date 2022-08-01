using System.Text.Json;
using System.Text.Json.Serialization;
using EnkaNetworkLib.Models;

namespace EnkaNetworkLib;

public class EnkaNetworkClient 
	: IEnkaNetworkClient, 
	  IDisposable
{
	private const string EnkaUrl = "https://enka.network";
	private const string LocalesUrl = "https://raw.githubusercontent.com/EnkaNetwork/API-docs/master/store/loc.json";
	private const string CharactersUrl = "https://raw.githubusercontent.com/EnkaNetwork/API-docs/master/store/characters.json";
	
	private readonly HttpClient _httpClient;

	private Dictionary<string, Dictionary<string, string>>? _locales;
	private Dictionary<string, CharacterInfo>? _characterInfos;

	public EnkaNetworkClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<ProfileInfo> GetProfileInfo(uint uuid)
	{
		var requestUrl = EnkaUrl + $"/u/{uuid}/__data.json";
		var response = await _httpClient.GetStringAsync(requestUrl);
		var profileInfo = JsonSerializer.Deserialize<ProfileInfo>(response);

		return profileInfo!;
	}

	public async Task<CharacterInfo> GetCharacterInfo(int characterId)
	{
		if (_characterInfos is null)
		{
			var rawData = await _httpClient.GetStringAsync(CharactersUrl);
			_characterInfos = JsonSerializer.Deserialize<Dictionary<string, CharacterInfo>>(rawData);
		}

		return _characterInfos[characterId.ToString()];
	}

	public async Task<string> GetLocalization(string localizationHash, string language)
	{
		if (_locales is null)
		{
			var rawData = await _httpClient.GetStringAsync(LocalesUrl);
			_locales = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(rawData);
		}

		return _locales is not null &&
			   _locales.TryGetValue(localizationHash, out var translations) &&
			   translations.TryGetValue(language, out var localized)
			? localized
			: string.Empty;
	}

	public void Dispose()
	{
		_httpClient.Dispose();
	}
}