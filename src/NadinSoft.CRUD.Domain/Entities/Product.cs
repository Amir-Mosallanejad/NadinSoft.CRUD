namespace NadinSoft.CRUD.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = null!;
    public DateTime ProduceDate { get; set; }
    public string ManufacturePhone { get; set; } = null!;
    public string ManufactureEmail { get; set; } = null!;
    public bool IsAvailable { get; set; }

    public string CreatedByUserId { get; set; } = null!;
    public ApplicationUser CreatedByUser { get; set; } = null!;
}