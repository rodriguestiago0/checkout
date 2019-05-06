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

        public Task<IEnumerable<Item>> GetItems() 
        {
            return Task.FromResult<IEnumerable<Item>>(_items.Values);
        }

        public Task<Item> GetItem(int id) 
        {
            if(_items.ContainsKey(id))
                return null;
            return Task.FromResult(_items[id]);
        }

        public Task<int> InitBasket()
        {
            _baskets[AvailableId] = new Basket{
                Id = AvailableId
            };
            return Task.FromResult(AvailableId++);
        }

        public Task<bool> AddItem(int basketId, int itemId, int count)
        {
            if(_baskets.ContainsKey(basketId) && _items.ContainsKey(itemId)){
                var item = _items[itemId];
                var items = _baskets[basketId].Items;
                if(items.ContainsKey(item.Id))
                    items[item.Id].Count+=count;
                else
                    items.Add(item.Id, new BasketItem{
                        Item = item,
                        Count = count
                    });
            
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        private Task<bool> ClearBascket(int basketId)
        {
            if(_baskets.ContainsKey(basketId)){
                _baskets[basketId].Items.Clear();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<IEnumerable<Basket>> GetBaskets()
        {
            return Task.FromResult<IEnumerable<Basket>>(_baskets.Values);
        }

        public Task<Basket> GetBasket(int basketId)
        {
            if(_baskets.ContainsKey(basketId))
                return Task.FromResult(_baskets[basketId]);
            return null;
        }

        public Task<bool> BasketExists(int id)
        {
            return Task.FromResult(_baskets.ContainsKey(id));
        }

        public Task<bool> AddOrReplaceBasket(Basket basket)
        {
            if(basket == null)
                return Task.FromResult(false);
            if(basket.Items.Values.Any(basketItem => !_items.ContainsKey(basketItem.Item.Id)))
                return Task.FromResult(false);
            if(basket != null)
                _baskets[basket.Id] = basket;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeQuantity(int basketId, int itemId, int count)
        {
            var basket = _baskets[basketId];
            
            if(!basket.Items.ContainsKey(itemId))
                return Task.FromResult(false);
            
            var item = basket.Items[itemId];
            item.Count = count;
            return Task.FromResult(true);
        }

        public Task Checkout(int basketId)
        {
            ClearBascket(basketId);
            return Task.CompletedTask;
        }

        public Task<bool> RemoveBasket(int basketId)
        {
            if(!_baskets.ContainsKey(basketId))
                return Task.FromResult(false);
            _baskets.Remove(basketId);
            return Task.FromResult(true);
        }

        public void Dispose(){

            _items?.Clear();
            _items = null;
            _baskets?.Clear();
            _baskets = null;
        }
    }
}
