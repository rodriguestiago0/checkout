using System;
using System.Collections.Generic;

namespace Checkout.Api.Model
{
    public class BasketResponse
    {
        public int Id{get; set;}

        public List<BasketItemResponse> Items {get; set;} = new List<BasketItemResponse>();
    }
}
