using System;

using Library.Core;
using Library.Core.Models;

namespace Library.Management.Book;

public class BookService : IBookService
{
    public void AddToList(BookModel book, IList<BookModel> targetBooksList)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BookModel>> LoadAllFromFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveAllToFileAsync(IEnumerable<BookModel> books, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void SortByAuthorAscThanByTitleAsc(IList<BookModel> books)
    {
        throw new NotImplementedException();
    }
}
