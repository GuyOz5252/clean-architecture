using ChatServer.Application.Messaging;

namespace ChatServer.Application.Users.Login;

public record LoginUserCommand : ICommand<LoginResponse>
{
    public string Email { get; init; }
    public string Password { get; init; }
}
