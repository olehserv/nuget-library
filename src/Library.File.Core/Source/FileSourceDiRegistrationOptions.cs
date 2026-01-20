using Microsoft.Extensions.DependencyInjection;

namespace Library.File.Core.Source;

public class FileSourceDiRegistrationOptions
{
    private readonly IServiceCollection _services;

    private FileSourceDiRegistrationOptions(IServiceCollection services)
    {
        _services = services;
    }

    public void UseSource<TFileSourceProvider, TFileSourceType>()
        where TFileSourceProvider : class, IFileSourceProvider<TFileSourceType>
        where TFileSourceType : class, IFileSourceType
    {
        _services.AddTransient<IFileSourceProvider<TFileSourceType>, TFileSourceProvider>();
    }

    internal static void Configure(IServiceCollection services,
        Action<FileSourceDiRegistrationOptions> fileSourceRegistrationSetup)
    {
        fileSourceRegistrationSetup.Invoke(new(services));
    }
}
