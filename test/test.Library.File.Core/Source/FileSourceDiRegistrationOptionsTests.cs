using Library.File.Core.Source;
using Library.File.Core.Source.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace test.Library.File.Core.Source;

public sealed class FileSourceDiRegistrationOptionsTests
{
    [Fact]
    public void Configure_ShouldAddIFileSourceServiceProviderAsSingleton_AndInvokeSetup()
    {
        // Arrange
        var services = new ServiceCollection();
        var invoked = false;

        // Act
        FileSourceDiRegistrationOptions.Configure(
            services,
            _ => invoked = true);

        // Assert
        invoked.Should().BeTrue();

        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IFileSourceServiceProvider));
        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Singleton);
        descriptor.ImplementationType.Should().Be<FileSourceServiceProvider>();
    }

    [Fact]
    public void Configure_ShouldNotOverrideExistingIFileSourceServiceProviderRegistration()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IFileSourceServiceProvider, CustomFileSourceServiceProvider>();

        // Act
        FileSourceDiRegistrationOptions.Configure(services, _ => { });

        // Assert
        var descriptors = services.Where(d => d.ServiceType == typeof(IFileSourceServiceProvider)).ToList();
        descriptors.Should().HaveCount(1);

        descriptors[0].ImplementationType.Should().Be<CustomFileSourceServiceProvider>();
        descriptors[0].Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void Configure_WithUseSource_ShouldRegisterTransientProvider()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        FileSourceDiRegistrationOptions.Configure(
            services,
            opts => opts.UseSource<TestFileSourceProvider, TestFileSourceType>());

        // Assert
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IFileSourceProvider<TestFileSourceType>));
        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Transient);
        descriptor.ImplementationType.Should().Be<TestFileSourceProvider>();
    }

    [Fact]
    public void Configure_WithUseSource_ShouldResolveProviderViaIFileSourceServiceProvider()
    {
        // Arrange
        var services = new ServiceCollection();

        FileSourceDiRegistrationOptions.Configure(
            services,
            opts => opts.UseSource<TestFileSourceProvider, TestFileSourceType>());

        using var sp = services.BuildServiceProvider();

        var sut = sp.GetRequiredService<IFileSourceServiceProvider>();

        // Act
        var result = sut.GetFileSource(new TestFileSourceType());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TestFileSourceProvider>();
    }

    public sealed class TestFileSourceType : IFileSourceType { }

    public sealed class TestFileSourceProvider : IFileSourceProvider<TestFileSourceType>
    {
        public Task<Stream> GetReadStream(string filePath, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task<Stream> GetWriteStream(string toFilePath, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
    }

    public sealed class CustomFileSourceServiceProvider : IFileSourceServiceProvider
    {
        public IFileSourceProvider<IFileSourceType> GetFileSource(IFileSourceType fileSourceType)
            => throw new NotImplementedException();
    }
}