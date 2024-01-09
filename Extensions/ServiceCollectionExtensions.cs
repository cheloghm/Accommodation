using DestinationDiscoveryService.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DestinationDiscoveryService.Extensions
{

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<AccommodationService>();
        // Add other services here as needed

        return services;
    }
}

}