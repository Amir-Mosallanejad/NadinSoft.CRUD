namespace NadinSoft.CRUD.AcceptanceTest.Dto;

public class CreateProductRequestDto
{
    public string Name { get; set; } = null!;
    public DateTime ProduceDate { get; set; }
    public string ManufacturePhone { get; set; } = null!;
    public string ManufactureEmail { get; set; } = null!;
    public bool IsAvailable { get; set; }
}