using Ardalis.Result;
using Flightware.Application.Messaging;

namespace Flightware.Application.Users.Login;

public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand>
{
    public ValueTask<Result> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
