using System;

namespace Checkout.Api.Model
{
    public class BasketItemResponse
    {
        public ItemResponse Item {get; set;}

        public int Count {get; set;}
    }
}
