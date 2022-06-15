using Microsoft.Extensions.Hosting;

namespace HenBot.Core.Providers.FileProvider;

public interface IFileProvider : IHostedService
{
	FileInfo CreateTempFile(string? fileName = null, string? extension = null);
}