using Microsoft.Extensions.DependencyInjection;

namespace Library.File.Core.Source.DependencyInjection;

internal sealed class FileSourceServiceProvider(IServiceProvider serviceProvider) : IFileSourceServiceProvider
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IFileSourceProvider<IFileSourceType> GetFileSource(IFileSourceType fileSourceType)
    {
        Type fileSourceProviderType = typeof(IFileSourceProvider<>).MakeGenericType(fileSourceType.GetType());

        return (IFileSourceProvider<IFileSourceType>)_serviceProvider.GetRequiredService(fileSourceProviderType);
    }
}