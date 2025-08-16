namespace NadinSoft.CRUD.AcceptanceTest.Dto;

/// <summary>
/// Represents a standard API response wrapper with success or failure information.
/// </summary>
/// <typeparam name="T">The type of data returned in the response.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Gets or sets a value indicating whether the request was successful.
    /// </summary>
    /// <value>
    /// <c>true</c> if the request succeeded; otherwise, <c>false</c>.
    /// </value>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the data returned by the API if the request was successful.
    /// </summary>
    /// <value>
    /// The response payload of type <typeparamref name="T"/> when <see cref="IsSuccess"/> is <c>true</c>;
    /// otherwise, <c>null</c>.
    /// </value>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the error message if the request failed.
    /// </summary>
    /// <value>
    /// A descriptive error message when <see cref="IsSuccess"/> is <c>false</c>; otherwise, <c>null</c>.
    /// </value>
    public string? Error { get; set; }
}