namespace Library.File.Format.Xml.FluentXmlSerialization;

internal sealed class XmlConventionsBuilder : IXmlConventionsBuilder
{
    private static XmlConventions _defaultConventions = new("records", "record");
    private readonly Dictionary<Type, XmlConventions> _modelXmlSchemaMap = [];

    public IXmlConventionsBuilder Default(string rootName, string itemName)
    {
        _defaultConventions = new XmlConventions(rootName, itemName);
        return this;
    }

    public IXmlConventionsBuilder For<T>(string? rootName = null, string? itemName = null) where T : class, new()
    {
        var t = typeof(T);

        if (_modelXmlSchemaMap.ContainsKey(t))
            throw new InvalidOperationException($"Conventions for type '{t.FullName}' have already been defined.");
        
        _modelXmlSchemaMap.Add(t, 
            new XmlConventions(
                rootName ?? _defaultConventions.RootName,
                itemName ?? _defaultConventions.ItemName
            ));

        return this;
    }

    public IXmlSerializationConventions Build()
        => new XmlSerializationConventions(_defaultConventions, _modelXmlSchemaMap);
}