using Host.Registrations;

namespace Dbinit;

public static class RegistrationFactory
{
    private static Random _random = new Random();

    static RegistrationFactory()
    {
    }

    public static Registration Create(Guid clientId)
    {
        var registration = new Registration()
        {
            ClientId = clientId,
            Id = Guid.NewGuid(),
            CreationDate = DateTime.Now.AddSeconds(-_random.Next(100000)),
            Status = _random.Next(100) > 50 ? RegistrationStatus.Confirmed : RegistrationStatus.WaitingForConfirmation
        };
        return registration;
    }
}