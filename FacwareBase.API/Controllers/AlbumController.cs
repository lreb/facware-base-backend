using System.Data.Entity;
using System.Linq;
using FacwareBase.API.Helpers.Domain.POCO;
using FacwareBase.API.Helpers.OData;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FacwareBase.API.Controllers
{
    /// <summary>
    /// Demo Controller with OData feature
    /// </summary>
    public class AlbumController : ODataController
    {
        private readonly ILogger<AlbumController> _logger;  
        private readonly MusicContext _context;
        /// <summary>
        /// Demo constructor
        /// </summary>
        /// <param name="context">database context</param>
        /// <param name="logger">serilog DI</param>
        public AlbumController(MusicContext context, ILogger<AlbumController> logger)
        {
            _logger = logger;
            _context = context;
            if(!context.Albums.Any())
            {
                _context.Database.EnsureCreated();
            }
        }
        /// <summary>
        /// Get data using OData
        /// </summary>
        /// <returns>Albums <see cref="Album"/></returns>
        [EnableQueryFromODataToAWS]
        public IActionResult Get()
        {
            try
            {
                 foreach (var query in HttpContext.Request.Query)
                {
                    _logger.LogInformation($"EventId = 5000, Message = KEY: {query.Key} VALUE: {query.Value}");
                }

                _logger.LogInformation($"Get Album");
                var albums = _context.Albums;
                return Ok(albums);    
            }
            catch (System.Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }

        /// <summary>
        /// Get data using OData
        /// </summary>
        /// <param name="key">Album id</param>
        /// <returns>Albums <see cref="Album"/></returns>
        [EnableQueryFromODataToAWS]
        public IActionResult Get(int key)
        {
            try
            {
                _logger.LogDebug($"Get Album ID");
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
            catch (System.Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }
        }
    }
}