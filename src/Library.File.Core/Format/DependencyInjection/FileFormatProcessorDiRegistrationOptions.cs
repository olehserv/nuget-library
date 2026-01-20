using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Library.File.Core.Format.DependencyInjection;

public sealed class FileFormatProcessorDiRegistrationOptions
{
    private readonly IServiceCollection _services;

    private FileFormatProcessorDiRegistrationOptions(IServiceCollection services)
    {
        _services = services;
    }

    public void UseFileFormatProcessor<TFileFormatProcessor, TFileFormatType>(
        Action<IServiceCollection>? servicesExtraSetup = null)
        where TFileFormatProcessor : class, IFileFormatProcessor<TFileFormatType>
        where TFileFormatType : class, IFileFormatType
    {
        _services.AddTransient<IFileFormatProcessor<TFileFormatType>, TFileFormatProcessor>();
        servicesExtraSetup?.Invoke(_services);
    }

    internal static void Configure(
        IServiceCollection services,
        Action<FileFormatProcessorDiRegistrationOptions> registerOptionsSetup)
    {
        services.TryAddSingleton<IFileFormatProcessorServiceProvider, FileFormatProcessorServiceProvider>();
        registerOptionsSetup?.Invoke(new (services));
    }
}