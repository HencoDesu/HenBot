using EnkaNetworkLib;
using HenBot.Core.Commands;
using HenBot.Core.Providers.FileProvider;
using JetBrains.Annotations;

namespace HenBot.Modules.Genshin.Commands.Flex;

[UsedImplicitly]
public class FlexCommand : BaseCommand<FlexCommandData>
{
	private readonly IFileProvider _fileProvider;
	private readonly IEnkaNetworkClient _enkaNetworkClient;

	public FlexCommand(
		IFileProvider fileProvider,
		IEnkaNetworkClient enkaNetworkClient)
	{
		_fileProvider = fileProvider;
		_enkaNetworkClient = enkaNetworkClient;
	}

	protected override async Task<CommandResult> Execute(CommandContext<FlexCommandData> commandContext)
	{
		var profile = await _enkaNetworkClient.GetProfileInfo(commandContext.Data.Uid);
		var firstCharacter = await _enkaNetworkClient.GetCharacterInfo(profile.Characters.First().CharacterId);
		var firstCharacterName = await _enkaNetworkClient.GetLocalization(firstCharacter.NameTextMapHash.ToString(), "RU");

		return commandContext.ExecutionResult
							 .WithMessage($"Флекс писюном {profile.PlayerInfo.Nickname}")
							 .WithMessage($"Первый персонаж: {firstCharacterName}");
	}
}