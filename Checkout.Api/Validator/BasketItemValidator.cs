using Checkout.Api.Model;
using FluentValidation;

namespace Checkout.Api.Validation
{
    public class BasketItemValidator : AbstractValidator<BasketItemResponse>
    {
        public BasketItemValidator()
        {
            RuleFor(basketItem => basketItem.Count).Must(count => count > 0).WithMessage("Count must be greater than 0");;
            RuleFor(basketItem => basketItem.Item).NotNull().WithMessage("Item cannot be null");
        }
    }
}
