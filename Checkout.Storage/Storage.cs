using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Entities.Data;

namespace Checkout.Storage
{
    public class Storage : IStorage
    {
        private Dictionary<int, Item> _items = new Dictionary<int, Item>
        {
            {1, new Item{
                Id = 0,
                Description = "Item-0",
                Name = "Item-0",
                Price = 10
            }},
            {2, new Item{
                Id = 1,
                Description = "Item-1",
                Name = "Item-1",
                Price = 10
            }},
            {3, new Item{
                Id = 2,
                Description = "Item-2",
                Name = "Item-2",
                Price = 10
            }}
        };

        private Dictionary<int, Basket> _baskets {get; set;}
        
        private int AvailableId = 0;

        public Task<IEnumerable<Item>> GetItemsAsync() 
        {
            return Task.FromResult<IEnumerable<Item>>(_items.Values);
        }

        public Task<Item> GetItemAsync(int id) 
        {
            if(_items.ContainsKey(id))
                return null;
            return Task.FromResult(_items[id]);
        }

        public Task<int> InitBasketAsync()
        {
            _baskets[AvailableId] = new Basket{
                Id = AvailableId
            };
            return Task.FromResult(AvailableId++);
        }

        public Task<bool> AddOrReplaceItemAsync(int basketId, int itemId, int count)
        {
            if(count > 0 && _baskets.ContainsKey(basketId) && _items.ContainsKey(itemId)){
                var item = _items[itemId];
                var items = _baskets[basketId].Items;
                items.Add(item.Id, new BasketItem{
                    Item = item,
                    Count = count
                });
            
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> RemoveItemFromBasket(int basketId, int itemId)
        {
            if(_baskets.ContainsKey(basketId) && _items.ContainsKey(itemId)){
                var item = _items[itemId];
                var items = _baskets[basketId].Items;
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
            if(_baskets.ContainsKey(basketId)){
                _baskets[basketId].Items.Clear();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<IEnumerable<Basket>> GetBasketsAsync()
        {
            return Task.FromResult<IEnumerable<Basket>>(_baskets.Values);
        }

        public Task<Basket> GetBasketAsync(int basketId)
        {
            if(_baskets.ContainsKey(basketId))
                return Task.FromResult(_baskets[basketId]);
            return null;
        }

        public Task<bool> BasketExistsAsync(int id)
        {
            return Task.FromResult(_baskets.ContainsKey(id));
        }

        public Task<bool> AddOrReplaceBasketAsync(Basket basket)
        {
            if(basket == null)
                return Task.FromResult(false);
            if(basket.Items.Values.Any(basketItem => !_items.ContainsKey(basketItem.Item.Id)))
                return Task.FromResult(false);
            if(basket != null)
                _baskets[basket.Id] = basket;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeQuantityAsync(int basketId, int itemId, int count)
        {
            var basket = _baskets[basketId];
            
            if(!basket.Items.ContainsKey(itemId))
                return Task.FromResult(false);
            
            var item = basket.Items[itemId];
            item.Count = count;
            return Task.FromResult(true);
        }

        public Task<decimal> CheckoutAsync(int basketId)
        {
            if(!_baskets.ContainsKey(basketId))
                return Task.FromResult(0m);

            var total = _baskets[basketId].Items.Values.Sum(i => i.Item.Price * i.Count);
            return Task.FromResult(total);
        }

        public Task<bool> RemoveBasketAsync(int basketId)
        {
            if(!_baskets.ContainsKey(basketId))
                return Task.FromResult(false);
            _baskets.Remove(basketId);
            return Task.FromResult(true);
        }

        public  Task<bool> ItemExistsAsync(int id){
            return Task.FromResult(_items.ContainsKey(id));
        }

        public Task<bool> AddItemAsync(Item item){
            if(item == null)
                return Task.FromResult(false);
            _items.Add(item.Id, item);
            return Task.FromResult(true);
        }

        public Task RemoveItemAsync(int id){
            _items.Remove(id);
            return Task.CompletedTask;
        }

        public void Dispose(){

            _items?.Clear();
            _items = null;
            _baskets?.Clear();
            _baskets = null;
        }
    }
}
