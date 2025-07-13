using FluentValidation;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;

public class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.CreatedByUserId)
            .NotEmpty().WithMessage("CreatedByUserId is required.");
    }
}