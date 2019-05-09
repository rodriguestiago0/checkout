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

        public Task<bool> AddOrReplaceItemAsync(int basketId, int itemId, int count)
        {
            if(count > 0 && _baskets.ContainsKey(basketId) && _items.ContainsKey(itemId)){
                var item = _items[itemId];
                var items = _baskets[basketId].Items;
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
            if(basket.Items.Values.Any(basketItem => !_items.ContainsKey(basketItem.Item.Id)))
                return Task.FromResult(false);
            if(basket != null)
                _baskets[basket.Id] = basket;
            return Task.FromResult(true);
        }

        public Task<decimal> CheckoutAsync(int basketId)
        {
            if(!_baskets.ContainsKey(basketId))
                return Task.FromResult(0m);

            var total = _baskets[basketId].Items.Values.Sum(i => i.Item.Price * i.Count);
            ClearBascketAsync(basketId);
            return Task.FromResult(total);
        }

        public Task<bool> RemoveBasketAsync(int basketId)
        {
            if(!_baskets.ContainsKey(basketId))
                return Task.FromResult(false);
            _baskets.Remove(basketId);
            return Task.FromResult(true);
        }

        public Task<bool> AddOrUpdateItemAsync(Item item){
            if(item == null)
                return Task.FromResult(false);
            _items[item.Id] = item;
            return Task.FromResult(true);
        }

        public Task<bool> RemoveItemAsync(int id){
            var result = _items.Remove(id);
            return Task.FromResult(result);
        }

        public void Dispose(){

            _items?.Clear();
            _items = null;
            _baskets?.Clear();
            _baskets = null;
        }
    }
}
