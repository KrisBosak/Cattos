using Cattos.Models.Cats;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;

namespace Cattos.Services
{
    public class CatClient : HttpClient
    {
        private readonly HttpClient _httpClient;
        // How could I hide this and still use it?
        private readonly string _baseUri = "https://api.thecatapi.com/v1/";
        private readonly string _apiKey = "live_hbJoCAugAYqMslfKQfs0Sbs9jEaUrHfLRxhiEQvccQuGTDuo1MVmtXWPuTICiNOs";

        public CatClient(HttpClient httpClient) 
        {
            this._httpClient = httpClient;
        }

        public async Task<List<Images>> GetAsync(bool hasBreeds)
        {
            List<Images> catImage = new();

            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _baseUri + "images/search?" + "limit=11&has_breeds=" + hasBreeds);
            request.Headers.Add("x-api-key", _apiKey);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                catImage = JsonSerializer.Deserialize<List<Images>>(jsonString);
            }

            return catImage;
        }
    }
}
