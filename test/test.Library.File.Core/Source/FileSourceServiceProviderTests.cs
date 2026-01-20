using Library.File.Core.Source;
using Library.File.Core.Source.DependencyInjection;

namespace test.Library.File.Core.Source;

public sealed class FileSourceServiceProviderTests
{
    public sealed class TestFileSourceType : IFileSourceType { }

    [Fact]
    public void GetFileSource_ShouldResolveProviderBasedOnRuntimeType_AndReturnSameInstance()
    {
        // Arrange
        var fileSourceType = new TestFileSourceType();
        var expectedServiceType = typeof(IFileSourceProvider<TestFileSourceType>);

        var providerMock = new Mock<IFileSourceProvider<TestFileSourceType>>(MockBehavior.Strict);

        var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
        serviceProviderMock
            .Setup(sp => sp.GetService(expectedServiceType))
            .Returns(providerMock.Object);

        var sut = new FileSourceServiceProvider(serviceProviderMock.Object);

        // Act
        IFileSourceProvider<IFileSourceType> result = sut.GetFileSource(fileSourceType);

        // Assert
        result.Should().BeSameAs(providerMock.Object);

        serviceProviderMock.Verify(sp => sp.GetService(expectedServiceType), Times.Once);
        serviceProviderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void GetFileSource_WhenProviderIsNotRegistered_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var fileSourceType = new TestFileSourceType();
        var expectedServiceType = typeof(IFileSourceProvider<TestFileSourceType>);

        var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
        serviceProviderMock
            .Setup(sp => sp.GetService(expectedServiceType))
            .Returns(null!);

        var sut = new FileSourceServiceProvider(serviceProviderMock.Object);

        // Act
        var exc = Record.Exception(() => sut.GetFileSource(fileSourceType));

        // Assert
        exc.Should().NotBeNull();

        serviceProviderMock.Verify(sp => sp.GetService(expectedServiceType), Times.Once);
        serviceProviderMock.VerifyNoOtherCalls();
    }
}