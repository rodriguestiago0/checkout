using System;

namespace Checkout.Entities
{
    public class Item
    {
        public ulong Id{get; set;}

        public string Name {get; set;}

        public string Description {get; set;}

        public float Price {get; set;}
    }
}
