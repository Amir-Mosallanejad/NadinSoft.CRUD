namespace NadinSoft.CRUD.AcceptanceTest.Dto;

public class UpdateProductRequestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime ProduceDate { get; set; }
    public string ManufacturePhone { get; set; } = null!;
    public string ManufactureEmail { get; set; } = null!;
    public bool IsAvailable { get; set; }
}