using Ardalis.Result;
using Common.Core.Abstract;
using Flightware.Application.Messaging;
using Flightware.Domain.Abstract;
using Flightware.Domain.Entities;

namespace Flightware.Application.Users.Create;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<Result<Guid>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Username = command.Username,
            Email = command.Email
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
