using ChatServer.Domain.Abstract;

namespace ChatServer.Domain.Entities;

public class User : EntityBase
{
    public string Username { get; init; }
    public string Email { get; init; }
    public string DisplayName { get; init; }
    public string PasswordHash { get; init; }
    public List<string> Roles { get; init; } = [];
}
