using Math.Application.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Math.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IUnitConversionQuery, UnitConversionQueryService>();
        return services;
    }
}
