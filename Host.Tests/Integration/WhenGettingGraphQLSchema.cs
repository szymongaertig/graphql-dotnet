using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Snapshooter.Xunit;
using Xunit;

namespace Host.Tests.ComponentTests;

public class WhenGettingGraphQLSchema : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WhenGettingGraphQLSchema(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ThenRespondWithHttp200OK()
    {
        // Arrange / Act
        var client = _factory.CreateClient();
        var result = await InvokeGetSchemaRequest(client);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private static async Task<HttpResponseMessage> InvokeGetSchemaRequest(HttpClient client)
    {
        var assembly = typeof(WhenGettingGraphQLSchema).Assembly;
        using var streamReader =
            new StreamReader(
                assembly.GetManifestResourceStream("Host.Tests.Integration.SchemaQuery.json"));
        var queryBody = await streamReader.ReadToEndAsync();
        var content = new StringContent(queryBody);
        content.Headers.Remove("Content-Type");
        content.Headers.Add("Content-Type", "application/json");
        var result = await client.PostAsync("/graphql", content);
        return result;
    }

    [Fact]
    public async Task ThenRespondWithValidSchema()
    {
        // Arrange / Act
        var client = _factory.CreateClient();
        var result = await InvokeGetSchemaRequest(client);

        // Assert
        var resultBody = await result.Content.ReadAsStringAsync();
        Snapshot.Match(resultBody);
    }
}