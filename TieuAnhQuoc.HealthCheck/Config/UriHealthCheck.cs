using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TieuAnhQuoc.HealthCheck.HealthChecks;
using TieuAnhQuoc.HealthCheck.Models;

namespace TieuAnhQuoc.HealthCheck.Config;

public static class UriHealthCheck
{
    public static IHealthChecksBuilder AddUriCheck(this IHealthChecksBuilder builder, CheckUriOption checkUriOption)
    {
        return builder.Add(new HealthCheckRegistration(
            checkUriOption.Name,
            sp => new UriCheck(
                checkUriOption.Uri,
                checkUriOption.IsHealthCheck,
                checkUriOption.Name,
                checkUriOption.Important,
                checkUriOption.DependencyType.ToString(),
                checkUriOption.HealthUri
            ),
            checkUriOption.FailureStatus,
            checkUriOption.Tags,
            checkUriOption.Timeout));
    }
}