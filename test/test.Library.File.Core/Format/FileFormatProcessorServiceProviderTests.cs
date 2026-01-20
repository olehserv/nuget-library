using Library.File.Core.Format;
using Library.File.Core.Format.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace test.Library.File.Core.Format;

public sealed class FileFormatProcessorServiceProviderTests
{
    private sealed class CsvFormat : IFileFormatType { }

    private sealed class CsvProcessor : IFileFormatProcessor<CsvFormat>
    {
        public IEnumerable<T?> Read<T>(Stream readFileStream) where T : class, new() => [];

        public void Write<T>(Stream writeFileStream, IEnumerable<T> records) where T : class, new() { }
    }

    [Fact]
    public void GetFileFormatProcessor_should_resolve_processor_by_runtime_format_type()
    {
        // arrange
        var services = new ServiceCollection();

        FileFormatProcessorDiRegistrationOptions.Configure(
            services, 
            opts => opts.UseFileFormatProcessor<CsvProcessor, CsvFormat>());

        var sp = services.BuildServiceProvider();
        var provider = sp.GetRequiredService<IFileFormatProcessorServiceProvider>();

        // act
        var processor = provider.GetFileFormatProcessor(new CsvFormat());

        // assert
        processor.Should().NotBeNull();
        processor.Should().BeOfType<CsvProcessor>();
    }

    [Fact]
    public void GetFileFormatProcessor_should_throw_when_processor_not_registered()
    {
        // arrange
        var services = new ServiceCollection();
        FileFormatProcessorDiRegistrationOptions.Configure(services, _ => { });

        var sp = services.BuildServiceProvider();
        var provider = sp.GetRequiredService<IFileFormatProcessorServiceProvider>();

        // act
        var exc = Record.Exception(() => provider.GetFileFormatProcessor(new CsvFormat()));

        // assert
        exc.Should().NotBeNull();
    }
}