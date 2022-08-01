using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace EnkaNetworkLib.Models;

public class CharacterInfo
{
	[JsonProperty("Element")]
	public Element Element { get; set; }

	[JsonProperty("Consts")]
	public List<string> ConstellationIcons { get; set; }

	[JsonProperty("SkillOrder")]
	public List<int> SkillOrder { get; set; }

	[JsonProperty("Skills")]
	public Dictionary<string, string> Skills { get; set; }

	[JsonProperty("ProudMap")]
	public Dictionary<string, int> ProudMap { get; set; }

	[JsonProperty("NameTextMapHash")]
	public long NameTextMapHash { get; set; }

	[JsonProperty("SideIconName")]
	public string SideIconName { get; set; }

	[JsonProperty("QualityType")]
	public string QuailtyType { get; set; }
}