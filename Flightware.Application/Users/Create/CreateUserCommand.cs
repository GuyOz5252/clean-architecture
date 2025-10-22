namespace Flightware.Application.Users.Create;

public record CreateUserCommand : ICommand<Guid>
{
    public string Username { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}
