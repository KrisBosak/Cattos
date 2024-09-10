using Cattos.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddHttpClient("catClient", c =>
//{
//    c.BaseAddress = new Uri("https://api.thecatapi.com/v1/");
//    c.DefaultRequestHeaders.Add("Accept", "application/json");
//    c.DefaultRequestHeaders.Add("x-api-key", "live_hbJoCAugAYqMslfKQfs0Sbs9jEaUrHfLRxhiEQvccQuGTDuo1MVmtXWPuTICiNOs");
//});
builder.Services.AddScoped<CatClient>();
builder.Services.AddScoped<HttpClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
