using System;

namespace Checkout.Api.Model
{
    public class ItemResponse
    {
        public int Id{get; set;}

        public string Name {get; set;}

        public string Description {get; set;}

        public decimal Price {get; set;}
    }
}
