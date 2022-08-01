using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace EnkaNetworkLib.Models;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum Element
{
	[EnumMember(Value = "Fire")]
	Pyro,
	
	[EnumMember(Value = "Water")]
	Hydro,
	
	[EnumMember(Value = "Wind")]
	Anemo,
	
	[EnumMember(Value = "Electric")]
	Electro,

	[EnumMember(Value = "Ice")]
	Cryo,
	
	[EnumMember(Value = "Rock")]
	Geo,
	
	//TODO: Add a name when dendro element will be released
	[EnumMember(Value = "???")]
	Dendro
}