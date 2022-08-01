using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace EnkaNetworkLib.Models;

public class EquipmentReliquary
{
	[JsonProperty("level")]
	public int Level { get; set; }

	[JsonProperty("mainPropId")]
	public int MainPropId { get; set; }

	[JsonProperty("appendPropIdList")]
	public List<int> AppendPropIdList { get; set; }
}