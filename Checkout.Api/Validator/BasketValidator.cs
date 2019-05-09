using Checkout.Api.Model;
using FluentValidation;

namespace Checkout.Api.Validation
{
    public class BasketValidator : AbstractValidator<BasketResponse>
    {
        public BasketValidator()
        {
            RuleForEach(reg => reg.Items.Values).SetValidator(new BasketItemValidator());
        }
    }
}
