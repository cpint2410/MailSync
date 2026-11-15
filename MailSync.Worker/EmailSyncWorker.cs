using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MailSync.Infrastructure.Persistence;
using MailSync.Domain.Entities;

namespace MailSync.Worker;

public class EmailSyncWorker : BackgroundService
{
    private readonly ILogger<EmailSyncWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _syncInterval = TimeSpan.FromSeconds(30); // Fast polling for demo

    public EmailSyncWorker(ILogger<EmailSyncWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MailSync Fast-Track Background Worker started at: {time}", DateTimeOffset.UtcNow);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Executing rapid email sync cycle at: {time}", DateTimeOffset.UtcNow);

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<MailSyncDbContext>();
                    var accounts = await dbContext.EmailAccounts.ToListAsync(stoppingToken);

                    _logger.LogInformation("Found {count} connected email account(s) to sync.", accounts.Count);

                    foreach (var account in accounts)
                    {
                        _logger.LogInformation("Syncing and ingesting messages for: {email} ({provider})", account.EmailAddress, account.Provider);

                        // Simulate fetching a new incoming email message for portfolio demonstration
                        var dummyMessage = new EmailMessage
                        {
                            Id = Guid.NewGuid(),
                            EmailAccountId = account.Id,
                            ExternalMessageId = $"msg-{Guid.NewGuid().ToString()[..8]}",
                            Subject = $"Sync update from {account.Provider} at {DateTime.UtcNow:HH:mm:ss}",
                            Sender = $"notifications@{account.Provider.ToLower()}.com",
                            Recipient = account.EmailAddress,
                            BodySnippet = "This is an automated sync test message processed successfully by MailSync Worker.",
                            IsRead = false,
                            ReceivedAt = DateTime.UtcNow,
                            SyncedAt = DateTime.UtcNow
                        };

                        dbContext.EmailMessages.Add(dummyMessage);
                        await dbContext.SaveChangesAsync(stoppingToken);
                        
                        _logger.LogInformation("Successfully ingested message '{subject}' for account {email}", dummyMessage.Subject, account.EmailAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during rapid email synchronization.");
            }

            await Task.Delay(_syncInterval, stoppingToken);
        }
    }
}