using Clients;

namespace Host.Clients;

public class ClientsByIdsDataLoader : DataLoaderBase<int, Client>
{
    private readonly IClientsService _clientsService;

    public ClientsByIdsDataLoader(IBatchScheduler batchScheduler, IClientsService clientsService,
        DataLoaderOptions? options = null) : base(
        batchScheduler, options)
    {
        _clientsService = clientsService;
    }

    protected override async ValueTask FetchAsync(IReadOnlyList<int> keys, Memory<Result<Client>> results,
        CancellationToken cancellationToken)
    {
        var clients = await _clientsService.GetClients(keys.ToArray(), cancellationToken);
        for (var index = 0; index < keys.Count; index++)
        {
            results.Span[index] = clients[index];
        }
    }
}