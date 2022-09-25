using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TieuAnhQuoc.HandleResponse.Models;

namespace TieuAnhQuoc.HandleResponse;

public class HandleResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public HandleResponseMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<HandleResponseMiddleware>();
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
            _logger.LogInformation(
                "Http Response Information | Method: {Method} | Path: {Path} | Status Code: {StatusCode} | QueryString: {QueryString}",
                context.Request.Method.ToUpper(),
                context.Request.Path,
                context.Response.StatusCode,
                context.Request.QueryString
            );
        }
        catch (ApiException apiException)
        {
            await HandleExceptionAsync(context, new ApiException(apiException.Message, apiException.StatusCode),
                apiException.Message);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, new ApiException(), exception.Message);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, ApiException apiException, string message)
    {
        context.Response.ContentType = "application/json";
        int.TryParse(apiException.StatusCode.ToString(), out var statusCode);
        context.Response.StatusCode = statusCode;

        var response = JsonSerializer.Serialize(new
        {
            apiException.Message,
            apiException.StatusMessage,
            apiException.Result
        });

        await context.Response.WriteAsync(response);

        _logger.LogError(
            "Http Response Error | Method: {Method} | Path: {Path} | Status Code: {StatusCode} | QueryString: {QueryString} Message: {Message}",
            context.Request.Method.ToUpper(),
            context.Request.Path,
            context.Response.StatusCode,
            $"{context.Request.QueryString}{Environment.NewLine}",
            message
        );
    }
}