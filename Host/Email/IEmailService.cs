namespace Host.Email;

public interface IEmailService
{
    Task<Emails.EmailList> GetClientEmails(int clientId);
}