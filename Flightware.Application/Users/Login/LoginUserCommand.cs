using Mediator;
using ICommand = Flightware.Application.Messaging.ICommand;

namespace Flightware.Application.Users.Login;

public record LoginUserCommand : ICommand
{
    public string Username { get; init; }
    public string Password { get; init; }
}
