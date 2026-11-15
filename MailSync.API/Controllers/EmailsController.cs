using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MailSync.Infrastructure.Persistence;

namespace MailSync.API.Controllers;

[ApiController]
[Route("api/v1/mail")]
public class EmailsController : ControllerBase
{
    private readonly MailSyncDbContext _dbContext;

    public EmailsController(MailSyncDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("messages")]
    public async Task<IActionResult> GetMessages([FromQuery] Guid? accountId)
    {
        var query = _dbContext.EmailMessages.AsQueryable();

        if (accountId.HasValue)
        {
            query = query.Where(m => m.EmailAccountId == accountId.Value);
        }

        var messages = await query
            .OrderByDescending(m => m.ReceivedAt)
            .Take(50)
            .ToListAsync();

        return Ok(messages);
    }
}