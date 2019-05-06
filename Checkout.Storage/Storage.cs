using System.Collections.Generic;
using System.Linq;
using Checkout.Entities;

namespace Checkout.Storage
{
    public class Storage : IStorage
    {
        private List<Item> _items = new List<Item>{
            new Item{
                Id = 0,
                Description = "Item-0",
                Name = "Item-0",
                Price = 10
            },
            new Item{
                Id = 1,
                Description = "Item-1",
                Name = "Item-1",
                Price = 10
            },
            new Item{
                Id = 2,
                Description = "Item-2",
                Name = "Item-2",
                Price = 10
            }
        };

        private Dictionary<ulong, Basket> _baskets {get; set;}
        
        private ulong AvailableId = ulong.MinValue;

        public IEnumerable<Item> GetItems() {
            return _items;
        }

        public ulong InitBasket()
        {
            _baskets[AvailableId] = new Basket{
                Id = AvailableId
            };
            return AvailableId++;
        }

        public bool AddItem(ulong basketId, Item item, int count){
            if(_baskets.ContainsKey(basketId)){
                var items = _baskets[basketId].Items;
                if(items.ContainsKey(item.Id))
                    items[item.Id].Count+=count;
                else
                    items.Add(item.Id, new BasketItem{
                        Item = item,
                        Count = count
                    });
            
                return true;
            }
            return false;
        }

        public bool ClearBascket(ulong basketId){
            if(_baskets.ContainsKey(basketId)){
                _baskets[basketId].Items.Clear();
                return true;
            }
            return false;
        }

        public Basket GetBasket(ulong basketId){
            if(_baskets.ContainsKey(basketId))
                return _baskets[basketId];
            return null;
        }

        public bool ChangeQuantity(ulong basketId, ulong itemId, int count){
            var basket = _baskets[basketId];
            var item = basket.Items.FirstOrDefault(i => i.Item.Id == itemId);
            
            if(item == null)
                return false;
            
            item.Count = count;
            return true;
        }

        public void Checkout(ulong basketId){
            ClearBascket(basketId);
        }
    }
}
