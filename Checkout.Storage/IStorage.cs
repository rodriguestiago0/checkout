using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.Entities.Data;

namespace Checkout.Storage
{
    public interface IStorage : IDisposable
    {
        Task<IEnumerable<Item>> GetItemsAsync();


        Task<Item> GetItemAsync(int id);

        Task<int> InitBasketAsync();

        Task<bool> AddItemAsync(int basketId, int itemId, int count);

        Task<IEnumerable<Basket>> GetBasketsAsync();  

        Task<Basket> GetBasketAsync(int basketId);

        Task<bool> BasketExistsAsync(int id);

        Task<bool> AddOrReplaceBasketAsync(Basket basket);

        Task<bool> ChangeQuantityAsync(int basketId, int itemId, int count);

        Task CheckoutAsync(int basketId);

        Task<bool> RemoveBasketAsync(int basketId);
    }
}