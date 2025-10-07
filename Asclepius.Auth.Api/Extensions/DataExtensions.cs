using Asclepius.Auth.Data;
using Asclepius.Auth.Data.Repo;
using Asclepius.Auth.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Asclepius.Auth.Api.Extensions;

public static class DataExtensions
{

    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres") ?? throw new InvalidOperationException();
        services.AddDbContextPool<ApplicationContext>(op =>
        {
            op.UseNpgsql(connectionString, b => b.MigrationsAssembly("Asclepius.Auth.Api"));
            // op.EnableSensitiveDataLogging();
            // op.LogTo(Console.WriteLine, LogLevel.Information);
        });
        
        services.AddScoped<IUser, UserRepo>();
        services.AddScoped<IRole, RoleRepo>();
        services.AddScoped<IRefreshToken, RefreshTokenRepo>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
    
}