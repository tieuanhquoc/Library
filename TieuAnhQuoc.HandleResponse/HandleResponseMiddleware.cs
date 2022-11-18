using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;
using TieuAnhQuoc.HandleResponse.Models;

namespace TieuAnhQuoc.HandleResponse;

public class HandleResponseMiddleware
{
    private readonly RequestDelegate _next;

    public HandleResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var startTime = Stopwatch.GetTimestamp();
        try
        {
            await _next(context);
            var elapsedMilliseconds = GetElapsedMilliseconds(startTime, Stopwatch.GetTimestamp());
            Log.Information(
                "Http Response Information | Method: {Method} | Path: {Path} | Status Code: {StatusCode} | QueryString: {QueryString}",
                context.Request.Method.ToUpper(),
                context.Request.Path,
                $"{context.Response.StatusCode} in {elapsedMilliseconds} ms",
                context.Request.QueryString
            );
        }
        catch (ApiException apiException)
        {
            var elapsedMilliseconds = GetElapsedMilliseconds(startTime, Stopwatch.GetTimestamp());
            await HandleExceptionAsync(context,
                new ApiException(apiException.Message, apiException.StatusCode, apiException.Result), apiException,
                elapsedMilliseconds);
        }
        catch (Exception exception)
        {
            var elapsedMilliseconds = GetElapsedMilliseconds(startTime, Stopwatch.GetTimestamp());
            await HandleExceptionAsync(context, new ApiException(), exception, elapsedMilliseconds);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, ApiException apiException, Exception exception,
        double elapsedMilliseconds)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) apiException.StatusCode;
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(new
            {
                message = apiException.Message,
                statusCode = apiException.StatusCode,
                result = apiException.Result
            })
        );


        IEnumerable<LogEventProperty> properties = new LogEventProperty[]
        {
            new("RequestMethod", new ScalarValue(context.Request.Method.ToUpper())),
            new("RequestPath", new ScalarValue(context.Request.Path)),
            new("StatusCode", new ScalarValue(context.Response.StatusCode)),
            new("Elapsed", new ScalarValue(elapsedMilliseconds)),
            new("Message", new ScalarValue(apiException.Message))
        };
        var messageTemplate = new MessageTemplateParser().Parse(
            "Http Response - Method: {RequestMethod} | Path: {RequestPath} | Responded {StatusCode} in {Elapsed:0.0000} ms | Message: {Message}");
        var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Error, exception, messageTemplate,
            properties);
        Log.Write(logEvent);
    }

    private static double GetElapsedMilliseconds(long start, long stop) =>
        (stop - start) * 1000L / (double) Stopwatch.Frequency;
}