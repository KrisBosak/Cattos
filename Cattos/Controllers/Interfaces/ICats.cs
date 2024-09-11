using Microsoft.AspNetCore.Mvc;

namespace Cattos.Controllers.Interfaces
{
    public interface ICats
    {
        public Task<IActionResult> Search(bool hasBreeds, int numberOfCats, int pageNumber);
    }
}
