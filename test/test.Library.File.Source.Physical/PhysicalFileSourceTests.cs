using System.Text;

using Library.File.Source.Physical;

namespace test.Library.File.Source.Physical;

public sealed class PhysicalFileSourceTests
{
    [Fact]
    public async Task GetReadStream_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
    {
        // arrange
        var sut = new PhysicalFileSource();
        var missingPath = Path.Combine(Path.GetTempPath(), GetType().Name, Guid.NewGuid().ToString("N"), "missing.txt");

        // act
        var exc = await Record.ExceptionAsync(async () =>
        { 
            await using var _ = await sut.GetReadStream(missingPath);
        });

        // assert
        exc.Should().BeOfType<FileNotFoundException>();
    }

    [Fact]
    public async Task GetReadStream_ShouldReturnReadableStream_WhenFileExists()
    {
        // arrange
        var sut = new PhysicalFileSource();
        var tempDir = Path.Combine(Path.GetTempPath(), GetType().Name, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        var filePath = Path.Combine(tempDir, "input.txt");
        var content = "hello read";
        await System.IO.File.WriteAllTextAsync(filePath, content, Encoding.UTF8);

        // act
        await using var stream = await sut.GetReadStream(filePath);

        // assert
        stream.CanRead.Should().BeTrue();
        using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
        var read = await reader.ReadToEndAsync();
        read.Should().Be(content);
    }

    [Fact]
    public async Task GetWriteStream_ShouldCreateDirectoryAndAllowWriting_WhenDirectoryDoesNotExist()
    {
        // arrange
        var sut = new PhysicalFileSource();
        var tempDir = Path.Combine(Path.GetTempPath(), GetType().Name, Guid.NewGuid().ToString("N"));
        var nestedDir = Path.Combine(tempDir, "a", "b", "c");
        var filePath = Path.Combine(nestedDir, "out.txt");

        Directory.Exists(nestedDir).Should().BeFalse();

        // act
        await using (var stream = await sut.GetWriteStream(filePath))
        {
            stream.CanWrite.Should().BeTrue();

            var bytes = Encoding.UTF8.GetBytes("hello write");
            await stream.WriteAsync(bytes, 0, bytes.Length);
            await stream.FlushAsync();
        }

        // assert
        Directory.Exists(nestedDir).Should().BeTrue();
        System.IO.File.Exists(filePath).Should().BeTrue();

        var text = await System.IO.File.ReadAllTextAsync(filePath, Encoding.UTF8);
        text.Should().Be("hello write");
    }

    [Fact]
    public async Task GetWriteStream_ShouldNotThrow_WhenDirectoryNameIsNull()
    {
        // arrange
        var sut = new PhysicalFileSource();

        var tempDir = Path.Combine(Path.GetTempPath(), GetType().Name, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        var oldCwd = Environment.CurrentDirectory;
        Environment.CurrentDirectory = tempDir;

        try
        {
            var fileNameOnly = "cwd-out.txt";

            // act
            await using (var stream = await sut.GetWriteStream(fileNameOnly))
            {
                var bytes = Encoding.UTF8.GetBytes("cwd");
                await stream.WriteAsync(bytes, 0, bytes.Length);
                await stream.FlushAsync();
            }

            // assert
            System.IO.File.Exists(Path.Combine(tempDir, fileNameOnly)).Should().BeTrue();
            (await System.IO.File.ReadAllTextAsync(Path.Combine(tempDir, fileNameOnly), Encoding.UTF8)).Should().Be("cwd");
        }
        finally
        {
            Environment.CurrentDirectory = oldCwd;
        }
    }
}