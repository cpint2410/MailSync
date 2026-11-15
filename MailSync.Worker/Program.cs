using Microsoft.EntityFrameworkCore;
using MailSync.Infrastructure.Persistence;
using MailSync.Worker;

var builder = Host.CreateApplicationBuilder(args);

// Register PostgreSQL DbContext so the worker can fetch connected email accounts
builder.Services.AddDbContext<MailSyncDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the Background Worker Service
builder.Services.AddHostedService<EmailSyncWorker>();

var host = builder.Build();
host.Run();