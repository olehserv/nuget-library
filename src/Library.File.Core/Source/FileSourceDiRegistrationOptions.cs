using Microsoft.Extensions.DependencyInjection;

namespace Library.File.Core.Source;

public class FileSourceDiRegistrationOptions
{
    private readonly IServiceCollection _services;

    private FileSourceDiRegistrationOptions(IServiceCollection services)
    {
        _services = services;
    }

    public void UseSource<TFileSource>()
        where TFileSource : class, IFileSource
    {
        _services.AddTransient<IFileSource, TFileSource>();
    }

    internal static void Configure(IServiceCollection services,
        Action<FileSourceDiRegistrationOptions> fileSourceRegistrationSetup)
    {
        fileSourceRegistrationSetup.Invoke(new(services));
    }
}
