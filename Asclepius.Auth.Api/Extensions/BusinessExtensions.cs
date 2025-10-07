using Asclepius.Auth.Business;
using Asclepius.Auth.Domain.Interfaces;

namespace Asclepius.Auth.Api.Extensions;

public static class BusinessExtensions
{

    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddSingleton<JwtGenerator>();
        
        return services;
    }
    
}