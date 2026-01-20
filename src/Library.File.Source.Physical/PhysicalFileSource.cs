using Library.File.Core.Source;

namespace Library.File.Source.Physical;

///<inheritdoc/>
internal sealed class PhysicalFileSource : IFileSourceProvider<PhysicalFileSourceType>
{
    public async Task<Stream> GetReadStream(string filePath, CancellationToken cancellationToken = default)
    {
        EnsureFileExist(filePath);
        return await Task.FromResult(System.IO.File.OpenRead(filePath) as Stream);
    }

    public async Task<Stream> GetWriteStream(string toFilePath, CancellationToken cancellationToken = default)
    {
        EnsureDirectoryExists(toFilePath);
        return await Task.FromResult(System.IO.File.OpenWrite(toFilePath) as Stream);
    }

    private static void EnsureFileExist(string filePath)
    {
        if (System.IO.File.Exists(filePath)) return;

        throw new FileNotFoundException("File not found", Path.GetFileName(filePath));
    }

    private static void EnsureDirectoryExists(string filePath)
    {
        string? directory = Path.GetDirectoryName(filePath);

        if (string.IsNullOrWhiteSpace(directory)) return;

        Directory.CreateDirectory(directory);
    }
}