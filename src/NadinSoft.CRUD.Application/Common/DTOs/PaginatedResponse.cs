namespace NadinSoft.CRUD.Application.Common.DTOs;

public class PaginatedResponse<T> where T : class
{
    public List<T> Items { get; set; } = null!;
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int TotalCount { get; set; }
}