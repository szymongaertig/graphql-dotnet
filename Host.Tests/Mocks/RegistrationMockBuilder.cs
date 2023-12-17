using Host.Events;
using Host.Registrations;

namespace Host.Tests.Mocks;

public class RegistrationMockBuilder
{
    private static Random _random = new Random();

    private Registration _registration = new Registration()
    {
        Id = _random.Next(),
        CreationDate = DateTime.Now.AddMinutes(-_random.Next(1000)),
        ClientId = _random.Next(),
        Status = _random.Next() % 2 == 0 ? RegistrationStatus.Confirmed : RegistrationStatus.WaitingForConfirmation,
        Event = new Event()
        {
            Id = _random.Next(),
            EventName = Guid.NewGuid().ToString()
        }
    };

    public Registration Build()
    {
        return _registration;
    }
}