using Microsoft.Extensions.DependencyInjection;

namespace Math.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // No infrastructure dependencies in the Math service — no DB, no HTTP clients.
        return services;
    }
}
