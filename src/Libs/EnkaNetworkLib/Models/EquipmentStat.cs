using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace EnkaNetworkLib.Models;

public class EquipmentStat
{
	[JsonProperty("mainPropId")]
	public string? MainStat { get; set; }

	[JsonProperty("appendPropId")]
	public string? AdditionalStat { get; set; }

	[JsonProperty("statValue")]
	public double Value { get; set; }
}