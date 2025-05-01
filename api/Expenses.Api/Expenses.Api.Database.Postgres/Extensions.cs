using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Expenses.Api.Database.Postgres;

public static class Extensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services, String connectionString)
    {
        return services
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(IDatabaseContext).Assembly));
    }
}