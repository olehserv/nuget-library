using Library.File.Core.Format;
using Library.File.Core.Source;
using Library.Models;

namespace Library.Management.Book;

public interface IBookService
{
    Task<IEnumerable<BookModel?>> LoadAllFromFileAsync(
        string filePath, 
        IFileSourceType fileSourceType, 
        IFileFormatType fileFormatType, 
        CancellationToken cancellationToken = default);

    void AddToList(BookModel book, IList<BookModel> targetBooksList);

    IList<BookModel> SortByAuthorAscThanByTitleAsc(IList<BookModel> books);

    IList<BookModel> SearchByTitle(IList<BookModel> books, string titlePart);

    Task<bool> SaveAllToFileAsync(
        IEnumerable<BookModel> books,         
        string filePath, 
        IFileSourceType fileSourceType, 
        IFileFormatType fileFormatType, 
        CancellationToken cancellationToken = default);
}