using Library.File.Core.Format;
using Library.File.Core.Source;

using Microsoft.Extensions.DependencyInjection;

namespace Library.File.Core;

public static class DiExtensions
{
	public static IServiceCollection AddFileFormatProcessor(this IServiceCollection services, 
		Action<FileFormatProcessorDiRegistrationOptions> fileProcessorDiSetup)
	{
		ArgumentNullException.ThrowIfNull(fileProcessorDiSetup);

		FileFormatProcessorDiRegistrationOptions.Configure(services, fileProcessorDiSetup);
		return services;
	}

	public static IServiceCollection AddFileSource(this IServiceCollection services,
		Action<FileSourceDiRegistrationOptions> repositoryDiSetup)
	{
		ArgumentNullException.ThrowIfNull(repositoryDiSetup);

		FileSourceDiRegistrationOptions.Configure(services, repositoryDiSetup);
		return services;
	}
}