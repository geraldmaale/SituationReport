using GreatIdeas.MailServices;
using Serilog;

namespace SR.Components.Helpers;
public class EmailSender : IEmailSender
{
    private readonly IMsGraphService _msGraphService;

    public EmailSender(IMsGraphService msGraphService)
    {
        _msGraphService = msGraphService;
    }

    public async Task<bool> SendEmailAsync(string email, string subject, string body, string name)
    {
        try
        {
            var response = await _msGraphService.SendEmailAsync(
                new EmailModel()
                {
                    To = email,
                    Subject = subject,
                    Body = body,
                    FromName = name
                }
            );

            if (response)
            {
                Log.Information("Email sent to {Email}", email);
                return true;
            }

            Log.Error("Email failed to send to {Email}", email);
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}
