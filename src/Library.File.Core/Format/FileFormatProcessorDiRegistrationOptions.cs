using Microsoft.Extensions.DependencyInjection;

namespace Library.File.Core.Format;

public class FileFormatProcessorDiRegistrationOptions
{
    private readonly IServiceCollection _services;

    private FileFormatProcessorDiRegistrationOptions(IServiceCollection services)
    {
        _services = services;
    }

    public void UseFileFormatProcessor<TFileFormatProcessor, TFileFormatType>()
        where TFileFormatProcessor : class, IFileFormatProcessor<TFileFormatType>
        where TFileFormatType : class, IFileFormatType
    {
        _services.AddTransient<IFileFormatProcessor<TFileFormatType>, TFileFormatProcessor>();
    }

    internal static void Configure(
        IServiceCollection services,
        Action<FileFormatProcessorDiRegistrationOptions> registerOptionsSetup)
    {
        registerOptionsSetup?.Invoke(new (services));
    }
}