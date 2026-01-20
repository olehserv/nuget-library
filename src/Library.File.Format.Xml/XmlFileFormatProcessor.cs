using Library.File.Core.Format;
using Library.File.Format.Xml.FluentXmlSerialization;

using System.Xml.Serialization;

namespace Library.File.Format.Xml;

///<inheritdoc/>
internal sealed class XmlFileFormatProcessor(IXmlSerializerFactory xmlSerializerFactory) 
: IFileFormatProcessor<XmlFileFormatType>
{
    private readonly IXmlSerializerFactory _xmlSerializerFactory = xmlSerializerFactory;

    public IEnumerable<T?> Read<T>(Stream readFileStream) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(readFileStream);

        if (!readFileStream.CanRead)
            throw new ArgumentException("Impossible to read from stream.", nameof(readFileStream));

        if (readFileStream.CanSeek)
            readFileStream.Position = 0;

        if (readFileStream.CanSeek && readFileStream.Length == 0)
            return [];

        var serializer = _xmlSerializerFactory.CreateForRecordsWrapper(typeof(T));

        var data = serializer.Deserialize(readFileStream) as XmlRecordsWrapper<T>;

        if (data?.Records is null)
            return [];

        return data.Records;
    }

    public void Write<T>(Stream writeFileStream, IEnumerable<T> records) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(writeFileStream);

        ArgumentNullException.ThrowIfNull(records);

        if (!writeFileStream.CanWrite)
            throw new ArgumentException("Impossible to write to stream.", nameof(writeFileStream));

        if (writeFileStream.CanSeek)
        {
            writeFileStream.SetLength(0);
            writeFileStream.Position = 0;
        }

        var serializer = _xmlSerializerFactory.CreateForRecordsWrapper(typeof(T));

        var wrapper = new XmlRecordsWrapper<T>
        {
            Records = records.ToList()
        };

        // To avoid adding xmlns:xsi and xmlns:xsd namespaces
        var ns = new XmlSerializerNamespaces();
        ns.Add(string.Empty, string.Empty);

        serializer.Serialize(writeFileStream, wrapper, ns);

        if (writeFileStream.CanSeek)
        {
            writeFileStream.Position = 0;
        }
    }
}