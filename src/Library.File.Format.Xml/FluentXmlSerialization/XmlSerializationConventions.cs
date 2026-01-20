namespace Library.File.Format.Xml.FluentXmlSerialization;

internal sealed class XmlSerializationConventions(
    XmlConventions defaultConventions, 
    IReadOnlyDictionary<Type, XmlConventions> map) 
    : IXmlSerializationConventions
{
    private readonly XmlConventions _defaultConventions = defaultConventions;
    private readonly IReadOnlyDictionary<Type, XmlConventions> _modelXmlSchemaMap = map;

    public XmlConventions GetFor(Type recordType)
        => _modelXmlSchemaMap.TryGetValue(recordType, out var v) ? v : _defaultConventions;
}