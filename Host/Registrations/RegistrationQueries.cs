namespace Host.Registrations;

[ExtendObjectType(OperationTypeNames.Query)]
public class RegistrationQueries
{
    public static Registration GetRegistration()
    {
        return new Registration();
    }
    public static IQueryable<Registration> GetRegistrations(RegistrationDbContext context) =>
        context.Set<Registration>();
}