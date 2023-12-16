using Emails;
using Grpc.Core;
using Grpc.Net.Client;

namespace Host.Email;

public class EmailServiceGrpc : IEmailService
{
    private readonly Emails.Email.EmailClient _emailClient;

    public EmailServiceGrpc(Emails.Email.EmailClient emailClient)
    {
        _emailClient = emailClient;
    }

    public async Task<EmailList> GetClientEmails(int clientId)
    {
        var request = new ClientRequest()
        {
            ClientId = clientId
        };
        try
        {
            var result = await _emailClient.GetClientEmailsAsync(request, new CallOptions());
            return result;
        }
        catch
        {
            throw;
        }
    }
}