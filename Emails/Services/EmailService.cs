using Grpc.Core;

namespace Emails.Services;

public class EmailService : Email.EmailBase
{
    public override async Task<EmailList> GetClientEmails(ClientRequest request, ServerCallContext context)
    {
        var emails = new EmailList()
        {
            Emails =
            {
                new EmailEntry()
                {
                    Body = Guid.NewGuid().ToString(),
                    Subject = Guid.NewGuid().ToString()
                },
                new EmailEntry()
                {
                    Body = Guid.NewGuid().ToString(),
                    Subject = Guid.NewGuid().ToString()
                },
                new EmailEntry()
                {
                    Body = Guid.NewGuid().ToString(),
                    Subject = Guid.NewGuid().ToString()
                }
            }
        };

        return emails;
    }
}