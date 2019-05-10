using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Entities.Data;

namespace Checkout.Storage
{
    public class Storage : IStorage
    {
        private Dictionary<int, Item> _items = new Dictionary<int, Item>();

        private Dictionary<int, Basket> _baskets {get; set;} = new Dictionary<int, Basket>();
        
        private int AvailableId = 0;

        public Task<IEnumerable<Item>> GetItemsAsync() 
        {
            return Task.FromResult<IEnumerable<Item>>(_items.Values);
        }

        public Task<Item> GetItemAsync(int id) 
        {
            if(!_items.ContainsKey(id))
                return Task.FromResult(default(Item));
            return Task.FromResult(_items[id]);
        }

        public Task<int> InitBasketAsync()
        {
            _baskets[AvailableId] = new Basket{
                Id = AvailableId
            };
            return Task.FromResult(AvailableId++);
        }

        public Task<bool> AddOrReplaceItemOnBasketAsync(int basketId, int itemId, int count)
        {
            Basket basket;
            Item item;
            if(count > 0 && _baskets.TryGetValue(basketId, out basket) && _items.TryGetValue(itemId, out item)){
                var items = basket.Items;
                items[item.Id] =  new BasketItem{
                    Item = item,
                    Count = count
                };
            
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> RemoveItemFromBasket(int basketId, int itemId)
        {
            Basket basket;
            Item item;
            if(_baskets.TryGetValue(basketId, out basket) && _items.TryGetValue(itemId, out item)){
                var items = basket.Items;
                if(items.ContainsKey(item.Id))
                    items.Remove(itemId);
                else
                    return Task.FromResult(false);
            
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> ClearBascketAsync(int basketId)
        {
            Basket basket;
            if(!_baskets.TryGetValue(basketId, out basket))
                return Task.FromResult(false);

            basket.Items.Clear();
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Basket>> GetBasketsAsync()
        {
            return Task.FromResult<IEnumerable<Basket>>(_baskets.Values);
        }

        public Task<Basket> GetBasketAsync(int basketId)
        {
            Basket basket;
            if(_baskets.TryGetValue(basketId, out basket))
                return Task.FromResult(basket);
            return Task.FromResult(default(Basket));
        }

        public Task<bool> BasketExistsAsync(int id)
        {
            return Task.FromResult(_baskets.ContainsKey(id));
        }

        public Task<bool> AddOrReplaceBasketAsync(Basket basket)
        {
            if(basket == null)
                return Task.FromResult(false);
            foreach(var basketItem in basket.Items.Values)
            {
                Item item;
                if(!_items.TryGetValue(basketItem.Item.Id, out item))
                    return Task.FromResult(false);
                if(item.Price != basketItem.Item.Price || item.Description != basketItem.Item.Description || item.Name !=basketItem.Item.Name)
                    return Task.FromResult(false);
            }
            if(basket.Items.Values.Any(basketItem => !_items.ContainsKey(basketItem.Item.Id)))
                return Task.FromResult(false);
            if(basket != null)
                _baskets[basket.Id] = basket;
            return Task.FromResult(true);
        }

        private Task<decimal> GetPrice(Basket basket){

            return Task.FromResult(basket.Items.Values.Sum(i => i.Item.Price * i.Count));
        }
        public Task<decimal> CheckoutAsync(int basketId)
        {   
            Basket basket;
            if(!_baskets.TryGetValue(basketId, out basket))
                return Task.FromResult(0m);

            var total = GetPrice(basket);
            basket.Items.Clear();
            return total;
        }


        public Task<decimal> GetTotalPriceAsync(int basketId){
            Basket basket;
            if(!_baskets.TryGetValue(basketId, out basket))
                return Task.FromResult(0m);
            return GetPrice(basket);
        }

        public Task<bool> RemoveBasketAsync(int basketId)
        {
            return Task.FromResult(_baskets.Remove(basketId));
        }

        public Task<bool> AddOrUpdateItemAsync(Item item){
            if(item == null)
                return Task.FromResult(false);
            _items[item.Id] = item;
            return Task.FromResult(true);
        }

        public Task<bool> RemoveItemAsync(int id){
            return Task.FromResult(_items.Remove(id));
        }

        public void Dispose(){

            _items?.Clear();
            _items = null;
            _baskets?.Clear();
            _baskets = null;
        }
    }
}
