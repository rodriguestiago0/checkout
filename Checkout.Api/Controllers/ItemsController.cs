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
    public class ItemsController : ControllerBase
    {
        private IStorage _storage;

        public ItemsController(IStorage storage)
        {
            _storage = storage;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Item>> Get()
        {
            return Ok(_storage.GetItems());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var item = _storage.GetItem(id);
            if(item == null)
                return NotFound();
           return Ok();
        }
    }
}
