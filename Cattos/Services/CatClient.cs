using Cattos.Models.Cats;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Cattos.Services
{
    public class CatClient
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly IMemoryCache _memoryCache;

        // This should be defined somewhere else - I could probably make a custom class/service which could define/handle everything
        private MemoryCacheEntryOptions GetCacheOptions()
        {
            return new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
                SlidingExpiration = TimeSpan.FromMinutes(1),
            };
        }

        public CatClient(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        { 
            this._httpClientFactory = httpClientFactory;
            this._memoryCache = memoryCache;
        }

        public async Task<List<Images>> SearchCatsAsync(bool hasBreeds, int numberOfCats, int pageNumber)
        {
            HttpClient client = _httpClientFactory.CreateClient("CatApiClient");
            string cacheKey = $"limit={numberOfCats}&page={pageNumber}&has_breeds={hasBreeds}";

            if (_memoryCache.TryGetValue(cacheKey, out List<Images> cachedData))
            {
                if (cachedData == null)
                {
                    throw new InvalidDataException("No data found");
                }

                return cachedData;
            }

            using (HttpResponseMessage response = await client.GetAsync($"images/search?order=ASC&limit={numberOfCats}&page={pageNumber}&has_breeds={hasBreeds}"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("Request not successful - stopped with the status code: " + response.StatusCode);
                }

                string jsonString = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    throw new InvalidOperationException("No content received.");
                }

                List<Images>? catImage = JsonSerializer.Deserialize<List<Images>>(jsonString);
                if (catImage == null)
                {
                    throw new JsonException("Can't deserialize the content.");
                }

                _memoryCache.Set(cacheKey, catImage, GetCacheOptions());

                return catImage;
            }
        }
    }
}
