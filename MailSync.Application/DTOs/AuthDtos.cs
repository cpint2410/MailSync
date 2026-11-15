namespace MailSync.Application.DTOs;

public record RegisterRequest(string Email, string Password);

public record LoginRequest(string Email, string Password);

public record AuthResponse(Guid Id, string Email, string Token);