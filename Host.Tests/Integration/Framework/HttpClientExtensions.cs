using Angeleo.AffiliateService.Tests.Framework;
using Newtonsoft.Json;

public static class HttpClientExtensions
{
    public static async Task<GraphQlResponse> InvokeGraphQlRequestWithVariables(this HttpClient client,
        string query,
        object? variables = null,
        string? operationName = null)
    {
        var result = await client.PostAsync("graphql", new JsonContent(new
        {
            query,
            variables,
            operationName
        }));
        var responseBody = await result.Content.ReadAsStringAsync();
        if (!result.IsSuccessStatusCode) throw new Exception($"Invalid service response: {responseBody}");
        var response = JsonConvert.DeserializeObject<GraphQlResponse>(responseBody);
        return response;
    }
}