using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Checkout.Entities.Data;
using Checkout.Storage;
using Checkout.Api.Model;
using AutoMapper;

namespace Checkout.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private IStorage _storage;
        private readonly IMapper _mapper;

        public ItemsController(IStorage storage, IMapper mapper)
        {
            _storage = storage;
            _mapper = mapper;
        }
        // GET api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> GetAll()
        {
            var items = await _storage.GetItemsAsync();
            return Ok(items.Select(i => _mapper.Map<ItemResponse>(i)));
        }

        // GET api/items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemResponse>> Get(int id)
        {
            var item = await _storage.GetItemAsync(id);
            if(item == null)
                return NotFound();
           return Ok(_mapper.Map<ItemResponse>(item));
        }

        // POST api/items/
        [HttpPost()]
        public async Task<ActionResult<ItemResponse>> Post([FromBody] ItemResponse item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (item == null)
                return BadRequest();

            if(!await _storage.AddOrUpdateItemAsync(_mapper.Map<Item>(item)))
                return BadRequest();

            return Ok(item);
        }

        // POST api/items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if(!await _storage.RemoveItemAsync(id))
                return NotFound();

            return NoContent();
        }
                
    }
}
