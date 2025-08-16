namespace NadinSoft.CRUD.Application.Common.DTOs;

/// <summary>
/// Represents a standard API response wrapper with success or failure information.
/// </summary>
/// <typeparam name="T">The type of data returned in the response. Must be a reference type.</typeparam>
public class ApiResponse<T>
    where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
    /// </summary>
    /// <param name="data">The data returned in the response. Can be <c>null</c> if the request failed.</param>
    /// <param name="isSuccess"><c>true</c> if the operation was successful; otherwise, <c>false</c>.</param>
    /// <param name="error">The error message if the operation failed; otherwise, <c>null</c>.</param>
    private ApiResponse(T? data, bool isSuccess, string? error)
    {
        Data = data;
        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Gets the data returned by the API.
    /// </summary>
    public T? Data { get; }

    /// <summary>
    /// Gets a value indicating whether the API operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the error message if the API operation failed.
    /// </summary>
    public string? Error { get; }

    /// <summary>
    /// Creates a successful API response containing the specified data.
    /// </summary>
    /// <param name="data">The data to include in the response.</param>
    /// <returns>A new <see cref="ApiResponse{T}"/> instance representing a successful operation.</returns>
    public static ApiResponse<T> Success(T data)
    {
        return new ApiResponse<T>(data, true, null);
    }

    /// <summary>
    /// Creates a failed API response containing the specified error message.
    /// </summary>
    /// <param name="error">The error message describing the failure.</param>
    /// <returns>A new <see cref="ApiResponse{T}"/> instance representing a failed operation.</returns>
    public static ApiResponse<T> Fail(string error)
    {
        return new ApiResponse<T>(null, false, error);
    }
}