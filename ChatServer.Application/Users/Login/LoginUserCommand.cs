using ChatServer.Application.Messaging;

namespace ChatServer.Application.Users.Login;

public record LoginUserCommand : ICommand
{
    public string Username { get; init; }
    public string Password { get; init; }
}
