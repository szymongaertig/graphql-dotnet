using Clients;

namespace Dbinit;

public static class ClientFactory
{
    static ClientFactory()
    {
        var names = new List<string>();
        var surnames = new List<string>();
        
        var allFiles = typeof(Program).Assembly.GetManifestResourceNames();
        var sourceFile = typeof(Program).Assembly.GetManifestResourceStream("Dbinit.names.txt");
        var rows = new StreamReader(sourceFile);
        foreach (var line in rows.ReadToEnd().Split(Environment.NewLine))
        {
            var fields = line.Split(' ');
            names.Add(fields[0].Trim());
            surnames.Add(fields[1].Trim());
        }

        _names = names.ToArray();
        _surnames = names.ToArray();
    }

    private static string[] _domains = new[]
    {
        "gmail.com",
        "wp.pl",
        "protonmail.com"
    };

    private static readonly string[] _names;
    private static readonly string[] _surnames;

    public static Client Create()
    {
        var name = _names[Random.Shared.Next(0, _names.Length - 1)];
        var surname = _surnames[Random.Shared.Next(0, _surnames.Length - 1)];
        var domain = _domains[Random.Shared.Next(0, _domains.Length - 1)];
        var email = $"{name}.{surname.Substring(0, surname.Length / 2)}.{Random.Shared.Next(70, 99)}@{domain}";
        var client = new Client()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Surname = surname,
            EmailAddress = email
        };
        return client;
    }
}