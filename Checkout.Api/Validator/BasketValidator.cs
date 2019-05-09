using Checkout.Api.Model;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.Api.Validation
{
    public class BasketValidator : AbstractValidator<BasketResponse>
    {
        public BasketValidator()
        {
            RuleFor(reg => reg.Items).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Must(IsUnique).WithMessage("The items must be unique");
            RuleForEach(reg => reg.Items).SetValidator(new BasketItemValidator());
        }

        private bool IsUnique(List<BasketItemResponse> items)
        {
            var hashSet = new HashSet<int>();
            foreach(var item in items){
                hashSet.Add(item.Item.Id);
            }
            return items.Count == hashSet.Count;
        }
    }
}
