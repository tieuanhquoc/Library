using System.Text.Json.Serialization;

namespace TieuAnhQuoc.HealthCheck.Models;

public class HealthCheckResponse
{
    public string Name { get; set; }
    public int Status { get; set; }
    public string Msg { get; set; }
    public string Description => GetDescription();
    public IEnumerable<Dependency> Dependencies { get; set; }

    private string GetDescription()
    {
        var descriptions = Dependencies.Where(x => x.Status != 200).Select(z => $"{z.Name} - {z.Description}").Distinct().ToList();
        return string.Join(", ", descriptions);
    }
}

public class Dependency
{
    public string Name { get; set; }
    public int Status { get; set; }
    public string Msg { get; set; }
    public string Description { get; set; }
    public object Important { get; set; }
    public string ServiceType { get; set; }
    [JsonIgnore] public DateTime DateTime { get; set; }
    public string DateTimeFormat { get; set; }
    public string TimeCheck => DateTime.ToString(DateTimeFormat);
    [JsonIgnore] public TimeSpan Speed { get; set; }
    public string CountTimeCheck => $"{Speed.TotalSeconds} seconds";
}

public enum DependencyType
{
    Internal,
    External
}