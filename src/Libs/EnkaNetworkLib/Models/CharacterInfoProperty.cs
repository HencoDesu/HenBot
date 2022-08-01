using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace EnkaNetworkLib.Models;

public class CharacterInfoProperty
{
	[JsonProperty("type")]
	public int Type { get; set; }

	[JsonProperty("val")]
	public string Value { get; set; }
}