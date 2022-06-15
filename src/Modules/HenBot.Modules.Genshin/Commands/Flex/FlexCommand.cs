using HenBot.Core.Commands;
using HenBot.Core.Extensions;
using HenBot.Core.Providers.FileProvider;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using OpenQA.Selenium;

namespace HenBot.Modules.Genshin.Commands.Flex;

[UsedImplicitly]
public class FlexCommand : BaseCommand<FlexCommandData>
{
	private readonly IFileProvider _fileProvider;
	private readonly IWebDriver _driver;
	private readonly IMemoryCache _profileDataCache;

	public FlexCommand(
		FlexCommandParser dataParser,
		IFileProvider fileProvider,
		IWebDriver driver,
		IMemoryCache profileDataCache) 
		: base(dataParser)
	{
		_fileProvider = fileProvider;
		_driver = driver;
		_profileDataCache = profileDataCache;
	}

	protected override async Task<CommandResult> Execute(FlexCommandData commandData)
	{
		if (!_profileDataCache.TryGetValue(commandData.Uid, out List<FileInfo> cards))
		{
			_driver.Navigate().GoToUrl($"https://enka.shinshin.moe/u/{commandData.Uid}");

			var characters = _driver.FindElements(By.CssSelector(@"div .avatar")).ToList();

			cards = new List<FileInfo>();
			foreach (var character in characters)
			{
				character.Click();
				await Task.Delay(TimeSpan.FromSeconds(10));
				var card = _driver.FindElement(By.XPath(@"/html/body/main/content/div[3]/div[2]/div"));
				var screenshot = (card as ITakesScreenshot)!.GetScreenshot();
				var file = _fileProvider.CreateTempFile(extension: ".png");
				await file.WriteBytes(screenshot.AsByteArray);
				cards.Add(file);

				var cacheEntry = _profileDataCache.CreateEntry(commandData.Uid);
				cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(5);
				cacheEntry.RegisterPostEvictionCallback(CleanupCachedValue);
			}

			_driver.Quit();
		}

		return new CommandResult
		{
			Text = $"Флекс писюнами",
			AttachmentFiles = cards
		};
	}

	private void CleanupCachedValue(object key, object value, EvictionReason reason, object state)
	{
		if (value is not List<FileInfo> cards)
		{
			return;
		}

		foreach (var card in cards)
		{
			card.Delete();
		}
	}
}