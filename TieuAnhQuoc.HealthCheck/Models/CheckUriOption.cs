using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TieuAnhQuoc.HealthCheck.Models;

public class CheckUriOption
{
    public Uri Uri { get; set; }
    public bool Important { get; set; } = false;
    public DependencyType DependencyType { get; set; } = DependencyType.Internal;
    public string Name { get; set; } = string.Empty;
    public HealthStatus? FailureStatus { get; set; } = default;
    public IEnumerable<string> Tags { get; set; } = default;
    public TimeSpan? Timeout { get; set; } = default;
    public bool IsHealthCheck { get; set; } = false;
    public string HealthUri { get; set; } = "health";
}