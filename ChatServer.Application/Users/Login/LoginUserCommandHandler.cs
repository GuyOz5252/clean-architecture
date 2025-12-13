using Ardalis.Result;
using ChatServer.Application.Messaging;

namespace ChatServer.Application.Users.Login;

public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand>
{
    public ValueTask<Result> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
