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
    public class ItemsController : ControllerBase
    {
        private IStorage _storage;

        public ItemsController(IStorage storage)
        {
            _storage = storage;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> Get()
        {
            return Ok(await _storage.GetItems());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetAsync(int id)
        {
            var item = await _storage.GetItem(id);
            if(item == null)
                return NotFound();
           return Ok(item);
        }
    }
}
