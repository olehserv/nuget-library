using Library.File.Core.Format;
using Library.File.Core.Format.DependencyInjection;
using Library.File.Core.Source;
using Library.File.Core.Source.DependencyInjection;
using Library.Management.Book;
using Library.Models;

namespace test.Library.Management;

public sealed class BookServiceTests
{
    [Fact]
    public async Task LoadAllFromFileAsync_ShouldResolveDependencies_AndReadBooks_FromStream()
    {
        // arrange
        var filePath = "books.xml";
        var ct = new CancellationTokenSource().Token;

        var fileSourceType = new TestFileSourceType();
        var fileFormatType = new TestFileFormatType();

        var readStream = new MemoryStream([1, 2, 3]);

        var expected = new List<BookModel?>
        {
            new() { Author = "A", Title = "T1" },
            new() { Author = "B", Title = "T2" }
        };

        var fileSource = new Mock<IFileSourceProvider<TestFileSourceType>>(MockBehavior.Strict);
        fileSource
            .Setup(s => s.GetReadStream(filePath, ct))
            .ReturnsAsync(readStream);

        var fileFormatProcessor = new Mock<IFileFormatProcessor<TestFileFormatType>>(MockBehavior.Strict);
        fileFormatProcessor
            .Setup(p => p.Read<BookModel>(It.IsAny<Stream>()))
            .Returns(expected);

        var fileSourceProvider = new Mock<IFileSourceServiceProvider>(MockBehavior.Strict);
        fileSourceProvider
            .Setup(p => p.GetFileSource(fileSourceType))
            .Returns(fileSource.Object);

        var fileFormatProvider = new Mock<IFileFormatProcessorServiceProvider>(MockBehavior.Strict);
        fileFormatProvider
            .Setup(p => p.GetFileFormatProcessor(fileFormatType))
            .Returns(fileFormatProcessor.Object);

        var sut = new BookService(fileSourceProvider.Object, fileFormatProvider.Object);

        // act
        var result = await sut.LoadAllFromFileAsync(filePath, fileSourceType, fileFormatType, ct);

        // assert
        result.Should().BeSameAs(expected);

        fileSourceProvider.Verify(p => p.GetFileSource(fileSourceType), Times.Once);
        fileFormatProvider.Verify(p => p.GetFileFormatProcessor(fileFormatType), Times.Once);

        fileSource.Verify(s => s.GetReadStream(filePath, ct), Times.Once);
        fileFormatProcessor.Verify(p => p.Read<BookModel>(It.Is<Stream>(st => ReferenceEquals(st, readStream))), Times.Once);

        fileSourceProvider.VerifyNoOtherCalls();
        fileFormatProvider.VerifyNoOtherCalls();
        fileSource.VerifyNoOtherCalls();
        fileFormatProcessor.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SaveAllToFileAsync_ShouldResolveDependencies_AndWriteBooks_ToStream_AndReturnTrue()
    {
        // arrange
        var filePath = "books.xml";
        var ct = new CancellationTokenSource().Token;

        var fileSourceType = new TestFileSourceType();
        var fileFormatType = new TestFileFormatType();

        var writeStream = new MemoryStream();

        var books = new List<BookModel>
        {
            new BookModel { Author = "A", Title = "T1" },
            new BookModel { Author = "B", Title = "T2" }
        };

        var fileSource = new Mock<IFileSourceProvider<TestFileSourceType>>(MockBehavior.Strict);
        fileSource
            .Setup(s => s.GetWriteStream(filePath, ct))
            .ReturnsAsync(writeStream);

        var fileFormatProcessor = new Mock<IFileFormatProcessor<TestFileFormatType>>(MockBehavior.Strict);
        fileFormatProcessor
            .Setup(p => p.Write<BookModel>(It.IsAny<Stream>(), It.IsAny<IEnumerable<BookModel>>()));

        var fileSourceProvider = new Mock<IFileSourceServiceProvider>(MockBehavior.Strict);
        fileSourceProvider
            .Setup(p => p.GetFileSource(fileSourceType))
            .Returns(fileSource.Object);

        var fileFormatProvider = new Mock<IFileFormatProcessorServiceProvider>(MockBehavior.Strict);
        fileFormatProvider
            .Setup(p => p.GetFileFormatProcessor(fileFormatType))
            .Returns(fileFormatProcessor.Object);

        var sut = new BookService(fileSourceProvider.Object, fileFormatProvider.Object);

        // act
        var ok = await sut.SaveAllToFileAsync(books, filePath, fileSourceType, fileFormatType, ct);

        // assert
        ok.Should().BeTrue();

        fileSourceProvider.Verify(p => p.GetFileSource(fileSourceType), Times.Once);
        fileFormatProvider.Verify(p => p.GetFileFormatProcessor(fileFormatType), Times.Once);

        fileSource.Verify(s => s.GetWriteStream(filePath, ct), Times.Once);
        fileFormatProcessor.Verify(
            p => p.Write<BookModel>(
                It.Is<Stream>(st => ReferenceEquals(st, writeStream)),
                It.Is<IEnumerable<BookModel>>(b => ReferenceEquals(b, books))),
            Times.Once);

        fileSourceProvider.VerifyNoOtherCalls();
        fileFormatProvider.VerifyNoOtherCalls();
        fileSource.VerifyNoOtherCalls();
        fileFormatProcessor.VerifyNoOtherCalls();
    }

    [Fact]
    public void AddToList_ShouldAddBook_ToTargetList()
    {
        // arrange
        var sut = new BookService(Mock.Of<IFileSourceServiceProvider>(), Mock.Of<IFileFormatProcessorServiceProvider>());

        var target = new List<BookModel>();
        var book = new BookModel { Author = "X", Title = "Y" };

        // act
        sut.AddToList(book, target);

        // assert
        target.Should().ContainSingle()
              .Which.Should().BeSameAs(book);
    }

    [Fact]
    public void SortByAuthorAscThanByTitleAsc_ShouldFilterNulls_AndSort()
    {
        // arrange
        var sut = new BookService(Mock.Of<IFileSourceServiceProvider>(), Mock.Of<IFileFormatProcessorServiceProvider>());

        var books = new List<BookModel>
        {
            new() { Author = "B", Title = "Z" },
            new() { Author = "A", Title = "B" },
            new() { Author = "A", Title = "A" },
            null!,
            new() { Author = "B", Title = "A" }
        };

        // act
        var sorted = sut.SortByAuthorAscThanByTitleAsc(books);

        // assert
        sorted.Should().HaveCount(4);

        sorted.Select(b => (b.Author, b.Title))
        .Should().Equal(
            ("A", "A"),
            ("A", "B"),
            ("B", "A"),
            ("B", "Z")
        );
    }

    [Fact]
    public void SearchByTitle_ShouldReturnMatches_CaseInsensitive_AndIgnoreNullBooksAndNullTitles()
    {
        // arrange
        var sut = new BookService(Mock.Of<IFileSourceServiceProvider>(), Mock.Of<IFileFormatProcessorServiceProvider>());

        var books = new List<BookModel?>
        {
            new() { Author = "A", Title = "Clean Code" },
            new() { Author = "B", Title = "The Clean Coder" },
            new() { Author = "C", Title = "Domain-Driven Design" },
            new() { Author = "D", Title = null }, // має ігноритись
            null
        };

        // act
        var found = sut.SearchByTitle(books!.Cast<BookModel>().ToList(), "cLeAn");

        // assert
        found.Should().HaveCount(2);
        found.Select(b => b.Title).Should().BeEquivalentTo(["Clean Code", "The Clean Coder"]);
    }

    [Fact]
    public void SearchByTitle_ShouldReturnEmpty_WhenNoMatches()
    {
        // arrange
        var sut = new BookService(Mock.Of<IFileSourceServiceProvider>(), Mock.Of<IFileFormatProcessorServiceProvider>());

        var books = new List<BookModel>
        {
            new() { Author = "A", Title = "AAA" },
            new() { Author = "B", Title = "BBB" },
        };

        // act
        var found = sut.SearchByTitle(books, "xyz");

        // assert
        found.Should().BeEmpty();
    }

    public class TestFileSourceType : IFileSourceType
    {
    }

    public class TestFileFormatType : IFileFormatType
    {
    }
}