using System.Linq;
using System.Threading.Tasks;
using Checkout.Entities.Data;
using Checkout.Storage;
using NUnit.Framework;

namespace Tests
{
    public class StorageTest
    {
        private IStorage _storage;
        [SetUp]
        public void Setup()
        {
            _storage = new Storage();
        }

        [TearDown]
        public void TearDown()
        {
            _storage.Dispose();
        }

        [Test]
        public async Task AddItem()
        {
            var item = new Item{
                Id = 1
            };
            var result = await _storage.AddOrUpdateItemAsync(item);
            Assert.IsTrue(result);
            
            result = await _storage.AddOrUpdateItemAsync(null);
            Assert.IsFalse(result);
            
            var item2 = new Item{
                Id = 2
            };
            result = await _storage.AddOrUpdateItemAsync(item2);
            Assert.IsTrue(result);

            var items = await _storage.GetItemsAsync();
            Assert.AreEqual(2, items.Count());
        }

        [Test]
        public async Task UpdateItem()
        {
            var item = new Item{
                Id = 1,
                Name = "1",
                Price = 1m,
                Description = "d"
            };
            var result = await _storage.AddOrUpdateItemAsync(item);
            Assert.IsTrue(result);
            
            item = await _storage.GetItemAsync(item.Id);
            Assert.AreEqual("1", item.Name);
            Assert.AreEqual("d", item.Description);
            Assert.AreEqual(1m, item.Price);

            item.Name = "2";
            item.Price = 2m;
            item.Description = "d2";

            result = await _storage.AddOrUpdateItemAsync(item);
            Assert.IsTrue(result);
            item = await _storage.GetItemAsync(item.Id);
            Assert.AreEqual("2", item.Name);
            Assert.AreEqual("d2", item.Description);
            Assert.AreEqual(2m, item.Price);

        }

