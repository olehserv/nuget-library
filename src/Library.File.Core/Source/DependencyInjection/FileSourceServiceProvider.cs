using Microsoft.Extensions.DependencyInjection;

namespace Library.File.Core.Source.DependencyInjection;

internal sealed class FileSourceServiceProvider : IFileSourceServiceProvider
{
    private readonly IServiceProvider _serviceProvider;

    public FileSourceServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IFileSourceProvider<IFileSourceType> GetFileSource(IFileSourceType fileSourceType)
    {
        Type fileSourceProviderType = typeof(IFileSourceProvider<>).MakeGenericType(fileSourceType.GetType());

        return (IFileSourceProvider<IFileSourceType>)_serviceProvider.GetRequiredService(fileSourceProviderType);
    }
}