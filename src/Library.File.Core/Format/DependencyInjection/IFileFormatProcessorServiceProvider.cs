namespace Library.File.Core.Format.DependencyInjection;

public interface IFileFormatProcessorServiceProvider
{
    public IFileFormatProcessor<IFileFormatType> GetFileFormatProcessor(IFileFormatType fileFormatType);
}