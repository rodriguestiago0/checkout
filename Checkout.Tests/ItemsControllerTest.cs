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
    public class ItemsControllerTest
    {
        private ItemsController _controller;
        private Storage _storage;
        [SetUp]
        public void Setup()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            _storage = new Storage();
            _controller = new ItemsController(_storage, mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _storage.Dispose();
        }

        [Test]
        public async Task AddItem()
        {
            var response = await _controller.Post(null);
            Assert.IsInstanceOf<BadRequestResult>(response.Result);

            response = await _controller.Post(new ItemResponse{Id = 1});
            Assert.IsInstanceOf<OkObjectResult>(response.Result);
        }

        [Test]
        public async Task DeleteItem()
        {
            await _controller.Post(new ItemResponse{Id = 1});

            var response = await _controller.Delete(0);
            Assert.IsInstanceOf<NotFoundResult>(response);

            response = await _controller.Delete(1);
            Assert.IsInstanceOf<NoContentResult>(response);
        }
    }
}