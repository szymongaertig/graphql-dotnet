using Clients;
using Host.Clients;
using Host.Email;

namespace Host.Registrations;

public class RegistrationType : ObjectType<Registration>
{
    protected override void Configure(IObjectTypeDescriptor<Registration> descriptor)
    {
        descriptor
            .Field(x => x.ClientId)
            .IsProjected(true);

        descriptor
            .Field("client")
            .Resolve<Client?>((cx, ct) =>
            {
                var service = cx.Service<IClientsService>();
                var parent = cx.Parent<Registration>();
                return service.GetClient(parent.ClientId, ct);
            });

        descriptor
            .Field("clientWithLoader")
            .Resolve<Client?>((cx, ct) =>
            {
                var loader = cx.DataLoader<ClientsByIdsDataLoader>();
                var parent = cx.Parent<Registration>();
                return loader.LoadAsync(parent.ClientId, ct);
            });

        descriptor
            .Field("emails")
            .Resolve<Emails.EmailEntry[]?>(async (cx, ct) =>
            {
                var service = cx.Service<IEmailService>();
                var parent = cx.Parent<Registration>();
                var result = await service.GetClientEmails(parent.ClientId);
                return result.Emails.ToArray();
            });

        base.Configure(descriptor);
    }
}