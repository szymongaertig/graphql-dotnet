using FluentAssertions;
using Host.Clients;
using Host.Registrations;
using Host.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Host.Tests.Units;

public class WhenGettingRegistration_ExtendedByClientData
{
    [Fact]
    public async Task ThenReturnsValidEmailAddress()
    {
        // Arrange
        using var fixture = new RegistrationFixture();
        var dbContext = fixture.GetDbContext();
        await dbContext.Database.MigrateAsync();
        var registration = new RegistrationMockBuilder().Build();

        dbContext.Add(registration);
        dbContext.SaveChanges();


        var client = new ClientMockBuilder().WithClientId(registration.ClientId).Build();
        var clientServiceMock = new Mock<IClientsService>();
        clientServiceMock.Setup(x => x.GetClient(client.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(client);

        // Act
        var jsonResult = await fixture.ExecuteGraphqlRequest(
            @"query getRegistrations($registrationId: Int!) {
          registrations(where: {id : {eq: $registrationId }}) {
            nodes {
              client {
                emailAddress
              }
            }
          }
        }", new Dictionary<string, object>()
            {
                { "registrationId", registration.Id }
            },
            (services) =>
            {
                services.AddDbContextPool<RegistrationDbContext>(optionsBuilder =>
                    optionsBuilder.UseSqlite(fixture.GetConnectionString()));
                services.AddTransient<IClientsService>(_ => clientServiceMock.Object);
            });

        // Assert
        var emailAddress = jsonResult["data"]["registrations"]["nodes"]?.FirstOrDefault()["client"]
            .Value<string>("emailAddress");
        emailAddress.Should().Be(client.EmailAddress);
    }
}