using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace EnkaNetworkLib.Models;

public class StandCharacterInfo
{
	[JsonProperty("avatarId")]
	public int CharacterId { get; set; }

	[JsonProperty("propMap")]
	public Dictionary<int, CharacterInfoProperty> InfoProperties { get; set; }
	
	[JsonProperty("talentIdList")]
	public List<int> ConstellationsInfo { get; set; }

	[JsonProperty("fightPropMap")]
	public Dictionary<int, double> FightProperties { get; set; }

	[JsonProperty("skillDepotId")]
	public int SkillSetId { get; set; }

	[JsonProperty("inherentProudSkillList")]
	public List<int> UnlockedSkillIds { get; set; }

	[JsonProperty("skillLevelMap")]	
	public Dictionary<int, int> SkillLevelMap { get; set; }

	[JsonProperty("proudSkillExtraLevelMap")]
	public Dictionary<int, int> ProudSkillExtraLevelMap { get; set; }

	[JsonProperty("equipList")]
	public List<Equipment> Equipments { get; set; }

	[JsonProperty("fetterInfo")]
	public FetterInfo FetterInfo { get; set; }
}