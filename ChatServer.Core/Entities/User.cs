namespace ChatServer.Core.Entities;

public class User
{
    public string Id { get; init; }
    public string Username { get; init; }
    public string Email { get; init; }
    public string DisplayName { get; init; }
    public string PasswordHash { get; init; }
    public List<string> Roles { get; init; } = [];
}
