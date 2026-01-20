namespace Library.File.Core.Source.DependencyInjection;

public interface IFileSourceServiceProvider
{
    public IFileSourceProvider<IFileSourceType> GetFileSource(IFileSourceType fileSourceType);
}