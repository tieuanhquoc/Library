# How do I get started?
#### Install
    Install-Package QuocTa.HandleResponse

#### Add setting
    app.UseMiddleware<HandleResponseMiddleware>();

#### Use ApiResponse
    return ApiResponse.Success("Message");
    return ApiResponse<ResponseObject>.Success(responseObject);

#### Use ApiResponses
    return return ApiResponses<ResponseObject>.Success(responseObjects, totalRow, pageSize, offset, totalPage);
