using Microsoft.EntityFrameworkCore;

namespace Clients;

public class ClientsDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }

    public ClientsDbContext(DbContextOptions<ClientsDbContext> options) : base(options)
    {
    }
}