using Checkout.Api.Model;
using FluentValidation;

namespace Checkout.Api.Validation
{
    public class ItemValidator : AbstractValidator<ItemResponse>
    {
        public ItemValidator()
        {
            RuleFor(reg => reg.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(reg => reg.Description).NotEmpty().WithMessage("Desciption cannot be empty");
            RuleFor(reg => reg.Price).NotEmpty().ExclusiveBetween(0, decimal.MaxValue).WithMessage("Price must be greater than 0");
        }
    }
}