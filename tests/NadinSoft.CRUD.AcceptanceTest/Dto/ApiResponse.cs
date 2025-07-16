namespace NadinSoft.CRUD.AcceptanceTest.Dto;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
}