using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facware.Data.Access;
using Facware.Data.Access.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Facware.Api.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private readonly FacwareDbContext _context;

        private IItemRepository _itemRepository;

        public ItemsController(FacwareDbContext context,
            IItemRepository itemRepository)
        {
            _context = context;
            _itemRepository = itemRepository;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _context.Items.ToListAsync();
            return Ok(data);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_itemRepository.GetById(id));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
