using FluentValidation;
using Xero.Products.Api.Resources;
using Xero.Products.BusinessLayer;

namespace Xero.Products.Api.Validation
{
    public class CreateUpdateProductOptionResourceValidator : AbstractValidator<CreateUpdateProductOptionResource>
    {
        public CreateUpdateProductOptionResourceValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is mandatory");
        }
    }
}
