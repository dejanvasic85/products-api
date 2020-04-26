using FluentValidation;
using Xero.Products.BusinessLayer;

namespace Xero.Products.Api.Validation
{
    public class ProductOptionValidator : AbstractValidator<ProductOption>
    {
        public ProductOptionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is mandatory");

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is mandatory");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is mandatory");
        }
    }
}
