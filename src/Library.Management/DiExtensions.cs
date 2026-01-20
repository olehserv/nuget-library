using Library.File.Core;
using Library.Management.Book;

using Microsoft.Extensions.DependencyInjection;

namespace Library.Management;

public static class DiExtensions
{
    public static IServiceCollection AddBookService(
        this IServiceCollection services,
        Action<FileDependenciesRegistrationsOptions> fileDependenciesSetup)
    {
        ArgumentNullException.ThrowIfNull(services);

        FileDependenciesRegistrationsOptions.Configure(services, fileDependenciesSetup);
        
        services.AddScoped<IBookService, BookService>();
        return services;
    }
}