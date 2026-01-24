using Ardalis.Result;
using ChatServer.Application.Messaging;
using ChatServer.Core.Abstract;
using ChatServer.Core.Entities;

namespace ChatServer.Application.Users.Create;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async ValueTask<Result<string>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Username = command.Username,
            Email = command.Email,
            DisplayName = command.DisplayName,
            PasswordHash = _passwordHasher.Hash(command.Password),
            Roles = ["User"]
        };

        var createdUser = await _userRepository.CreateAsync(user, cancellationToken);
        var result = await _unitOfWork.CommitAsync(cancellationToken);
        if (!result.IsSuccess)
        {
            return result;
        }

        return createdUser.Id;
    }
}
