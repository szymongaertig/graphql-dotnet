Modyfikacja testu sprawdzającego czy nie ma rejestracji

```
var nodes = (JArray)result.data["registrations"]["nodes"];nodes.Should().BeEmpty();
```


Modyfikacja testu, sprawdzającego czy dane rejestracji są prawidłowe


```
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
    ```