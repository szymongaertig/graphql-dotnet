using Clients;
using Refit;

namespace Host.Clients;

public interface IClientsService
{
    [Get("/clients/{clientId}")]
    Task<Client> GetClient(int clientId, CancellationToken cancellationToken);

    [Get("/clients")]
    Task<Client[]> GetClients([AliasAs("clientIds")]int[] clientIds, CancellationToken cancellationToken);
}