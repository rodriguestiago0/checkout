using System;
using System.Collections.Generic;

namespace Checkout.Entities.Data
{
    public class Basket
    {
        public int Id{get; set;}

        public Dictionary<int, BasketItem> Items {get; set;}
    }
}
