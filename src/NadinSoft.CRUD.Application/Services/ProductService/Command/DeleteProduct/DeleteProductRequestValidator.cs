using FluentValidation;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;

/// <summary>
/// Validates <see cref="DeleteProductRequest"/> instances to ensure the request contains a valid product ID.
/// </summary>
public class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProductRequestValidator"/> class.
    /// Configures the rule to ensure <see cref="DeleteProductRequest.ProductId"/> is not empty.
    /// </summary>
    public DeleteProductRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");
    }
}