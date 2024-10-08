﻿using Cattos.Models.Cats;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Cattos.Services
{
    public class CatClient
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly IMemoryCache _memoryCache;

        // This should be defined somewhere else - I could probably make a custom class/service which could define/handle everything
        private static MemoryCacheEntryOptions GetCacheOptions()
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

        public async Task<List<Images>> SearchCattossAsync(int numberOfCats, int pageNumber)
        {
            HttpClient client = _httpClientFactory.CreateClient("CatApiClient");
            string cacheKey = $"search_limit={numberOfCats}&page={pageNumber}";

            if (_memoryCache.TryGetValue(cacheKey, out List<Images>? cachedData))
            {
                if (cachedData == null)
                {
                    throw new InvalidDataException("No data found");
                }

                return cachedData;
            }

            using (HttpResponseMessage response = await client.GetAsync($"images/search?order=ASC&limit={numberOfCats}&page={pageNumber}&has_breeds=true"))
            {
                response.EnsureSuccessStatusCode();

                string jsonString = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    throw new InvalidOperationException("No content received.");
                }

                List<Images>? catImages = JsonSerializer.Deserialize<List<Images>>(jsonString);
                if (catImages == null)
                {
                    throw new JsonException("Can't deserialize the content.");
                }

                _memoryCache.Set(cacheKey, catImages, GetCacheOptions());

                return catImages;
            }
        }

        public async Task<string> GetCattoAsync(string id)
        {
            HttpClient client = _httpClientFactory.CreateClient("CatApiClient");
            string cacheKey = $"getSingle_catId={id}";

            if (_memoryCache.TryGetValue(cacheKey, out string? cachedData))
            {
                if (string.IsNullOrWhiteSpace(cachedData))
                {
                    throw new InvalidDataException("No data found");
                }

                return cachedData;
            }

            using (HttpResponseMessage response = await client.GetAsync($"images/{id}"))
            {
                response.EnsureSuccessStatusCode();

                string jsonString = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    throw new InvalidOperationException("No content received.");
                }

                Images? catImage = JsonSerializer.Deserialize<Images>(jsonString);
                if (catImage == null)
                {
                    throw new JsonException("Can't deserialize the content.");
                }

                var breedsList = catImage.Breeds
                    .GroupBy(g => g.Name)
                    .Select(b => b.Key)
                    .ToList();
                string breeds = string.Join(", ", breedsList);

                string toReturn = catImage.Breeds.GroupBy(g => g.Name).Count() > 1
                    ? $"The picture features a delightful variety of cat breeds: {breeds}. Each one adds its own unique charm and personality to the mix!"
                    : $"This striking image highlights a splendid {breeds} cat, admired for its distinctive features and elegant presence!";

                _memoryCache.Set(cacheKey, toReturn, GetCacheOptions());

                return toReturn;
            }
        }

        public async Task<string> CreateCattoAsync(UploadData imageData)
        {
            HttpClient client = _httpClientFactory.CreateClient("CatApiClient");

            StreamContent fileStreamContent = new StreamContent(imageData.File.OpenReadStream());
            fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imageData.File.ContentType);
            MultipartFormDataContent content = new MultipartFormDataContent
            {
                { fileStreamContent, "file", imageData.File.FileName },
                { new StringContent("sub_id"), imageData.sub_id },
                { new StringContent("breed_ids"), imageData.breed_ids }
            };

            using (HttpResponseMessage response = await client.PostAsync($"images/upload", content))
            {
                response.EnsureSuccessStatusCode();

                string jsonString = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    throw new InvalidOperationException("No content received.");
                }

                Images? catImage = JsonSerializer.Deserialize<Images>(jsonString);
                if (catImage == null)
                {
                    throw new JsonException("Can't deserialize the content.");
                }

                return $"You uploaded a picture of a cat. The id of your picture is: {catImage.Id} and you can view the picture here: {catImage.Url}";
            }
        }
    }
}
