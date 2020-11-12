using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RedisDistributedCacheWithDotNet5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IDistributedCache distributedCache;
        public BooksController(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        // GET: api/<MoviesController>
        [HttpGet("{actorname}")]
        public async Task<IActionResult> GetAsync(string actorname)
        {
            //await Task.Delay(100);
            var cacheKey = actorname.ToLower();
            List<string> bookList = new List<string>();
            string serializedBooks;
            var encodedBooks = await distributedCache.GetAsync(cacheKey);
            if (encodedBooks != null)
            {
                serializedBooks = Encoding.UTF8.GetString(encodedBooks);
                bookList = JsonConvert.DeserializeObject<List<string>>(serializedBooks);
            }
            else 
            {
                // cacheKey = Mark Twain
                bookList.Add("The Celebrated Jumping Frog of Calaveras County (1865)"); 
                bookList.Add("The Innocents Abroad (1869)");
                bookList.Add("Roughing It (1872)"); 
                bookList.Add("The Gilded Age (1873)");
                serializedBooks = JsonConvert.SerializeObject(bookList);
                encodedBooks = Encoding.UTF8.GetBytes(serializedBooks);

                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                await distributedCache.SetAsync(cacheKey, encodedBooks, options);
            }
            return Ok(bookList);
        }

        // GET api/<MoviesController>/5
        [HttpGet]
        public string Get()
        {
            return "value";
        }

        // POST api/<MoviesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
