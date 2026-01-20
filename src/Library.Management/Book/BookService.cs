using Library.File.Core.Format;
using Library.File.Core.Format.DependencyInjection;
using Library.File.Core.Source;
using Library.File.Core.Source.DependencyInjection;
using Library.Models;

namespace Library.Management.Book;

internal sealed class BookService : IBookService
{
    private readonly IFileSourceServiceProvider _fileSourceServiceProvider;
    private readonly IFileFormatProcessorServiceProvider _fileFormatProcessorServiceProvider;

    public BookService(
        IFileSourceServiceProvider fileSourceServiceProvider, 
        IFileFormatProcessorServiceProvider fileFormatProcessorServiceProvider)
    {
        _fileSourceServiceProvider = fileSourceServiceProvider;
        _fileFormatProcessorServiceProvider = fileFormatProcessorServiceProvider;
    }

    public Task<IEnumerable<BookModel>> LoadAllFromFileAsync(        
        string filePath, 
        IFileSourceType fileSourceType, 
        IFileFormatType fileFormatType, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void AddToList(BookModel book, IList<BookModel> targetBooksList)
    {
        throw new NotImplementedException();
    }

    public IList<BookModel> SortByAuthorAscThanByTitleAsc(IList<BookModel> books)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveAllToFileAsync(IEnumerable<BookModel> books,         
        string filePath, 
        IFileSourceType fileSourceType, 
        IFileFormatType fileFormatType, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}