using Library.File.Core.Source;

namespace Library.File.Source.Physical;

public static class FileSourceDiRegistrationOptionsExtension
{
    public static void UsePhysicalFileSource(this FileSourceDiRegistrationOptions options)
    {
        options.UseSource<PhysicalFileSource>();
    }
}