var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/test-list", () =>
{
    return new List<SimpleItem>
    {
        new() { Id = 1, Name = "Item 1" },
        new() { Id = 2, Name = "Item 2" }
    };
});

app.MapGet("/test-dictionary", () =>
{
    return new Dictionary<string, double>
    {
        ["metric1"] = 1.5,
        ["metric2"] = 2.7
    };
});

app.MapGet("/test-container", () =>
{
    return new CollectionContainer();
});

app.MapGet("/test-ienum", () =>
{
    var items = new List<string> { "tag1", "tag2", "tag3" };
    return items.AsEnumerable();
});

app.Run();
