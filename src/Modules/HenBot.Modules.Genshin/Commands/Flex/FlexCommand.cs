using HenBot.Core.Commands;
using HenBot.Core.Extensions;
using HenBot.Core.Providers.FileProvider;
using JetBrains.Annotations;
using OpenQA.Selenium;

namespace HenBot.Modules.Genshin.Commands.Flex;

[UsedImplicitly]
public class FlexCommand : BaseCommand<FlexCommandData>
{
	private readonly IFileProvider _fileProvider;
	private readonly IWebDriver _driver;

	public FlexCommand(
		IFileProvider fileProvider,
		IWebDriver driver)
	{
		_fileProvider = fileProvider;
		_driver = driver;
	}

	protected override async Task<CommandResult> Execute(FlexCommandData commandData)
	{
		_driver.Navigate().GoToUrl($"https://enka.shinshin.moe/u/{commandData.Uid}");

		var characters = _driver.FindElements(By.CssSelector(@"div .avatar")).ToList();

		var cards = new List<FileInfo>();
		foreach (var character in characters)
		{
			character.Click();
			await Task.Delay(TimeSpan.FromSeconds(10));
			var card = _driver.FindElement(By.XPath(@"/html/body/main/content/div[3]/div[2]/div"));
			var screenshot = (card as ITakesScreenshot)!.GetScreenshot();
			var file = _fileProvider.CreateTempFile(extension: ".png");
			await file.WriteBytes(screenshot.AsByteArray);
			cards.Add(file);
		}

		_driver.Quit();

		return CommandResult.Ok("Флекс писюнами")
							.WithAttachments(cards);
	}
}