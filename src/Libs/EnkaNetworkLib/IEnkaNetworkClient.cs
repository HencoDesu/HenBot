using EnkaNetworkLib.Models;

namespace EnkaNetworkLib;

public interface IEnkaNetworkClient
{
	Task<ProfileInfo> GetProfileInfo(uint uuid);

	Task<CharacterInfo> GetCharacterInfo(int characterId);

	Task<string> GetLocalization(string localizationHash, string language);
}