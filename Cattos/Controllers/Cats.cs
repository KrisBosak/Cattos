using Cattos.Controllers.Interfaces;
using Cattos.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cattos.Controllers
{
    [ApiController]
    [Route("cats")]
    public class Cats : ControllerBase, ICats
    {
        private readonly CatClient _client;

        public Cats(CatClient client)
        {
            this._client = client;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(bool hasBreeds, int numberOfCats, int pageNumber)
        {
            return Ok(await _client.SearchCatsAsync(hasBreeds, numberOfCats, pageNumber));
        }
    }
}
