using System;

namespace Checkout.Entities.Data
{
    public class BasketItem
    {
        public Item Item {get; set;}

        public int Count {get; set;}
    }
}
