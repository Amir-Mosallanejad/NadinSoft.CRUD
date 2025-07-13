namespace NadinSoft.CRUD.Application.Common.DTOs;

public class ApiResponse<T> where T : class
{
    public ApiResponse(T data)
    {
        Data = data;
        Success = true;
    }

    public static ApiResponse<T> Fail(IEnumerable<string> errors)
    {
        return new ApiResponse<T>(errors.ToList().First());
    }

    public ApiResponse(string error)
    {
        Success = false;
        Error = error;
    }

    public T? Data { get; set; }
    public bool Success { get; set; }
    public string? Error { get; set; }
}