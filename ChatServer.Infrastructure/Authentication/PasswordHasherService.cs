using ChatServer.Core.Abstract;
using ChatServer.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace ChatServer.Infrastructure.Authentication;

public class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<User> _passwordHasher;

    public PasswordHasherService()
    {
        _passwordHasher = new PasswordHasher<User>();
    }

    public string Hash(string password)
    {
        return _passwordHasher.HashPassword(null!, password);
    }

    public bool Verify(string password, string hash)
    {
        var result = _passwordHasher.VerifyHashedPassword(null!, hash, password);
        return result == PasswordVerificationResult.Success;
    }
}
