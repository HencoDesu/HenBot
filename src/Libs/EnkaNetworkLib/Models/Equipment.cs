using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace EnkaNetworkLib.Models;

public class Equipment
{
	[JsonProperty("itemId")]
	public int Id { get; set; }

	[JsonProperty("reliquary")]
	public EquipmentReliquary? Reqliquary { get; set; }

	[JsonProperty("weapon")]
	public EquipmentWeapon? Weapon { get; set; }

	[JsonProperty("flat")]
	public EquipmentFlat Flat { get; set; }
}