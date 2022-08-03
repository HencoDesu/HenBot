using HenBot.Core.Extensions;
using HenBot.Modules.Genshin;
using HenBot.Modules.Vk;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
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

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services.UseHenBot(henBotBuilder =>
{
	henBotBuilder.UseModule<GenshinModule>()
				 .UseModule<VkModule>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();