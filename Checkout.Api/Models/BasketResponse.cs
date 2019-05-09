using System;
using System.Collections.Generic;

namespace Checkout.Api.Model
{
    public class BasketResponse
    {
        public int Id{get; set;}

        public Dictionary<int, BasketItemResponse> Items {get; set;} = new Dictionary<int, BasketItemResponse>();
    }
}
