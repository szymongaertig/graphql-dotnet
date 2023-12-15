using Clients;
using Refit;

namespace Host.Clients;

public interface IClientsService
{
    [Get("/clients/{clientId}")]
    Task<Client> GetClient(Guid clientId);
}