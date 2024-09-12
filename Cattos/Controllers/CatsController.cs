using Cattos.Controllers.Interfaces;
using Cattos.Models.Cats;
using Cattos.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cattos.Controllers
{
    [ApiController]
    [Route("cats")]
    public class CatsController : ControllerBase, ICats
    {
        private readonly CatClient _client;

        public CatsController(CatClient client)
        {
            this._client = client;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCattos([FromQuery] int numberOfCats, [FromQuery] int pageNumber)
        {
            return Ok(await _client.SearchCattossAsync(numberOfCats, pageNumber));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCatto([FromRoute] string id)
        {
            return Ok(await _client.GetCattoAsync(id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCatto([FromBody] UploadData imageData)
        {
            // Need to fix matching from form-data 
            return Ok(await _client.CreateCattoAsync(imageData));
        }
    }
}
