using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TieuAnhQuoc.HealthCheck.Models;

public class CheckSqlOption
{
    public string ConnectionString { get; set; }
    public bool Important { get; set; } = false;
    public DependencyType DependencyType { get; set; } = DependencyType.Internal;
    public string HealthQuery { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public HealthStatus? FailureStatus { get; set; } = default;
    public IEnumerable<string> Tags { get; set; } = default;
    public TimeSpan? Timeout { get; set; } = default;
    public Func<IServiceProvider, string> ConnectionStringFactory => _ => ConnectionString;
}
