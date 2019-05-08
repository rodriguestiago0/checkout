using Checkout.Entities.Data;
using FluentValidation;

namespace CheckoutAssignment.Validation
{
    public class BasketValidator : AbstractValidator<Basket>
    {
        public BasketValidator()
        {
            RuleForEach(reg => reg.Items.Values).SetValidator(new BasketItemValidator());
        }
    }
}
