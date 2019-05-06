using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Checkout.Entities.Data;
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
        public async Task<ActionResult<IEnumerable<Basket>>> Get()
        {
            return Ok(await _storage.GetBasketsAsync());
        }

        // GET api/baskets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Basket>> Get(int id)
        {
            var basket = await _storage.GetBasketAsync(id);

            if(basket == null)
                return NotFound();

            return basket;
        }

        // POST api/baskets
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Basket basket)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (basket == null)
                return BadRequest();

            if(await _storage.BasketExistsAsync(basket.Id))
                return BadRequest("Basket already exists");

            if(!await _storage.AddOrReplaceBasketAsync(basket))
                return BadRequest("One of items does not exist.");

            return Ok(await _storage.GetBasketAsync(basket.Id));
        }
        
        [Route("{id}/order/{itemId}/{count}")]
        [HttpPut]
        public async Task<ActionResult> AddItemToBasket(int id, int itemId, int count)
        {
            if(count <=0)
                return BadRequest();

            if(!await _storage.AddItemAsync(id, itemId, count))
                return BadRequest("Item does not exist.");
            return NoContent();
        }

        // PUT api/baskets/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Basket basket)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (basket == null || id != basket.Id)
                return BadRequest();

            if(!await _storage.BasketExistsAsync(id))
                return BadRequest("Basket already exists");

            if(!await _storage.AddOrReplaceBasketAsync(basket))
                return BadRequest("One of items does not exist.");

            return NoContent();
        }

        // DELETE api/baskets/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if(!await _storage.RemoveBasketAsync(id))
                return NotFound();
            return NoContent();
        }
    }
}
