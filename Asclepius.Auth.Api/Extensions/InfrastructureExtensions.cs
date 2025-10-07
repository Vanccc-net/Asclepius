using Asclepius.Auth.Data;
using StackExchange.Redis;
using MassTransit;

namespace Asclepius.Auth.Api.Extensions;

public static class InfrastructureExtensions
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddStackExchangeRedisCache(op =>
        {
            op.Configuration = configuration["Redis:Configuration"] ?? throw new InvalidOperationException();
            op.InstanceName = configuration["Redis:InstanceName"] ?? throw new InvalidOperationException();
        });

        var redisConfiguration = configuration["Redis:Configuration"];
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(redisConfiguration!));
        
        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<ApplicationContext>(o =>
            {
                o.UsePostgres();
                o.UseBusOutbox();

                o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
                
                o.QueryDelay = TimeSpan.FromSeconds(30);
                o.QueryTimeout = TimeSpan.FromSeconds(5); 
                o.QueryMessageLimit = 20;
            });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Host"] ?? throw new InvalidOperationException(), "/", h =>
                {
                    h.Username(configuration["RabbitMq:Username"] ?? throw new InvalidOperationException());
                    h.Password(configuration["RabbitMq:Password"] ?? throw new InvalidOperationException());
                });

                cfg.PrefetchCount = 100;
                cfg.ConcurrentMessageLimit = 10;

                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
    
}