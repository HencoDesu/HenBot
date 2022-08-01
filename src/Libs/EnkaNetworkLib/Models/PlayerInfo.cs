using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace EnkaNetworkLib.Models;

public class PlayerInfo
{
	[JsonProperty("nickname")]
	public string Nickname { get; set; }
	
	[JsonProperty("level")]
	public int Level { get; set; }
	
	[JsonProperty("signature")]
	public string Signature { get; set; }
	
	[JsonProperty("worldLevel")]
	public int WorldLevel { get; set; }

	[JsonProperty("NameCardId")]
	public int NameCardId { get; set; }

	[JsonProperty("finishedAchievementNum")]
	public int FinishedAchievementNumber { get; set; }

	[JsonProperty("towerFloorIndex")]
	public int AbyssFloor { get; set; }

	[JsonProperty("towerLevelIndex")]
	public int AbyssLevel { get; set; }

	[JsonProperty("showAvatarInfoList")]
	public List<CharacterShortInfo> CharacterShortInfos { get; set; }

	[JsonProperty("showNameCardIdList")]
	public List<int> NameCardIds { get; set; }
}