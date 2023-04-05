namespace SR.Components.Helpers;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(string email, string subject, string body, string name);
}