using System;
using System.Collections.Generic;

namespace Checkout.Entities
{
    public class Basket
    {
        public ulong Id{get; set;}

        public Dictionary<ulong, BasketItem> Items {get; set;}
    }
}
