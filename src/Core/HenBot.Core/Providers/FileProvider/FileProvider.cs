namespace HenBot.Core.Providers.FileProvider;

public class FileProvider : IFileProvider
{
	private readonly DirectoryInfo _tempFolder;

	public FileProvider()
	{
		var rootFolder = new DirectoryInfo("Files");
		if (!rootFolder.Exists)
		{
			rootFolder.Create();
		}

		_tempFolder = rootFolder.CreateSubdirectory("Temp");
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_tempFolder.Delete(true);
		return Task.CompletedTask;
	}

	public FileInfo CreateTempFile(string? fileName = null, string? extension = null)
	{
		fileName ??= Guid.NewGuid().ToString();
		extension ??= string.Empty;
		return new FileInfo(Path.Combine(_tempFolder.FullName, $"{fileName}{extension}"));
	}
}