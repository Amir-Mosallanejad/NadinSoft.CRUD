using FluentValidation.TestHelper;
using NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;

namespace NadinSoft.CRUD.UnitTest.ProductService.Validators;

/// <summary>
/// Contains unit tests for <see cref="DeleteProductRequestValidator"/>.
/// </summary>
public class DeleteProductRequestValidatorTests
{
    /// <summary>
    /// Validator instance being tested.
    /// </summary>
    private readonly DeleteProductRequestValidator _validator = new();

    /// <summary>
    /// Tests that validation fails when the ProductId is empty (Guid.Empty).
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenProductIdIsEmpty()
    {
        DeleteProductRequest request = new DeleteProductRequest(Guid.Empty);

        TestValidationResult<DeleteProductRequest>? result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.ProductId)
            .WithErrorMessage("ProductId is required.");
    }

    /// <summary>
    /// Tests that no validation errors occur when the ProductId is valid.
    /// </summary>
    [Fact]
    public void ShouldNotHaveErrorWhenProductIdIsValid()
    {
        DeleteProductRequest request = new DeleteProductRequest(Guid.NewGuid());

        TestValidationResult<DeleteProductRequest>? result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.ProductId);
    }
}