        [Test]
        public async Task RemoveItem()
        {
            var item = new Item{
                Id = 1
            };
            var result = await _storage.AddOrUpdateItemAsync(item);
            Assert.IsTrue(result);
            
            var items = await _storage.GetItemsAsync();
            Assert.AreEqual(1, items.Count());

            result = await _storage.RemoveItemAsync(1);
            Assert.IsTrue(result);

            result = await _storage.RemoveItemAsync(1);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateBasket()
        {
            var basketId = await _storage.InitBasketAsync();
            Assert.GreaterOrEqual(0, basketId);

            var basket = await _storage.GetBasketAsync(basketId);
            Assert.NotNull(basket);
            Assert.AreEqual(0, basket.Items.Count);

             var item = new Item{
                Id = 1
            };
            await _storage.AddOrUpdateItemAsync(item);

            basket.Items.Add(1, new BasketItem{
                Item = item,
                Count = 1
            });

            await _storage.AddOrReplaceBasketAsync(basket);

            basket = await _storage.GetBasketAsync(basketId);
            Assert.NotNull(basket);
            Assert.AreEqual(1, basket.Items.Count);
        }

        [Test]
        public async Task GetBaskets()
        {
            var basketId = await _storage.InitBasketAsync();
            Assert.GreaterOrEqual(basketId, 0);

            var baskets = await _storage.GetBasketsAsync();
            Assert.NotNull(baskets);
            Assert.AreEqual(1, baskets.Count());

            basketId = await _storage.InitBasketAsync();
            Assert.GreaterOrEqual(basketId, 0);

            baskets = await _storage.GetBasketsAsync();
            Assert.NotNull(baskets);
            Assert.AreEqual(2, baskets.Count());
        }

        [Test]
        public async Task AddItemToBasket()
        {
            var basketId = 1;
            var itemId = 1;
            var item = new Item{
                Id = itemId
            };
            await _storage.AddOrUpdateItemAsync(item);
            var basket = new Basket{
                Id = basketId
            };
            basket.Items.Add(itemId, new BasketItem{
                Item = item,
                Count = 1
            });

            await _storage.AddOrReplaceBasketAsync(basket);

            basket = await _storage.GetBasketAsync(basketId);
            Assert.NotNull(basket);
            Assert.AreEqual(1, basket.Items.Count);
            Assert.AreEqual(1, basket.Items[itemId].Count);
            Assert.AreEqual(1, basket.Items[itemId].Item.Id);

            var result = await _storage.AddOrReplaceItemAsync(basketId, itemId, 5);
            Assert.IsTrue(result);
            Assert.AreEqual(1, basket.Items.Count);
            Assert.AreEqual(5, basket.Items[itemId].Count);
            Assert.AreEqual(1, basket.Items[itemId].Item.Id);


            result = await _storage.AddOrReplaceItemAsync(basketId, 2, 5);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveItemFromBasket(){
            var basketId = 1;
            var itemId = 1;
            var item = new Item{
                Id = itemId
            };
            await _storage.AddOrUpdateItemAsync(item);
            var basket = new Basket{
                Id = basketId
            };
            basket.Items.Add(1, new BasketItem{
                Item = item,
                Count = 1
            });

            await _storage.AddOrReplaceBasketAsync(basket);
            var result = await _storage.RemoveItemFromBasket(basketId, 2);
            basket = await _storage.GetBasketAsync(basketId);
            Assert.IsFalse(result);
            Assert.AreEqual(1, basket.Items.Count);

            result = await _storage.RemoveItemFromBasket(basketId, itemId);
            basket = await _storage.GetBasketAsync(basketId);
            Assert.IsTrue(result);
            Assert.AreEqual(0, basket.Items.Count);
        }

        [Test]
        public async Task ClearBasket(){
            var basketId = 1;
            var itemId = 1;
            var item = new Item{
                Id = itemId
            };
            await _storage.AddOrUpdateItemAsync(item);
            var basket = new Basket{
                Id = basketId
            };
            basket.Items.Add(1, new BasketItem{
                Item = item,
                Count = 1
            });

            await _storage.AddOrReplaceBasketAsync(basket);
            var result = await _storage.ClearBascketAsync(basketId);
            basket = await _storage.GetBasketAsync(basketId);
            Assert.IsTrue(result);
            Assert.AreEqual(0, basket.Items.Count);

            result = await _storage.ClearBascketAsync(2);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveBasket(){
            var basketId = 1;
            var itemId = 1;
            var item = new Item{
                Id = itemId
            };
            await _storage.AddOrUpdateItemAsync(item);
            var basket = new Basket{
                Id = basketId
            };
            basket.Items.Add(1, new BasketItem{
                Item = item,
                Count = 1
            });

            await _storage.AddOrReplaceBasketAsync(basket);
            var result = await _storage.RemoveBasketAsync(basketId);
            basket = await _storage.GetBasketAsync(basketId);
            Assert.IsTrue(result);
            Assert.IsNull(basket);

            result = await _storage.RemoveBasketAsync(2);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CheckoutBasket(){
            var basketId = 1;
            var itemId = 1;
            var item = new Item{
                Id = itemId, 
                Price = 4.5m
            };
            var item2 = new Item{
                Id = itemId, 
                Price = 3.5m
            };
            await _storage.AddOrUpdateItemAsync(item);
            await _storage.AddOrUpdateItemAsync(item2);
            var basket = new Basket{
                Id = basketId
            };
            basket.Items.Add(1, new BasketItem{
                Item = item,
                Count = 2
            });

            await _storage.AddOrReplaceBasketAsync(basket);
            var result = await _storage.CheckoutAsync(basketId);
            Assert.AreEqual(9m, result);

            result = await _storage.CheckoutAsync(basketId);
            Assert.AreEqual(0m, result);

            result = await _storage.CheckoutAsync(2);
            Assert.AreEqual(0m, result);

            basket = new Basket{
                Id = ++basketId
            };
            basket.Items.Add(1, new BasketItem{
                Item = item,
                Count = 2
            });

            basket.Items.Add(2, new BasketItem{
                Item = item2,
                Count = 1
            });

            await _storage.AddOrReplaceBasketAsync(basket);
            result = await _storage.CheckoutAsync(basketId);
            Assert.AreEqual(9m + 3.5m, result);
        }
    }
}