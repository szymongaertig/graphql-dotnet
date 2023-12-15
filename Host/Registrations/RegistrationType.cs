using Clients;
using Host.Clients;

namespace Host.Registrations;

public class RegistrationType : ObjectType<Registration>
{
    protected override void Configure(IObjectTypeDescriptor<Registration> descriptor)
    {
        descriptor
            .Field("client")
            .Resolve<Client?>((cx, ct) =>
            {
                var service = cx.Service<IClientsService>();
                var parent = cx.Parent<Registration>();
                return service.GetClient(parent.ClientId);
            });
        base.Configure(descriptor);
    }
}