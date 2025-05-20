using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

public class DummyEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // For dev: just log or skip sending
        Console.WriteLine($"Pretend email sent to {email}: {subject}");
        return Task.CompletedTask;
    }
}