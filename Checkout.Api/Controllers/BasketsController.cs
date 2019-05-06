using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Checkout.Entities;
using Checkout.Storage;

namespace Checkout.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private IStorage _storage;

        public BasketsController(IStorage storage)
        {
            _storage = storage;
        }
        // GET api/baskets
        [HttpGet]
        public ActionResult<IEnumerable<Basket>> Get()
        {
            return Ok(_storage.GetBaskets());
        }

        // GET api/baskets/5
        [HttpGet("{id}")]
        public ActionResult<Basket> Get(int id)
        {
            var basket = _storage.GetBasket(id);

            if(basket == null)
                return NotFound();

            return basket;
        }

        // POST api/baskets
        [HttpPost]
        public ActionResult Post([FromBody] Basket basket)
        {
            if(_storage.BasketExists(basket.Id))
                return BadRequest("Basket already exists");

            _storage.AddBasket(basket);

            return NoContent();
        }

        // PUT api/baskets/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, Basket basket)
        {
            if (id != basket.Id)
                return BadRequest();

            if(!_storage.BasketExists(id))
                return BadRequest("Basket already exists");

            _storage.AddBasket(basket);

            return NoContent();
        }

        // DELETE api/baskets/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _storage.RemoveBasket(id);
            if(!success)
                return NotFound();
            return NoContent();
        }
    }
}
