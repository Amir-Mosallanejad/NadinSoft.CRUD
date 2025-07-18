namespace NadinSoft.CRUD.Application.Common.DTOs;

public abstract class PaginatedRequest
{
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 10;
}