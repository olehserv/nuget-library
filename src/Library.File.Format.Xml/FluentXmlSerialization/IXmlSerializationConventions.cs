namespace Library.File.Format.Xml.FluentXmlSerialization;

public interface IXmlSerializationConventions
{
    XmlConventions GetFor(Type recordType);
}