using FluentValidation;
using Xero.Products.Api.Resources;

namespace Xero.Products.Api.Validation
{
    public class CreateUpdateProductResourceValidator : AbstractValidator<CreateUpdateProductResource>
    {
        private static readonly decimal MinimumPrice = 0;

        public CreateUpdateProductResourceValidator()
        {
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
