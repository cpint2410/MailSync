using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MailSync.Application.Services;
using MailSync.Domain.Entities;
using MailSync.Infrastructure.Persistence;

namespace MailSync.Infrastructure.Services;

public class EmailOAuthService : IEmailOAuthService
{
    private readonly MailSyncDbContext _context;
    private readonly IConfiguration _configuration;

    public EmailOAuthService(MailSyncDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public string GetAuthorizationUrl(string provider)
    {
        if (provider.Equals("Gmail", StringComparison.OrdinalIgnoreCase))
        {
            var clientId = _configuration["OAuth:Google:ClientId"] ?? "mock-google-client-id";
            var redirectUri = _configuration["OAuth:Google:RedirectUri"] ?? "http://localhost:5179/api/v1/mail/oauth/google/callback";
            return $"https://accounts.google.com/o/oauth2/v2/auth?client_id={clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&response_type=code&scope=https://www.googleapis.com/auth/gmail.readonly";
        }
        else if (provider.Equals("Outlook", StringComparison.OrdinalIgnoreCase))
        {
            var clientId = _configuration["OAuth:Microsoft:ClientId"] ?? "mock-microsoft-client-id";
            var redirectUri = _configuration["OAuth:Microsoft:RedirectUri"] ?? "http://localhost:5179/api/v1/mail/oauth/outlook/callback";
            return $"https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id={clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&response_type=code&scope=Mail.Read offline_access";
        }

        throw new ArgumentException("Unsupported email provider.");
    }

    public async Task HandleCallbackAsync(Guid userId, string provider, string code)
    {
        // In a production app, here is where you exchange the authorization 'code' 
        // for access and refresh tokens via HttpClient POST to Google/Microsoft token endpoints.
        // For our architecture setup, we'll store the verified tokens securely in the database.

        var emailAccount = new EmailAccount
        {
            UserId = userId,
            Provider = provider,
            EmailAddress = $"{provider.ToLowerInvariant()}_user_{Guid.NewGuid().ToString()[..6]}@mailsync.com",
            AccessToken = $"mock_access_token_for_{code}",
            RefreshToken = $"mock_refresh_token_for_{code}",
            TokenExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        _context.EmailAccounts.Add(emailAccount);
        await _context.SaveChangesAsync();
    }
}