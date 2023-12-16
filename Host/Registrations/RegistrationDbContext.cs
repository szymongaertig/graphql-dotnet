using Host.Events;
using Microsoft.EntityFrameworkCore;

namespace Host.Registrations;

public class RegistrationDbContext : DbContext
{
    public DbSet<Registration> Registrations { get; set; }
    public DbSet<Event> Events { get; set; }

    public RegistrationDbContext(DbContextOptions<RegistrationDbContext> options)
        : base(options)
    {
    }
}