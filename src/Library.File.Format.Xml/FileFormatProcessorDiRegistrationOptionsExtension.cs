using Library.File.Core.Format;

namespace Library.File.Format.Xml;

public static class FileFormatProcessorDiRegistrationOptionsExtension
{
    public static void UseXmlFileFormatProcessor(this FileFormatProcessorDiRegistrationOptions options) 
    // todo: add param to setup serialization schema settings for each model and registed default
    {
        options.UseFileFormatProcessor<XmlFileFormatProcessor>();
    }
}
