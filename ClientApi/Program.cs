using ClientApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ClientDb>(opt => opt.UseInMemoryDatabase("ClienList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "Client API");

app.MapGet("/clients", async (ClientDb db) =>
    await db.Clients.ToListAsync());

app.MapGet("/client/active", async (ClientDb db) =>
    await db.Clients.Where(x => x.IsActive).ToListAsync());

app.MapGet("/client/{id}", async (Guid id, ClientDb db) =>
    await db.Clients.FindAsync(id)
        is Client client
        ? Results.Ok(client)
        : Results.NotFound());

app.MapPost("/client/", async (Client client, ClientDb db) =>
{
    db.Clients.Add(client);
    await db.SaveChangesAsync();

    return Results.Created($"/client/{client.Id}", client);
});

app.MapPut("client/{id}", async (Guid id, Client inputClient, ClientDb db) =>
{
    var client = await db.Clients.FindAsync(id);
    if (client is null) return Results.NotFound();

    client.Name = inputClient.Name;
    client.IsActive = inputClient.IsActive;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/client/{id}", async (Guid id, ClientDb db) =>
{
    if (await db.Clients.FindAsync(id) is Client client)
    {
        db.Clients.Remove(client);
        await db.SaveChangesAsync();
        return Results.Ok(client);
    }

    return Results.NotFound("The client you are trying to remove does" +
        " not exists");
});

app.MapPost("client/deactivate/{id}", async(Guid id, ClientDb db) =>
{
    var client = await db.Clients.FindAsync(id);
    if (client is null) return Results.NotFound();

    client.IsActive = false;
    await db.SaveChangesAsync();

    return Results.Ok(client);
});


app.Run();

namespace ClientApi.Program
{
    public class Program
    {
        public void main() { }

    }
}



