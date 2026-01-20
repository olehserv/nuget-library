using Library.File.Core;
using Library.File.Core.Format.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace test.Library.File.Core;

public sealed class FileDependenciesRegistrationsOptionsTests
{
    [Fact]
    public void Configure_should_throw_ArgumentNullException_when_services_is_null()
    {
        // arrange
        IServiceCollection? services = null;

        // act
        var exc = Record.Exception(() => FileDependenciesRegistrationsOptions.Configure(
            services!,
            _ => { }));

        // assert
        exc.Should().NotBeNull();
    }

    [Fact]
    public void Configure_should_invoke_registration_setup_delegate()
    {
        // arrange
        var services = new ServiceCollection();

        var setupMock = new Mock<Action<FileDependenciesRegistrationsOptions>>(MockBehavior.Strict);
        setupMock.Setup(x => x(It.IsAny<FileDependenciesRegistrationsOptions>()));

        // act
        FileDependenciesRegistrationsOptions.Configure(services, setupMock.Object);

        // assert
        setupMock.Verify(x => x(It.IsAny<FileDependenciesRegistrationsOptions>()), Times.Once);
    }

    [Fact]
    public void Configure_should_pass_options_that_operate_on_the_same_IServiceCollection_instance()
    {
        // arrange
        var services = new ServiceCollection();
        IServiceCollection? servicesSeenInside = null;

        // act
        FileDependenciesRegistrationsOptions.Configure(services, opts =>
        {
            servicesSeenInside = opts.AddFileFormatProcessor(_ => { });
        });

        // assert
        servicesSeenInside.Should().NotBeNull();
        ReferenceEquals(services, servicesSeenInside).Should().BeTrue();
    }

    [Fact]
    public void AddFileFormatProcessor_should_throw_ArgumentNullException_when_setup_is_null()
    {
        // arrange
        var services = new ServiceCollection();

        FileDependenciesRegistrationsOptions.Configure(services, opts =>
        {
            // act
            var exc = Record.Exception(() => opts.AddFileFormatProcessor(null!));

            // assert
            exc.Should().NotBeNull();
        });
    }

    [Fact]
    public void AddFileFormatProcessor_should_return_same_service_collection_and_execute_action_and_register_provider()
    {
        // arrange
        var services = new ServiceCollection();
        var executed = false;

        // act
        IServiceCollection? returned = null;

        FileDependenciesRegistrationsOptions.Configure(services, opts =>
        {
            returned = opts.AddFileFormatProcessor(_ =>
            {
                executed = true;
            });
        });

        // assert
        returned.Should().NotBeNull();
        ReferenceEquals(services, returned).Should().BeTrue();

        executed.Should().BeTrue();

        services.Any(d => d.ServiceType == typeof(IFileFormatProcessorServiceProvider))
            .Should().BeTrue();
    }

    [Fact]
    public void AddFileSource_should_throw_ArgumentNullException_when_setup_is_null()
    {
        // arrange
        var services = new ServiceCollection();

        FileDependenciesRegistrationsOptions.Configure(services, opts =>
        {
            // act
            var exc = Record.Exception(() => opts.AddFileSource(null!));

            // assert
            exc.Should().NotBeNull();
        });
    }

    [Fact]
    public void AddFileSource_should_return_same_service_collection_for_valid_setup()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        IServiceCollection? returned = null;

        FileDependenciesRegistrationsOptions.Configure(services, opts =>
        {
            returned = opts.AddFileSource(_ => { });
        });

        // assert
        returned.Should().NotBeNull();
        ReferenceEquals(services, returned).Should().BeTrue();
    }
}