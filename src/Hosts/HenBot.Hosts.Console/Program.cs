using HenBot.Core.Extensions;
using HenBot.Hosts.Console;
using HenBot.Modules.Genshin;
using HenBot.Modules.Vk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
			 .MinimumLevel.Information()
			 .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
			 .Enrich.FromLogContext()
			 .WriteTo.Console()
			 .WriteTo.File("logs/bot-.log",
						   rollingInterval: RollingInterval.Hour,
						   retainedFileCountLimit: 10)
			 .CreateLogger();

await Host.CreateDefaultBuilder(args)
		  .ConfigureAppConfiguration((_, configuration) =>
		  {
			  configuration.AddUserSecrets<Program>();
		  })
		  .UseSerilog()
		  .UseHenBot(builder =>
		  {
			  builder.UseModule<GenshinModule>()
					 .UseModule<VkModule>();
		  })
		  .ConfigureServices((_, services) =>
		  {
			  services.AddTransient<IWebDriver>(_ =>
			  {
				  var options = new FirefoxOptions();
			  #if RELEASE
				  options.AddArgument("--headless");
			  #endif
				  return new FirefoxDriver(Path.Combine("Drivers"), options);
			  });
		  #if DEBUG
			  services.AddHostedService<ConsoleProvider>();
		  #endif
		  })
		  .UseSystemd()
		  .RunConsoleAsync();