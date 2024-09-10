using Cattos.Controllers.Interfaces;
using Cattos.Models.Cats;
using Cattos.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        public async Task<IActionResult> Search(bool hasBreeds)
        {
            return Ok(await _client.GetAsync(hasBreeds));
        }
    }
}
