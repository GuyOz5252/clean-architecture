using ChatServer.Application.Messaging;

namespace ChatServer.Application.Users.Create;

public record CreateUserCommand : ICommand<string>
{
    public string Username { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string DisplayName { get; init; }
}
