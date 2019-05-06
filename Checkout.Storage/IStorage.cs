using System;
using System.Collections.Generic;
using Checkout.Entities;

namespace Checkout.Storage
{
    public interface IStorage
    {
        IEnumerable<Item> GetItems();


        Item GetItem(int id);

        int InitBasket();

        bool AddItem(int basketId, Item item, int count);

        IEnumerable<Basket> GetBaskets();  

        Basket GetBasket(int basketId);

        bool BasketExists(int id);

        bool AddBasket(Basket basket);

        bool ChangeQuantity(int basketId, int itemId, int count);

        void Checkout(int basketId);

        bool RemoveBasket(int basketId);
    }
}