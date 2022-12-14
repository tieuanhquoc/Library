using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TieuAnhQuoc.HandleResponse.Models;

[Serializable]
public class ApiException : Exception
{
    protected ApiException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(
        serializationInfo, streamingContext)
    {
    }

    public ApiException(string message, ErrorCode statusCode) : base(message)
    {
        StatusCode = statusCode;
        StatusMessage = statusCode.ToString();
        Message = string.IsNullOrEmpty(message) ? StatusCode.ToString() : message;
    }

    public ApiException(string message, ErrorCode statusCode, object result) : base(message)
    {
        StatusCode = statusCode;
        StatusMessage = statusCode.ToString();
        Message = string.IsNullOrEmpty(message) ? StatusCode.ToString() : message;
        Result = result;
    }

    public ApiException(ErrorCode statusCode) : base(statusCode.ToString())
    {
        StatusCode = statusCode;
        StatusMessage = statusCode.ToString();
        Message = "Please try again later";
    }

    public ApiException(string message) : base(message)
    {
        StatusCode = ErrorCode.SERVER_ERROR;
        StatusMessage = StatusCode.ToString();
        Message = string.IsNullOrEmpty(message) ? StatusCode.ToString() : message;
    }

    public ApiException() : base("Please try again later")
    {
    }

    public ErrorCode StatusCode { get; set; } = ErrorCode.SERVER_ERROR;
    public string StatusMessage { get; } = "Please try again later";
    public override string Message { get; } = "Please try again later";
    public object Result { get; set; }
    public string ShortContent { get; set; }
}

public enum ErrorCode
{
    [Display(Name = "400")] OPERATION_NOT_ALLOWED = 400,
    [Display(Name = "403")] FORBIDDEN = 403,
    [Display(Name = "401")] UNAUTHORIZED = 401,
    [Display(Name = "404")] NOT_FOUND = 404,
    [Display(Name = "409")] ALREADY_EXISTS = 409,
    [Display(Name = "410")] GONE = 410,
    [Display(Name = "500")] SERVER_ERROR = 500
}