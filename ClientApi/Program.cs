using ClientApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ClientDb>(opt => opt.UseInMemoryDatabase("ClienList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/clients", async (ClientDb db) =>
    await db.Clients.ToListAsync());

app.Run();

