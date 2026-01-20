namespace Library.File.Core.Source;

/// <summary>
/// File source access interface 
/// </summary>
public interface IFileSource
{
    Task<Stream> GetReadStream(string filePath, CancellationToken cancellationToken = default);

    Task<Stream> GetWriteStream(string toFilePath, CancellationToken cancellationToken = default);
}