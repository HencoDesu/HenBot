using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace EnkaNetworkLib.Models;

public class EquipmentWeapon
{
	[JsonProperty("level")]
	public int Level { get; set; }

	[JsonProperty("promoteLevel")]
	public int PromoteLevel { get; set; }

	[JsonProperty("affixMap")]
	public Dictionary<int, int> AffixMap { get; set; }
}