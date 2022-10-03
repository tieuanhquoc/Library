using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TieuAnhQuoc.HealthCheck.Models;

namespace TieuAnhQuoc.HealthCheck.HealthChecks;

public class UriCheck : IHealthCheck
{
    private readonly Uri _uri;
    private readonly bool _isHealthCheck;
    private readonly string _checkPath;
    private readonly string _name;
    private readonly bool _important;
    private readonly string _dependencyType;


    public UriCheck(Uri uri, bool isHealthCheck, string name, bool important,
        string dependencyType, string checkPath)
    {
        _name = name;
        _important = important;
        _dependencyType = dependencyType;
        _checkPath = checkPath;
        _isHealthCheck = isHealthCheck;
        _uri = uri;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        var data = new Dictionary<string, object>
        {
            {"Name", _name},
            {"Important", _important},
            {"Type", _dependencyType}
        };
        try
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = _uri;

            var response = await httpClient.GetAsync(_isHealthCheck ? _checkPath : "", cancellationToken);
            if (!response.IsSuccessStatusCode && !_isHealthCheck)
                return _important
                    ? HealthCheckResult.Unhealthy(response.ReasonPhrase, null, data)
                    : HealthCheckResult.Degraded(response.ReasonPhrase, null, data);

            if (!_isHealthCheck && response.IsSuccessStatusCode) return HealthCheckResult.Healthy(_name, data);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var healthCheckResponse = JsonSerializer.Deserialize<HealthCheckResponse>(responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy()
                });
            if (healthCheckResponse == null || healthCheckResponse.Status != 200)
                return _important
                    ? HealthCheckResult.Unhealthy(healthCheckResponse?.Description, null, data)
                    : HealthCheckResult.Degraded(healthCheckResponse?.Description, null, data);
            return HealthCheckResult.Healthy(healthCheckResponse.Description, data);
        }
        catch (Exception exception)
        {
            return _important
                ? HealthCheckResult.Unhealthy(exception.Message, exception, data)
                : HealthCheckResult.Degraded(exception.Message, exception, data);
        }
    }
}