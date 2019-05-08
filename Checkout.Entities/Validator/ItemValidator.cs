using Checkout.Entities.Data;
using FluentValidation;

namespace Checkout.Entities.Validator
{
    public class ItemValidator : AbstractValidator<Item>
    {
        public ItemValidator()
        {
            RuleFor(reg => reg.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(reg => reg.Description).NotEmpty().WithMessage("Desciption cannot be empty");
            RuleFor(reg => reg.Price).NotEmpty().ExclusiveBetween(0, decimal.MaxValue).WithMessage("Price must be greater than 0");
        }
    }
}