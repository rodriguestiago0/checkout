using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.Entities.Data;

namespace Checkout.Storage
{
    public interface IStorage : IDisposable
    {
        Task<IEnumerable<Item>> GetItems();


        Task<Item> GetItem(int id);

        Task<int> InitBasket();

        Task<bool> AddItem(int basketId, int itemId, int count);

        Task<IEnumerable<Basket>> GetBaskets();  

        Task<Basket> GetBasket(int basketId);

        Task<bool> BasketExists(int id);

        Task<bool> AddOrReplaceBasket(Basket basket);

        Task<bool> ChangeQuantity(int basketId, int itemId, int count);

        Task Checkout(int basketId);

        Task<bool> RemoveBasket(int basketId);
    }
}