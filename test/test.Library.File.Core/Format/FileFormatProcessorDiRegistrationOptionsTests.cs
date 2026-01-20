using Library.File.Core.Format;
using Library.File.Core.Format.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace test.Library.File.Core.Format;

file sealed class CsvFormat : IFileFormatType { }

file sealed class CsvProcessor : IFileFormatProcessor<CsvFormat>
{
    public IEnumerable<T?> Read<T>(Stream readFileStream) where T : class, new() => [];

    public void Write<T>(Stream writeFileStream, IEnumerable<T> records) where T : class, new() { }
}

public sealed class FileFormatProcessorDiRegistrationOptionsTests
{
    [Fact]
    public void Configure_should_register_IFileFormatProcessorServiceProvider_as_singleton()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        FileFormatProcessorDiRegistrationOptions.Configure(services, _ => { });

        // assert
        var descriptors = services
            .Where(d => d.ServiceType == typeof(IFileFormatProcessorServiceProvider))
            .ToList();

        descriptors.Should().HaveCount(1);
        descriptors[0].Lifetime.Should().Be(ServiceLifetime.Singleton);
        descriptors[0].ImplementationType.Should().Be<FileFormatProcessorServiceProvider>();
    }

    [Fact]
    public void UseFileFormatProcessor_should_register_processor_as_transient_for_specific_format_type()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        FileFormatProcessorDiRegistrationOptions.Configure(
            services, 
            opts => opts.UseFileFormatProcessor<CsvProcessor, CsvFormat>());

        // assert
        var descriptor = services.Single(d => d.ServiceType == typeof(IFileFormatProcessor<CsvFormat>));

        descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
        descriptor.ImplementationType.Should().Be<CsvProcessor>();
    }

    [Fact]
    public void Configure_should_not_duplicate_IFileFormatProcessorServiceProvider_registration_when_called_twice()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        FileFormatProcessorDiRegistrationOptions.Configure(services, _ => { });
        FileFormatProcessorDiRegistrationOptions.Configure(services, _ => { });

        // assert
        services.Count(d => d.ServiceType == typeof(IFileFormatProcessorServiceProvider))
            .Should().Be(1);
    }
}