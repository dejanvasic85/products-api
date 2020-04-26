using FluentValidation;
using Xero.Products.BusinessLayer;

namespace Xero.Products.Api.Validation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        private static readonly decimal MinimumPrice = 0;

        public ProductValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is mandatory");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is mandatory");

            RuleFor(x => x.Price)
                .GreaterThan(MinimumPrice)
                .WithMessage("Price must be greater than 0");

            RuleFor(x => x.DeliveryPrice)
                .GreaterThanOrEqualTo(MinimumPrice)
                .WithMessage("Delivery Price must be greater than or equal to 0");
        }
    }
}
