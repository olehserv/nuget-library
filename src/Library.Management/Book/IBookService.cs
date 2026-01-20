namespace Library.Core.Models;

public interface IBookService
{
    Task<IEnumerable<BookModel>> LoadAllFromFileAsync(string filePath, CancellationToken cancellationToken = default);

    void AddToList(BookModel book, IList<BookModel> targetBooksList);

    void SortByAuthorAscThanByTitleAsc(IList<BookModel> books);

    Task<bool> SaveAllToFileAsync(IEnumerable<BookModel> books, CancellationToken cancellationToken = default);
}