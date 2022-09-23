namespace HandleResponse.Models;

public class ApiResponse<T> : ApiResponse
{
    public T Data { get; set; }

    public static ApiResponse<T> Success(T result)
    {
        return Create(result, ApiStatusCode.SUCCESS, ApiStatusCode.SUCCESS.ToString());
    }

    private static ApiResponse<T> Create(T data, ApiStatusCode statusCode, string message)
    {
        return new ApiResponse<T>
        {
            Data = data,
            StatusCode = (int) statusCode,
            Message = message,
        };
    }
}

public class ApiResponses<T> : ApiResponse
{
    public int? TotalCount { get; set; } = 0;
    public int? PageSize { get; set; } = 0;
    public int? Offset { get; set; } = 0;
    public int? TotalPages { get; set; } = 0;
    public IEnumerable<T> Data { get; set; }

    public static ApiResponses<T> Success(IEnumerable<T> data, int? totalCount = null, int? pageSize = null,
        int? offset = null,
        int? totalPages = null)
    {
        return Create(
            data,
            ApiStatusCode.SUCCESS,
            ApiStatusCode.SUCCESS.ToString(),
            totalCount,
            pageSize,
            offset,
            totalPages
        );
    }

    private static ApiResponses<T> Create(IEnumerable<T> data, ApiStatusCode statusCode, string message,
        int? totalCount,
        int? pageSize,
        int? offset,
        int? totalPages)
    {
        return new ApiResponses<T>
        {
            Data = data,
            StatusCode = (int) statusCode,
            Message = message,
            TotalCount = totalCount,
            PageSize = pageSize,
            Offset = offset,
            TotalPages = totalPages
        };
    }
}

public class ApiResponse
{
    public int? StatusCode { get; set; }

    public string Message { get; set; }

    public static ApiResponse Success()
    {
        return Create(ApiStatusCode.SUCCESS, "Success");
    }

    public static ApiResponse Success(string message)
    {
        return Create(ApiStatusCode.SUCCESS, message);
    }

    public static ApiResponse Failed()
    {
        return Create(ApiStatusCode.CLIENT_ERROR, "Failed");
    }

    private static ApiResponse Create(ApiStatusCode statusCode, string message)
    {
        return new ApiResponse
        {
            StatusCode = (int) statusCode,
            Message = message,
        };
    }
}

public enum ApiStatusCode
{
    SUCCESS = 200,
    CREATED = 201,
    CLIENT_ERROR = 400,
    UNAUTHORIZED = 401,
    SERVER_ERROR = 500,
    NOT_FOUND = 404
}