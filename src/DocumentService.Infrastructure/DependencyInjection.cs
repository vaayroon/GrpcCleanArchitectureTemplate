using DocumentService.Application.Abstractions.Persistence;
using DocumentService.Application.Abstractions.Storage;
using DocumentService.Infrastructure.Options;
using DocumentService.Infrastructure.Persistence;
using DocumentService.Infrastructure.Persistence.Repositories;
using DocumentService.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StorageOptions>(options => configuration.GetSection("Storage"));

        string? connectionString = configuration.GetConnectionString("DocumentDatabase");
        services.AddDbContext<DocumentDbContext>(
            options => options.UseNpgsql(
                connectionString,
                npgsql => npgsql.MigrationsAssembly(typeof(DocumentDbContext).Assembly.FullName)));

        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFileStorage, FileSystemFileStorage>();

        return services;
    }
}
