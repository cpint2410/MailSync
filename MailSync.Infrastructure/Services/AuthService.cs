using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MailSync.Application.DTOs;
using MailSync.Application.Services;
using MailSync.Domain.Entities;
using MailSync.Infrastructure.Persistence;

namespace MailSync.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly MailSyncDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(MailSyncDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        var user = new User
        {
            Email = request.Email
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new AuthResponse(user.Id, user.Email, "placeholder-jwt-token");
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        return new AuthResponse(user.Id, user.Email, "placeholder-jwt-token");
    }
}