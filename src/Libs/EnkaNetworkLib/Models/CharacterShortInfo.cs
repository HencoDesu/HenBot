using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace EnkaNetworkLib.Models;

public class CharacterShortInfo
{
	[JsonProperty("avatarId")]
	public int Id { get; set; }

	[JsonProperty("level")]
	public int Level { get; set; }

	[JsonProperty("costumeId")]
	public int? CostumeId { get; set; }
}