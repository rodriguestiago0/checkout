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

        private Dictionary<int, Basket> _baskets {get; set;}
        
        private int AvailableId = 0;

        public IEnumerable<Item> GetItems() {
            return _items;
        }

        public Item GetItem(int id) {
            return _items.FirstOrDefault(i => i.Id == id);
        }

        public int InitBasket()
        {
            _baskets[AvailableId] = new Basket{
                Id = AvailableId
            };
            return AvailableId++;
        }

        public bool AddItem(int basketId, Item item, int count){
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

        private bool ClearBascket(int basketId){
            if(_baskets.ContainsKey(basketId)){
                _baskets[basketId].Items.Clear();
                return true;
            }
            return false;
        }

        public IEnumerable<Basket> GetBaskets(){
            return _baskets.Values;
        }

        public Basket GetBasket(int basketId){
            if(_baskets.ContainsKey(basketId))
                return _baskets[basketId];
            return null;
        }

        public bool BasketExists(int id){
            return _baskets.ContainsKey(id);
        }

        public bool AddBasket(Basket basket){
            if(_baskets.ContainsKey(basket.Id))
                return false;
            _baskets.Add(basket.Id, basket);
            return true;
        }

        public bool ChangeQuantity(int basketId, int itemId, int count){
            var basket = _baskets[basketId];
            
            if(!basket.Items.ContainsKey(itemId))
                return false;
            
            var item = basket.Items[itemId];
            item.Count = count;
            return true;
        }

        public void Checkout(int basketId){
            ClearBascket(basketId);
        }

        public bool RemoveBasket(int basketId){
            if(!_baskets.ContainsKey(basketId))
                return false;
            _baskets.Remove(basketId);
            return true;
        }
    }
}
