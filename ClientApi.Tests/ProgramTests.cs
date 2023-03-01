using System;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Newtonsoft.Json;

namespace ClientApi.Tests;

public class MinimalControllerTests
{

    private Guid clientGuid;

    public MinimalControllerTests()
    {
        this.clientGuid = Guid.Parse("09a2520f-419e-4fa7-9792-2a4eb3d782aa");
    }

    [Fact]
    [Trait("Priority", "-2")]
    public async Task TestRootEndpoint()
    {
        // Arrange
        await using var application = new WebApplicationFactory<ClientApi.Program.Program>();
        using var client = application.CreateClient();

        // Act
        var response = await client.GetStringAsync("/");

        // Assert
        Assert.Equal("Client API", response);
    }

    [Fact]
    [Trait("Priority", "-1")]
    public async Task TestGetEmptyClientsEndpoint()
    {
        // Arrange
        await using var application = new WebApplicationFactory<ClientApi.Program.Program>();
        using var client = application.CreateClient();

        // Act
        var deleteResponse = await client.DeleteAsync($"/client/{clientGuid}");
        var response = await client.GetStringAsync("/clients");

        // Assert
        Assert.Equal("[]", response);
    }


    [Fact]
    [Trait("Priority", "3")]
    public async Task TestPostEndpoint()
    {
        // Arrange
        await using var application = new WebApplicationFactory<ClientApi.Program.Program>();
        using var client = application.CreateClient();

        var clientMock = new Client {  Id = clientGuid, IsActive= true, Name= "Client"};

        var content = new StringContent(JsonConvert.SerializeObject(clientMock), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/client/", content);


        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    [Trait("Priority", "4")]
    public async Task TestGetWithResultsEndpoint()
    {
        // Arrange
        await using var application = new WebApplicationFactory<ClientApi.Program.Program>();
        using var client = application.CreateClient();

        // Act
        var response = await client.GetAsync("/clients");

        string json = await response.Content.ReadAsStringAsync();
        Client[] clients = JsonConvert.DeserializeObject<Client[]>(json);

        // Assert
        Assert.NotNull(clients.First().Name);
    }
}
