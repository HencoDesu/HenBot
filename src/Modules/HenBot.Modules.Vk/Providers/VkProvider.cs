using System.Net;
using System.Net.Http.Headers;
using System.Text;
using HenBot.Core.Commands;
using HenBot.Core.Extensions;
using HenBot.Core.Providers;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VkNet.Abstractions;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace HenBot.Modules.Vk.Providers;

[UsedImplicitly]
public class VkProvider 
	: BaseProvider<VkProvider>
{
	private readonly Random _random = new();
	private readonly ILogger<VkProvider> _logger;
	private readonly IMemoryCache _attachmentCache;
	private readonly IVkApi _vkApi;
	private readonly string _authToken;
	private readonly ulong _groupId;
	private string _server = string.Empty;
	private string _key = string.Empty;
	private string _ts = string.Empty;

	public VkProvider(
		ILogger<VkProvider> logger,
		ICommandExecutor commandExecutor,
		IConfiguration configuration,
		IMemoryCache attachmentCache,
		IVkApi vkApi) 
		: base(commandExecutor, logger)
	{
		_authToken = configuration.GetRequiredSection("VK")["AuthToken"];
		_groupId = ulong.Parse(configuration.GetRequiredSection("VK")["GroupId"]);
		_logger = logger;
		_attachmentCache = attachmentCache;
		_vkApi = vkApi;
	}

	public override async Task StartAsync(CancellationToken cancellationToken)
	{
		try
		{
			await _vkApi.AuthorizeAsync(new ApiAuthParams
			{
				AccessToken = _authToken
			});
			await UpdateLpServer();

		} catch (Exception e)
		{
			_logger.LogError(e, "Error during VkAuth");
			throw;
		}
		
		
		await base.StartAsync(cancellationToken);
	}

	protected override async Task CheckForInput()
	{
		var lpResponse = await _vkApi.Groups.GetBotsLongPollHistoryAsync(new ()
		{
			Server = _server,
			Key = _key,
			Ts = _ts,
		});

		foreach (var update in lpResponse.Updates)
		{
			if (update.Type == GroupUpdateType.MessageNew)
			{
				var message = update.Message;
				CommandResult result;
				
				try
				{
					result = await HandleInput(message.Text);
				} catch (Exception e)
				{
					result = new CommandResult { Text = e.Message };
				}

				await SendResponse(message, result);
			}
		}

		_ts = lpResponse.Ts;
	}

	protected override async Task HandleException(Exception exception)
	{
		if (exception is LongPollKeyExpiredException)
		{
			await UpdateLpServer();
		}
	}

	private async Task SendResponse(Message message, CommandResult result)
	{
		if (result.IsEmpty)
		{
			return;
		}

		List<MediaAttachment> attachments = new ();
		foreach (var attachmentFile in result.AttachmentFiles)
		{
			if (_attachmentCache.TryGetValue(attachmentFile.Name, out MediaAttachment attachment))
			{
				attachments.Add(attachment);
				continue;
			}

			var uploadServer = _vkApi.Photo.GetMessagesUploadServer((long) _groupId);
			var uploadedFile = await UploadFile(
				uploadServer.UploadUrl,
				attachmentFile);
			var response = _vkApi.Photo.SaveMessagesPhoto(uploadedFile);
			attachment = response.Single();
			_attachmentCache.Set(attachmentFile.Name, attachment);
			attachments.Add(attachment);
		}
		
		await _vkApi.Messages.SendAsync(new MessagesSendParams
		{
			Attachments = attachments,
			Message = result.Text,
			PeerId = message.PeerId,
			RandomId = _random.NextInt64(),
			ReplyTo = message.Id
		});
	}
	
	private static async Task<string> UploadFile(string serverUrl, FileInfo fileInfo)
	{
		using var client = new WebClient();
		var response = await client.UploadFileTaskAsync(serverUrl, fileInfo.FullName);
		return Encoding.Default.GetString(response);
	}

	private async Task UpdateLpServer()
	{
		var lpServer = await _vkApi.Groups.GetLongPollServerAsync(_groupId);
		_server = lpServer.Server;
		_key = lpServer.Key;
		_ts = lpServer.Ts;
	}
}