using NadinSoft.CRUD.AcceptanceTest.Dto;

namespace NadinSoft.CRUD.AcceptanceTest.Context;

/// <summary>
/// Holds shared state for acceptance tests.
/// This allows different steps in a test scenario
/// to access common data such as authentication,
/// created entities, and API responses.
/// </summary>
public class TestContext
{
    /// <summary>
    /// Gets or sets the JWT token used for authenticated API requests.
    /// </summary>
    /// <value>
    /// The JWT token string, or <c>null</c> if not set.
    /// </value>
    public string? JwtToken { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of a product created during a test scenario.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the product ID.
    /// </value>
    public Guid CreatedProductId { get; set; }

    /// <summary>
    /// Gets or sets the last HTTP response returned by the API.
    /// </summary>
    /// <value>
    /// An <see cref="HttpResponseMessage"/> instance containing the last response,
    /// or <c>null</c> if no response has been received.
    /// </value>
    public HttpResponseMessage? LastResponse { get; set; }

    /// <summary>
    /// Gets or sets the raw body of the last HTTP response.
    /// </summary>
    /// <value>
    /// A string containing the response body, or <c>null</c> if not set.
    /// </value>
    public string? LastResponseBody { get; set; }

    /// <summary>
    /// Gets or sets the deserialized API response from a create operation.
    /// </summary>
    /// <value>
    /// An <see cref="ApiResponse{T}"/> with the result of the create request,
    /// or <c>null</c> if not set.
    /// </value>
    public ApiResponse<object>? CreateResponse { get; set; }

    /// <summary>
    /// Gets or sets the deserialized API response from an update operation.
    /// </summary>
    /// <value>
    /// An <see cref="ApiResponse{T}"/> with the result of the update request,
    /// or <c>null</c> if not set.
    /// </value>
    public ApiResponse<object>? UpdateResponse { get; set; }

    /// <summary>
    /// Gets or sets the deserialized API response from a delete operation.
    /// </summary>
    /// <value>
    /// An <see cref="ApiResponse{T}"/> with the result of the delete request,
    /// or <c>null</c> if not set.
    /// </value>
    public ApiResponse<object>? DeleteResponse { get; set; }

    /// <summary>
    /// Gets or sets the deserialized API response from a paginated get-all operation.
    /// </summary>
    /// <value>
    /// An <see cref="ApiResponse{T}"/> containing a <see cref="PaginatedResponse{T}"/> of
    /// <see cref="ProductResponseDto"/> items, or <c>null</c> if not set.
    /// </value>
    public ApiResponse<PaginatedResponse<ProductResponseDto>>? GetAllResponse { get; set; }
}