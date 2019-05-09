using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Checkout.Entities.Data;
using Checkout.Storage;
using AutoMapper;
using Checkout.Api.Model;

namespace Checkout.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private IStorage _storage;
        private readonly IMapper _mapper;

        public BasketsController(IStorage storage, IMapper mapper)
        {
            _storage = storage;
            _mapper = mapper;
        }
        // GET api/baskets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BasketResponse>>> GetAll()
        {
            var baskets = await _storage.GetBasketsAsync();
            return Ok(baskets.Select(b => _mapper.Map<BasketResponse>(b)));
        }

        // GET api/baskets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketResponse>> Get(int id)
        {
            var basket = await _storage.GetBasketAsync(id);

            if(basket == null)
                return NotFound();

            return Ok(_mapper.Map<BasketResponse>(basket));
        }

        // POST api/baskets
        [HttpPost]
        public async Task<ActionResult<BasketResponse>> Post([FromBody] BasketResponse basket)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (basket == null)
                return BadRequest();

            if(await _storage.BasketExistsAsync(basket.Id))
                return BadRequest("Basket already exists");

            if(!await _storage.AddOrReplaceBasketAsync(_mapper.Map<Basket>(basket)))
                return BadRequest("One of items does not exist.");

            return Ok(basket);
        }

        // PUT api/baskets/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, BasketResponse basket)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (basket == null || id != basket.Id)
                return BadRequest();

            if(!await _storage.AddOrReplaceBasketAsync(_mapper.Map<Basket>(basket)))
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

        [Route("{id}/add/{itemId}/{count}")]
        [HttpPut]
        public async Task<ActionResult> AddItemToBasket(int id, int itemId, int count)
        {
            if(!await _storage.AddOrReplaceItemOnBasketAsync(id, itemId, count))
                return BadRequest("Item does not exist.");
            return NoContent();
        }

        [Route("{id}/remove/{itemId}/{count}")]
        [HttpPut]
        public async Task<ActionResult> RemoveItemFromBasket(int id, int itemId)
        {
            if(!await _storage.RemoveItemFromBasket(id, itemId))
                return BadRequest("Item does not exist.");

            return NoContent();
        }

        [Route("{id}/clear")]
        [HttpPut]
        public async Task<ActionResult> ClearBasket(int id)
        {
            if(!await _storage.ClearBascketAsync(id))
                return BadRequest("Basket does not exist.");
            return NoContent();
        }

        [Route("create")]
        [HttpPost]
        public async Task<ActionResult<int>> CreateBasket()
        {
            var basketId = await _storage.InitBasketAsync();
            return Ok(basketId);
        }

        [Route("{id}/checkout")]
        [HttpPost]
        public async Task<ActionResult<decimal>> CheckoutBasket(int id)
        {
            var price = await _storage.CheckoutAsync(id);
            if(price == 0)
                return BadRequest("Basket is empty or does not exist.");
            return Ok(price);
        }

        [Route("{id}/price")]
        [HttpGet]
        public async Task<ActionResult<decimal>> BasketPrice(int id)
        {
            var price = await _storage.GetTotalPriceAsync(id);
            if(price == 0)
                return BadRequest("Basket is empty or does not exist.");
            return Ok(price);
        }
    }
}
