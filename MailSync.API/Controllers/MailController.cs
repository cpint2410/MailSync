using Microsoft.AspNetCore.Mvc;
using MailSync.Application.Services;

namespace MailSync.API.Controllers;

[ApiController]
[Route("api/v1/mail")]
public class MailController : ControllerBase
{
    private readonly IEmailOAuthService _oauthService;

    public MailController(IEmailOAuthService oauthService)
    {
        _oauthService = oauthService;
    }

    [HttpGet("oauth/{provider}/connect")]
    public IActionResult Connect(string provider)
    {
        try
        {
            var authUrl = _oauthService.GetAuthorizationUrl(provider);
            return Ok(new { authorizationUrl = authUrl });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("oauth/{provider}/callback")]
    public async Task<IActionResult> Callback(string provider, [FromQuery] string code, [FromQuery] Guid userId)
    {
        if (string.IsNullOrEmpty(code))
        {
            return BadRequest(new { message = "Authorization code is missing." });
        }

        await _oauthService.HandleCallbackAsync(userId, provider, code);
        return Ok(new { message = $"{provider} account connected successfully!" });
    }
}