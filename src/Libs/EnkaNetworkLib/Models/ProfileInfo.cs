using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace EnkaNetworkLib.Models;

public class ProfileInfo
{
	[JsonProperty("playerInfo")]
	public PlayerInfo PlayerInfo { get; set; }

	[JsonProperty("avatarInfoList")]
	public List<StandCharacterInfo> Characters { get; set; }
}