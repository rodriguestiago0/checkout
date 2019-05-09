using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.Api.Controllers;
using Checkout.Api.Model;
using Checkout.Entities.Data;
using Checkout.Storage;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class BasketsControllerTest
    {
        private BasketsController _controller;
        private IStorage _storage;
        [SetUp]
        public void Setup()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            _storage = new Storage();
            _controller = new BasketsController(_storage, mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _storage.Dispose();
        }

        [Test]
        public async Task AddBasket()
        {
            var response = await _controller.Post(null);
            Assert.IsInstanceOf<BadRequestResult>(response.Result);

            response = await _controller.Post(new BasketResponse{Id = 1});
            Assert.IsInstanceOf<OkObjectResult>(response.Result);
        }

        [Test]
        public async Task DeleteBasket()
        {
            await _controller.Post(new BasketResponse{Id = 1});

            var response = await _controller.Delete(0);
            Assert.IsInstanceOf<NotFoundResult>(response);

            response = await _controller.Delete(1);
            Assert.IsInstanceOf<NoContentResult>(response);
        }

        [Test]
        public async Task AddItemToBasket()
        {
            var response = await _controller.AddItemToBasket(1, 1, 1);
            Assert.IsInstanceOf<BadRequestObjectResult>(response);

            await _storage.AddOrUpdateItemAsync(new Item{Id = 1});

            var id = await _storage.InitBasketAsync();
            response = await _controller.AddItemToBasket(id, 1, 1);
            Assert.IsInstanceOf<NoContentResult>(response);
        }

        [Test]
        public async Task RemoveItemFromBasket()
        {
            var response = await _controller.RemoveItemFromBasket(1, 1);
            Assert.IsInstanceOf<BadRequestObjectResult>(response);

            await _storage.AddOrUpdateItemAsync(new Item{Id = 1});

            var id = await _storage.InitBasketAsync();
            response = await _controller.AddItemToBasket(id, 1, 1);
            Assert.IsInstanceOf<NoContentResult>(response);
        }

        [Test]
        public async Task ClearBasket()
        {
            var response = await _controller.ClearBasket(0);
            Assert.IsInstanceOf<BadRequestObjectResult>(response);

            await _storage.AddOrUpdateItemAsync(new Item{Id = 1});

            var id = await _storage.InitBasketAsync();
            response = await _controller.AddItemToBasket(id, 1, 1);
            Assert.IsInstanceOf<NoContentResult>(response);

            response = await _controller.ClearBasket(id);
            Assert.IsInstanceOf<NoContentResult>(response);
        }

        [Test]
        public async Task CheckoutBasket()
        {
            var response = await _controller.CheckoutBasket(0);
            Assert.IsInstanceOf<BadRequestObjectResult>(response.Result);

            await _storage.AddOrUpdateItemAsync(new Item{Id = 1, Price = 1});

            var id = await _storage.InitBasketAsync();
            response = await _controller.AddItemToBasket(id, 1, 1);

            response = await _controller.CheckoutBasket(id);
            Assert.IsInstanceOf<OkObjectResult>(response.Result);
        }

        [Test]
        public async Task BasketPrice()
        {
            var response = await _controller.BasketPrice(0);
            Assert.IsInstanceOf<BadRequestObjectResult>(response.Result);

            await _storage.AddOrUpdateItemAsync(new Item{Id = 1, Price = 1});

            var id = await _storage.InitBasketAsync();
            response = await _controller.AddItemToBasket(id, 1, 1);

            response = await _controller.BasketPrice(id);
            Assert.IsInstanceOf<OkObjectResult>(response.Result);
        }
    }
}