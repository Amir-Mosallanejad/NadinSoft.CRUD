using FluentValidation.TestHelper;
using NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;

namespace NadinSoft.CRUD.UnitTest.ProductService.Validators;

public class DeleteProductRequestValidatorTests
{
    private readonly DeleteProductRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_ProductId_Is_Empty()
    {
        DeleteProductRequest request = new DeleteProductRequest(Guid.Empty);

        TestValidationResult<DeleteProductRequest>? result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.ProductId)
            .WithErrorMessage("ProductId is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_ProductId_Is_Valid()
    {
        DeleteProductRequest request = new DeleteProductRequest(Guid.NewGuid());

        TestValidationResult<DeleteProductRequest>? result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.ProductId);
    }
}