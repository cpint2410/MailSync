namespace MailSync.Domain.Entities;

public class EmailMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmailAccountId { get; set; }
    public EmailAccount EmailAccount { get; set; } = null!;

    public string ExternalMessageId { get; set; } = string.Empty; // Message ID from Gmail/Outlook
    public string Subject { get; set; } = string.Empty;
    public string Sender { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public string BodySnippet { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime ReceivedAt { get; set; }
    public DateTime SyncedAt { get; set; } = DateTime.UtcNow;
}