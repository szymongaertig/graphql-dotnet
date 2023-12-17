using Host.Clients;
using Refit;

namespace Host;

public static class ServiceCollectionExtensions
{
    public static void AddEmailServiceGrpcClient(this IServiceCollection services)
    {
        services
            .AddGrpcClient<Emails.Email.EmailClient>(o =>
            {
                o.Address = new Uri("http://localhost:5002");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            });
    }

    public static void AddClientServiceHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<IClientsService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5001");
            })
            .AddTypedClient(client => RestService.For<IClientsService>(client));
    }
}