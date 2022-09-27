using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TieuAnhQuoc.HealthCheck.HealthChecks;
using TieuAnhQuoc.HealthCheck.Models;

namespace TieuAnhQuoc.HealthCheck.Config;

public static class SqlServerHealthCheck
{
    private const string HealthQuery = "SELECT 1;";
    private const string Name = "sqlserver";

    public static IHealthChecksBuilder AddSqlServer(this IHealthChecksBuilder builder, CheckSqlOption checkSqlOption)
    {
        if (checkSqlOption.ConnectionStringFactory == null)
        {
            throw new ArgumentNullException(checkSqlOption.ConnectionString, "False");
        }

        if (string.IsNullOrEmpty(checkSqlOption.HealthQuery))
            checkSqlOption.HealthQuery = HealthQuery;

        if (string.IsNullOrEmpty(checkSqlOption.Name))
            checkSqlOption.Name = Name;

        return builder.Add(new HealthCheckRegistration(
            checkSqlOption.Name,
            sp => new SqlServerCheck(
                checkSqlOption.ConnectionStringFactory(sp),
                checkSqlOption.HealthQuery,
                checkSqlOption.Name,
                checkSqlOption.Important,
                checkSqlOption.DependencyType.ToString()),
            checkSqlOption.FailureStatus,
            checkSqlOption.Tags,
            checkSqlOption.Timeout));
    }
}