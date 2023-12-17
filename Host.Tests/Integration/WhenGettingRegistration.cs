using FluentAssertions;
using Host.Events;
using Host.Registrations;
using Host.Tests.Integration;
using Host.Tests.Mocks;
using Moq;
using Snapshooter.Xunit;
using Xunit;

namespace Angeleo.AffiliateService.Tests.Framework;

public class WhenGettingRegistration : IClassFixture<CustomApplicationFactory>
{
    private readonly CustomApplicationFactory _factory;

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
        Snapshot.Match(result.data);
    }


    [Fact]
    public async Task ForExistingRegistration_ThenReturnsValidRegistrationId()
    {
        // Arrange
        var client = _factory.CreateClient();
        var registration = new RegistrationMockBuilder().Build();
        await _factory.ArrangeRegistration(registration);

        // Act
        var result = await client.InvokeGraphQlRequestWithVariables(@"
          query getRegistrations($registrationId: Int!) {
          registrations(where: {id : {eq: $registrationId }}) {
            nodes {
              id
            }
          }
        }
        ", new
        {
            registrationId = registration.Id
        });

        // Assert
        var registrationId = result.data["registrations"]["nodes"]?.FirstOrDefault()?.Value<int>("id");
        registrationId.Should().Be(registration.Id);
    }

    [Fact]
    public async Task ForExistingRegistration_ThenReturnsValidClientId()
    {
        // Arrange
        var client = _factory.CreateClient();
        var registration = new RegistrationMockBuilder().Build();
        await _factory.ArrangeRegistration(registration);

        // Act
        var result = await client.InvokeGraphQlRequestWithVariables(@"
          query getRegistrations($registrationId: Int!) {
          registrations(where: {id : {eq: $registrationId }}) {
            nodes {
              clientId
            }
          }
        }
        ", new
        {
            registrationId = registration.Id
        });

        // Assert
        result.data["registrations"]["nodes"]?.FirstOrDefault()?.Value<int>("clientId").Should()
            .Be(registration.ClientId);
    }

    [Fact]
    public async Task ForExistingRegistration_ThenReturnsValidRegistration()
    {
        // Arrange
        var client = _factory.CreateClient();
        var registrationId = 10;
        var registration = new Registration()
        {
            Id = registrationId,
            Event = new Event
            {
                Id = 1,
                EventName = "This is some event2"
            },
            ClientId = 100,
            Status = RegistrationStatus.Confirmed,
            CreationDate = new DateTime(2023, 10, 10)
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
              event {
                id
              }
            }
          }
        }
        ", new
        {
            registrationId = registrationId
        });

        // Assert
        result.data.MatchSnapshot();
    }
}