using Cattos.Models.Cats;
using Microsoft.AspNetCore.Mvc;

namespace Cattos.Controllers.Interfaces
{
    public interface ICats
    {
        public Task<IActionResult> SearchCattos(int numberOfCats, int pageNumber);
        public Task<IActionResult> GetCatto(string id);
        public Task<IActionResult> CreateCatto(UploadData imageData);
    }
}
