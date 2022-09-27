using System.Text.Json;
using System.Text.RegularExpressions;

namespace TieuAnhQuoc.HealthCheck;

public class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return string.IsNullOrWhiteSpace(name) ? name : Regex.Replace(name, "([a-z])([A-Z])", "$1_$2").ToLower();
    }
}