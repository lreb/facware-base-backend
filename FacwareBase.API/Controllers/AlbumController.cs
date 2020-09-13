using System.Data.Entity;
using System.Linq;
using FacwareBase.API.Helpers.Domain.POCO;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;

namespace FacwareBase.API.Controllers
{
    //[ODataRoutePrefix("Albums")]
    public class AlbumController : ODataController
    {
        private readonly MusicContext _context;
        public AlbumController(MusicContext context)
        {
            _context = context;
            if(!context.Albums.Any())
            {
                _context.Database.EnsureCreated();
            }
        }

        [EnableQuery]
        public IActionResult Get()
        {
            var albums = _context.Albums;
            return Ok(albums);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var album =  _context.Albums.Include(a => a.Songs).Where(a => a.Id == key);
            if(album == null)
            {
                return NotFound();
            }
            return Ok(album);
        }
    }
}