using FluentAssertions;
using Host.Events;
using Host.Registrations;
using Host.Tests.Integration;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Angeleo.AffiliateService.Tests.Framework;

public class WhenGettingRegistration : IClassFixture<CustomApplicationFactory>
{
    private readonly CustomApplicationFactory _factory;
    private Random _random = new Random();

    public WhenGettingRegistration(CustomApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ForNotExistingRegistration_ThenShouldReturnEmptyList()
    {
        // Arrange
        var client = _factory.CreateClient();
        var registration = Mock.Of<Registration>();

        // Act
        var result = await client.InvokeGraphQlRequestWithVariables(@"
  query getRegistrations($registrationId: Int!) {
  registrations(where: {id : {eq: $registrationId }}) {
    nodes {
      id
      creationDate
      clientId
      status
    }
  }
}
", new
        {
            registrationId = registration.Id
        });

        
        // Assert
        var nodes = (JArray)result.data["registrations"]["nodes"];
        nodes.Should().BeEmpty();
    }

    [Fact]
    public async Task ForExistingRegistration_ThenReturnsValidRegistration()
    {
        // Arrange
        var client = _factory.CreateClient();
        var registration = new Registration()
        {
            Id = _random.Next(),
            Event = new Event()
            {
                Id = _random.Next(),
                EventName = Guid.NewGuid().ToString()
            },
            ClientId = _random.Next(),
            Status = RegistrationStatus.Confirmed,
            CreationDate = DateTime.Now
        };
        
        await _factory.ArrangeRegistration(registration);

        // Act
        var result = await client.InvokeGraphQlRequestWithVariables(@"
          query getRegistrations($registrationId: Int!) {
          registrations(where: {id : {eq: $registrationId }}) {
            nodes {
              id
              creationDate
              clientId
              status
            }
          }
        }
        ", new
        {
            registrationId = registration.Id
        });

        // Assert
        var registrationId = result.data["registrations"]?["nodes"]?[0]["id"];
        var clientId = result.data["registrations"]?["nodes"]?[0]["clientId"];
        
        clientId.Value<int>().Should().Be(registration.ClientId);
        registrationId.Value<int>().Should().Be(registration.Id);
    }
}