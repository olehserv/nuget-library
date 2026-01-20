using Library.File.Core.Format.DependencyInjection;
using Library.File.Core.Source;

using Microsoft.Extensions.DependencyInjection;

namespace Library.File.Core;

public sealed class FileDependenciesRegistrationsOptions
{
    private readonly IServiceCollection _services;

    private FileDependenciesRegistrationsOptions(IServiceCollection services)
    {
        _services = services;
    }

    public IServiceCollection AddFileFormatProcessor(Action<FileFormatProcessorDiRegistrationOptions> fileProcessorDiSetup)
	{
		ArgumentNullException.ThrowIfNull(fileProcessorDiSetup);

		FileFormatProcessorDiRegistrationOptions.Configure(_services, fileProcessorDiSetup);
		return _services;
	}

	public IServiceCollection AddFileSource(Action<FileSourceDiRegistrationOptions> repositoryDiSetup)
	{
		ArgumentNullException.ThrowIfNull(repositoryDiSetup);

		FileSourceDiRegistrationOptions.Configure(_services, repositoryDiSetup);
		return _services;
	}

    public static void Configure(
        IServiceCollection services,
        Action<FileDependenciesRegistrationsOptions> fileSourceRegistrationSetup)
    {
        ArgumentNullException.ThrowIfNull(services);
        fileSourceRegistrationSetup.Invoke(new(services));
    }
}