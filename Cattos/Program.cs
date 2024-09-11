using Cattos.Services;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient("CatApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["CatApi:BaseUri"] ?? "");
    client.DefaultRequestHeaders.Add("x-api-key", builder.Configuration["CatApi:ApiKey"]);
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<CatClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
