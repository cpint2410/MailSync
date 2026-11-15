using MailSync.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MailSync.Infrastructure.Persistence;

public class MailSyncDbContext : DbContext
{
    public MailSyncDbContext(DbContextOptions<MailSyncDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<EmailAccount> EmailAccounts => Set<EmailAccount>();
    public DbSet<EmailMessage> EmailMessages => Set<EmailMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.HasMany(e => e.EmailAccounts)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // EmailAccount configuration
        modelBuilder.Entity<EmailAccount>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Provider)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.EmailAddress)
                .IsRequired()
                .HasMaxLength(255);
        });

        // EmailMessage configuration
        modelBuilder.Entity<EmailMessage>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.EmailAccount)
                .WithMany()
                .HasForeignKey(e => e.EmailAccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}