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

    public async Task<IEnumerable<BookModel?>> LoadAllFromFileAsync(        
        string filePath, 
        IFileSourceType fileSourceType, 
        IFileFormatType fileFormatType, 
        CancellationToken cancellationToken = default)
    {
        var fileSource = _fileSourceServiceProvider.GetFileSource(fileSourceType);
        var fileFormatProcessor = _fileFormatProcessorServiceProvider.GetFileFormatProcessor(fileFormatType);

        using var stream = await fileSource.GetReadStream(filePath, cancellationToken);
        
        return fileFormatProcessor.Read<BookModel>(stream);
    }

    public void AddToList(BookModel book, IList<BookModel> targetBooksList)
    {
        // hardest part of this project =)
        targetBooksList.Add(book);
    }

    public IList<BookModel> SortByAuthorAscThanByTitleAsc(IList<BookModel> books)
    {
        // that is a bit easier =)
        return books
            .Where(b => b is not null)
            .OrderBy(b => b.Author)
            .ThenBy(b => b.Title)
            .ToList();
    }

    public IList<BookModel> SearchByTitle(IList<BookModel> books, string titlePart)
    {
        return books
            .Where(b => b is not null && b.Title is not null)
            .Where(b => b.Title!.Contains(titlePart, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public async Task<bool> SaveAllToFileAsync(IEnumerable<BookModel> books,         
        string filePath, 
        IFileSourceType fileSourceType, 
        IFileFormatType fileFormatType, 
        CancellationToken cancellationToken = default)
    {
        var fileSource = _fileSourceServiceProvider.GetFileSource(fileSourceType);
        var fileFormatProcessor = _fileFormatProcessorServiceProvider.GetFileFormatProcessor(fileFormatType);

        using var stream = await fileSource.GetWriteStream(filePath, cancellationToken);

        fileFormatProcessor.Write(stream, books);
        return true;
    }
}