using System.Net;
using System.Text;
using HenBot.Core.Messaging;
using HenBot.Core.Messaging.Handling;
using HenBot.Core.Messaging.Messages;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VkNet.Abstractions;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.GroupUpdate;
using VkNet.Model.RequestParams;

namespace HenBot.Modules.Vk.Messaging;

[UsedImplicitly]
public class VkProvider 
	: MessageProviderBase
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
		IInputMessageHandler inputHandler,
		IConfiguration configuration,
		IMemoryCache attachmentCache,
		IVkApi vkApi) 
		: base(logger, inputHandler)
	{
		_authToken = configuration.GetRequiredSection("VK")["AuthToken"];
		_groupId = ulong.Parse(configuration.GetRequiredSection("VK")["GroupId"]);
		_logger = logger;
		_attachmentCache = attachmentCache;
		_vkApi = vkApi;
	}

	public override async Task EnableAsync()
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
		
		
		await base.EnableAsync();
	}

	protected override async Task CheckForInput()
	{
		var lpResponse = await _vkApi.Groups.GetBotsLongPollHistoryAsync(new BotsLongPollHistoryParams
		{
			Server = _server,
			Key = _key,
			Ts = _ts,
		});

		foreach (var update in lpResponse.Updates)
		{
			var input = update.Instance switch
			{
				Message message => ReadInput(message),
				MessageNew messageNew => ReadInput(messageNew.Message),
				_ => null
			};
			
			if (input is not null)
			{
				OnInputReceived(input);
			}
		}

		_ts = lpResponse.Ts;
	}

	protected override async Task HandleInputException(Exception exception)
	{
		if (exception is LongPollKeyExpiredException)
		{
			await UpdateLpServer();
		}
	}

	private InputMessage ReadInput(Message message) 
		=> new VkInputMessage
		{
			Provider = this,
			Message = message,
		};
	
	protected override async Task SendOutput(OutputMessage outputMessage)
	{
		if (outputMessage.IsEmpty || 
			outputMessage.OriginalMessage is not VkInputMessage vkInput ||
			vkInput.Message is null)
		{
			return;
		}
		
		await _vkApi.Messages.SendAsync(new MessagesSendParams
		{
			Attachments = await GetAttachments(outputMessage.Attachments).ToListAsync(),
			Message = outputMessage.MessageText,
			PeerId = vkInput.Message.PeerId,
			RandomId = _random.NextInt64(),
			ReplyTo = vkInput.Message.Id
		});
	}

	private async IAsyncEnumerable<MediaAttachment> GetAttachments(IEnumerable<FileInfo>? attachmentFiles)
	{
		if (attachmentFiles is null)
		{
			yield break;
		}
		
		foreach (var attachmentFile in attachmentFiles)
		{
			if (_attachmentCache.TryGetValue(attachmentFile.Name, out MediaAttachment attachment))
			{
				yield return attachment;
			} else
			{
				yield return await UploadAttachment(attachmentFile);
			}
		}
	}

	private async Task<MediaAttachment> UploadAttachment(FileSystemInfo fileInfo)
	{
		var uploadServer = _vkApi.Photo.GetMessagesUploadServer((long) _groupId);
		var uploadedFile = await UploadFile(uploadServer.UploadUrl, fileInfo);
		var response = _vkApi.Photo.SaveMessagesPhoto(uploadedFile);
		var attachment = response.Single();
		_attachmentCache.Set(fileInfo.Name, attachment);
		return attachment;
	}

	private static async Task<string> UploadFile(string serverUrl, FileSystemInfo fileInfo)
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