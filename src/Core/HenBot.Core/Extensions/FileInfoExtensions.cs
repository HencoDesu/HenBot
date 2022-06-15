namespace HenBot.Core.Extensions;

public static class FileInfoExtensions
{
	public static async Task<byte[]> ReadBytes(this FileInfo fileInfo)
	{
		return await File.ReadAllBytesAsync(fileInfo.FullName);
	}

	public static async Task WriteBytes(this FileInfo fileInfo, byte[] bytes)
	{
		await File.WriteAllBytesAsync(fileInfo.FullName, bytes);
	} 
}