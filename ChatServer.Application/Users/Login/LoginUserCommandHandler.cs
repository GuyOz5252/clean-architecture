using Ardalis.Result;
using ChatServer.Application.Messaging;
using ChatServer.Core.Abstract;

namespace ChatServer.Application.Users.Login;

public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async ValueTask<Result<LoginResponse>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (!userResult.IsSuccess)
        {
            return Result.NotFound(userResult.Errors.ToArray());
        }

        var user = userResult.Value;
        
        if (!_passwordHasher.Verify(command.Password, user.PasswordHash))
        {
            return Result.Unauthorized();
        }
        
        var token = _tokenGenerator.Generate(user.Id, user.Username, user.Email, user.Roles);

        var response = new LoginResponse(
            token,
            user.Id,
            user.Username,
            user.Email,
            user.Roles
        );

        return Result.Success(response);
    }
}
