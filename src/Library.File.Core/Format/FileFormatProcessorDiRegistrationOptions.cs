using Microsoft.Extensions.DependencyInjection;

namespace Library.File.Core.Format;

public class FileFormatProcessorDiRegistrationOptions
{
    private readonly IServiceCollection _services;

    private FileFormatProcessorDiRegistrationOptions(IServiceCollection services)
    {
        _services = services;
    }

    public void UseFileFormatProcessor<TFileFormatProcessor>()
        where TFileFormatProcessor : class, IFileFormatProcessor
    {
        _services.AddTransient<IFileFormatProcessor, TFileFormatProcessor>();
    }

    internal static void Configure(
        IServiceCollection services,
        Action<FileFormatProcessorDiRegistrationOptions> registerOptionsSetup)
    {
        registerOptionsSetup?.Invoke(new (services));
    }
}