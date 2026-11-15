namespace MailSync.Application.Services;

public interface IEmailOAuthService
{
    string GetAuthorizationUrl(string provider);
    Task HandleCallbackAsync(Guid userId, string provider, string code);
}