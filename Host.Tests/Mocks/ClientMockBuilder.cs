using Clients;

namespace Host.Tests.Mocks;

public class ClientMockBuilder
{
    private static Random _random = new Random();

    private Client _client = new Client();
    public ClientMockBuilder()
    {
        _client.Id = _random.Next();
        _client.EmailAddress = $"{Guid.NewGuid()}@gmail.com";
        _client.Surname = Guid.NewGuid().ToString();
        _client.Name = Guid.NewGuid().ToString();
    }

    public ClientMockBuilder WithClientId(int clientId)
    {
        _client.Id = clientId;
        return this;
    }
    
    public Client Build()
    {
        return _client;
    }
}