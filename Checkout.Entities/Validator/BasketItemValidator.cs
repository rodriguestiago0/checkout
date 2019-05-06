using Checkout.Entities.Data;
using FluentValidation;

namespace CheckoutAssignment.Validation
{
    public class BasketItemValidator : AbstractValidator<BasketItem>
    {
        public BasketItemValidator()
        {
            RuleFor(basketItem => basketItem.Count).Must(count => count > 0).WithMessage("Count must be greater than 0");;
            RuleFor(basketItem => basketItem.Item).NotNull().WithMessage("Item cannot be null");
        }
    }
}
