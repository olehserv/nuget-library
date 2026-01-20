using Library.File.Core.Source.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Library.File.Core.Source;

public sealed class FileSourceDiRegistrationOptions
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

    internal static void Configure(
        IServiceCollection services,
        Action<FileSourceDiRegistrationOptions> fileSourceRegistrationSetup)
    {
        services.TryAddSingleton<IFileSourceServiceProvider, FileSourceServiceProvider>();

        fileSourceRegistrationSetup.Invoke(new(services));
    }
}