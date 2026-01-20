namespace Library.File.Core.Format;

/// <summary>
/// File Format Processing (read/write) interface
/// </summary>
public interface IFileFormatProcessor
{
    /// <summary>
    /// Enumerable reading parsed record into type T
    /// </summary>
    /// <param name="readFileStream">File read stream</param>
    /// <typeparam name="T">Type to parse single record</typeparam>
    /// <returns></returns>
    IEnumerable<T?> Read<T>(Stream readFileStream) where T : class, new();

    /// <summary>
    /// Writing enumeration of records of T objects into write stream
    /// </summary>
    /// <param name="writeFileStream">File write stream</param>
    /// <param name="records">Data to write</param>
    /// <typeparam name="T">Type of record to parse</typeparam>
    void Write<T>(Stream writeFileStream, IEnumerable<T> records) where T : class, new();
}