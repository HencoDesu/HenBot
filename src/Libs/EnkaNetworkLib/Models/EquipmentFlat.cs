using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace EnkaNetworkLib.Models;

public class EquipmentFlat
{
	[JsonProperty("nameTextMapHash")]
	public string NameTextMapHash { get; set; }

	[JsonProperty("rankLevel")]
	public EquipmentRarity Rarity { get; set; }

	[JsonProperty("reliquaryMainStat")]
	public EquipmentStat? MainStat { get; set; }

	[JsonProperty("reliquarySubstats")]
	public List<EquipmentStat>? AdditionalStats { get; set; }

	[JsonProperty("equipType")]
	public string? EquipType { get; set; }

	[JsonProperty("weaponStats")]
	public List<EquipmentStat>? WeaponStats { get; set; }

	[JsonProperty("itemType")]
	public string ItemType { get; set; }

	[JsonProperty("icon")]
	public string Icon { get; set; }
}