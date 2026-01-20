namespace Library.File.Format.Xml.FluentXmlSerialization;

public interface IXmlConventionsBuilder
{
    IXmlConventionsBuilder Default(string rootName, string itemName);
    IXmlConventionsBuilder For<T>(string? rootName = null, string? itemName = null) where T : class, new();
    IXmlSerializationConventions Build();
}