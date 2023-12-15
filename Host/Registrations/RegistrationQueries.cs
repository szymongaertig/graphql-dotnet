namespace Host.Registrations;

public class RegistrationQueries
{
    [UsePaging(typeof(Registration))]
    [UseProjection]
    [UseFiltering<Registration>()]
    [UseSorting(typeof(Registration))]
    public IQueryable<Registration> GetRegistrations(RegistrationDbContext context) =>
        context.Set<Registration>();
}