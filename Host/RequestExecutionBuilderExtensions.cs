using Host.Clients;
using Host.Registrations;
using HotChocolate.Execution.Configuration;

namespace Host;

public static class RequestExecutionBuilderExtensions
{
    public static IRequestExecutorBuilder AddRegistartionGraphQL(this IRequestExecutorBuilder builder)
    {
        return builder.AddQueryType<RegistrationQueries>()
            .RegisterDbContext<RegistrationDbContext>()
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .AddType<RegistrationType>()
            .AddDataLoader<ClientsByIdsDataLoader>();
    }
}