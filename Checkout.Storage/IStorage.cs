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

        Task<bool> AddOrReplaceItemAsync(int basketId, int itemId, int count);

        Task<bool> RemoveItemFromBasket(int basketId, int itemId);

        Task<IEnumerable<Basket>> GetBasketsAsync();  

        Task<Basket> GetBasketAsync(int basketId);

        Task<bool> BasketExistsAsync(int id);

        Task<bool> AddOrReplaceBasketAsync(Basket basket);

        Task<bool> ChangeQuantityAsync(int basketId, int itemId, int count);

        Task<decimal> CheckoutAsync(int basketId);

        Task<bool> RemoveBasketAsync(int basketId);

        Task<bool> ClearBascketAsync(int basketId);

        Task<bool> ItemExistsAsync(int id);

        Task<bool> AddItemAsync(Item item);

        Task RemoveItemAsync(int id);
    }
}