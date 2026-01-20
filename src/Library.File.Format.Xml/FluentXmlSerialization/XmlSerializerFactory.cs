using System.Xml.Serialization;

namespace Library.File.Format.Xml.FluentXmlSerialization;

internal sealed class XmlSerializerFactory(IXmlSerializationConventions conventions) : IXmlSerializerFactory
{
    private readonly IXmlSerializationConventions _conventions = conventions;

    public XmlSerializer CreateForRecordsWrapper(Type recordType)
    {
        var convention = _conventions.GetFor(recordType);

        var wrapperType = typeof(XmlRecordsWrapper<>).MakeGenericType(recordType);

        var overrides = new XmlAttributeOverrides();

        var rootAttrs = new XmlAttributes
        {
            XmlRoot = new XmlRootAttribute(convention.RootName)
        };
        overrides.Add(wrapperType, rootAttrs);

        var recordsProp = wrapperType.GetProperty(nameof(XmlRecordsWrapper<>.Records))!;

        var recordsAttrs = new XmlAttributes();

        recordsAttrs.XmlElements.Add(new XmlElementAttribute(convention.ItemName, recordType));
        
        overrides.Add(wrapperType, recordsProp.Name, recordsAttrs);

        return new XmlSerializer(wrapperType, overrides);
    }
}