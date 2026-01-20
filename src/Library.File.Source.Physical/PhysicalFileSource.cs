using Library.File.Core.Source;

namespace Library.File.Source.Physical;

///<inheritdoc/>
internal class PhysicalFileSource : IFileSource
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

    private void EnsureFileExist(string filePath)
    {
        if (System.IO.File.Exists(filePath)) return;

        throw new FileNotFoundException("File not found", Path.GetFileName(filePath));
    }

    private void EnsureDirectoryExists(string filePath)
    {
        string? directory = Path.GetDirectoryName(filePath);

        if (directory is null) return;

        Directory.CreateDirectory(directory);
    }
}