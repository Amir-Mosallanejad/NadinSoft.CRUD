using NadinSoft.CRUD.AcceptanceTest.Dto;

namespace NadinSoft.CRUD.AcceptanceTest.Context;

public class TestContext
{
    public string? JwtToken { get; set; }
    public Guid CreatedProductId { get; set; }
    public HttpResponseMessage? LastResponse { get; set; }
    public string? LastResponseBody { get; set; }
    public ApiResponse<object>? CreateResponse { get; set; }
    public ApiResponse<object>? UpdateResponse { get; set; }
    public ApiResponse<object>? DeleteResponse { get; set; }
    public ApiResponse<PaginatedResponse<ProductResponseDto>>? GetAllResponse { get; set; }
}