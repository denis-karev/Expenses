using Microsoft.Extensions.DependencyInjection;

namespace Expenses.Api.Database;

public static class Extensions
{
    public static IServiceCollection AddMigrationsRunner(this IServiceCollection services)
    {
        return services.AddFluentMigratorCore();
    }
}