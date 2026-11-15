
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MailSync.Application.Services;
using MailSync.Infrastructure.Services;
using MailSync.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register PostgreSQL DbContext
builder.Services.AddDbContext<MailSyncDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Password Hasher & Auth Service
builder.Services.AddScoped<IPasswordHasher<MailSync.Domain.Entities.User>, PasswordHasher<MailSync.Domain.Entities.User>>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailOAuthService, EmailOAuthService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/core/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Automatically apply pending Entity Framework migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MailSyncDbContext>();
    dbContext.Database.Migrate();
}

app.Run();