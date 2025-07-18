namespace NadinSoft.CRUD.Application.Common.DTOs;

public class ApiResponse<T> where T : class
{
    private ApiResponse(T? data, bool isSuccess, string? error)
    {
        Data = data;
        IsSuccess = isSuccess;
        Error = error;
    }

    public T? Data { get; }
    public bool IsSuccess { get; }
    public string? Error { get; }

    public static ApiResponse<T> Success(T data)
    {
        return new ApiResponse<T>(data, true, null);
    }

    public static ApiResponse<T> Fail(string error)
    {
        return new ApiResponse<T>(null, false, error);
    }
}