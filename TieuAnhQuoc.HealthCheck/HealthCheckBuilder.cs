using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using TieuAnhQuoc.HealthCheck.Models;

namespace TieuAnhQuoc.HealthCheck;

public static class HealthCheckBuilder
{
    public static IApplicationBuilder UseHealthCheck(this IApplicationBuilder app, PathString path, string name,
        string dateTimeFormat = null)
    {
        if (app == null)
            throw new ArgumentNullException(nameof(app));

        var options = new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var healthCheckResponse = new HealthCheckResponse
                {
                    Status = report.Status == HealthStatus.Healthy ? 200 : 500,
                    Msg = report.Status.ToString(),
                    Name = name,
                    Dependencies = report.Entries.Select(x => new Dependency
                    {
                        Important = x.Value.Data.FirstOrDefault(z => z.Key == "Important").Value,
                        Status = x.Value.Status == HealthStatus.Healthy ? 200 : 500,
                        Msg = x.Value.Status.ToString(),
                        Description = x.Value.Description,
                        Name = x.Key,
                        Speed = x.Value.Duration,
                        ServiceType =
                            x.Value.Data.FirstOrDefault(z => z.Key == "Type").Value.ToString() ?? string.Empty,
                        DateTime = DateTime.Now,
                        DateTimeFormat = dateTimeFormat ?? "dd-MM-yyyy hh:mm:ss tt zz"
                    })
                };

                var response = JsonSerializer.Serialize(healthCheckResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy()
                });
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync(response);
            }
        };

        UseHealthChecksCore(app, path, new object[] {Options.Create(options)});
        return app;
    }
    private static void UseHealthChecksCore(IApplicationBuilder app, PathString path, object[] args)
    {
        bool Predicate(HttpContext c)
        {
            return !path.HasValue || (c.Request.Path.StartsWithSegments(path, out var remaining) &&
                                      string.IsNullOrEmpty(remaining));
        }

        app.MapWhen(Predicate, b => b.UseMiddleware<HealthCheckMiddleware>(args));
    }
}