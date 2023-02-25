using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ClientApi.Tests;

public class MinimalControllerTests
{
    [Fact]
    public async Task TestRootEndpoint()
    {
        await using var application = new WebApplicationFactory<ClientApi.Program.Program>();
        using var client = application.CreateClient();

        var response = await client.GetStringAsync("/");

        Assert.Equal("Client API", response);
    }

}
