using Library.File.Core.Format.DependencyInjection;
using Library.File.Format.Xml.FluentXmlSerialization;

using Microsoft.Extensions.DependencyInjection;

namespace Library.File.Format.Xml;

public static class FileFormatProcessorDiRegistrationOptionsExtension
{
    public static void UseXmlFileFormatProcessor(
        this FileFormatProcessorDiRegistrationOptions options,
        Action<IXmlConventionsBuilder>? conventionsBuilderSetup = null) 
    {
        options.UseFileFormatProcessor<XmlFileFormatProcessor, XmlFileFormatType>(
            extraServices =>
            {
                var conventionsBuilder = new XmlConventionsBuilder();
                conventionsBuilderSetup?.Invoke(conventionsBuilder);

                extraServices.AddSingleton(conventionsBuilder.Build());
                extraServices.AddSingleton<IXmlSerializerFactory, XmlSerializerFactory>();
            });
    }
}