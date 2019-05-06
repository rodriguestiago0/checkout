using Checkout.Entities.Data;
using FluentValidation;

namespace CheckoutAssignment.Validation
{
    public class BasketValidator : AbstractValidator<Basket>
    {
        public BasketValidator()
        {
            RuleFor(reg => reg.Items.Values).SetCollectionValidator(new BasketItemValidator());
        }
    }
}
