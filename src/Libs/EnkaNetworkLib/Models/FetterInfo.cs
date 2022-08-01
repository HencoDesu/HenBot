using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace EnkaNetworkLib.Models;

public class FetterInfo
{
	[JsonProperty("expLevel")]
	public int FriendShipLevel { get; set; }
}