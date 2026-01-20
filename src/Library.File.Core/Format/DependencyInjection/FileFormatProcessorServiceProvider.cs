using Microsoft.Extensions.DependencyInjection;

namespace Library.File.Core.Format.DependencyInjection;

internal sealed class FileFormatProcessorServiceProvider : IFileFormatProcessorServiceProvider
{
    private readonly IServiceProvider _serviceProvider;

    public FileFormatProcessorServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IFileFormatProcessor<IFileFormatType> GetFileFormatProcessor(IFileFormatType fileFormatType)
    {
        Type fileProcessorType = typeof(IFileFormatProcessor<>).MakeGenericType(fileFormatType.GetType());

        return (IFileFormatProcessor<IFileFormatType>)_serviceProvider.GetRequiredService(fileProcessorType);
    }
}