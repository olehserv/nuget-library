using System.Xml.Serialization;

namespace Library.File.Format.Xml.FluentXmlSerialization;

public interface IXmlSerializerFactory
{
    XmlSerializer CreateForRecordsWrapper(Type recordType);
}