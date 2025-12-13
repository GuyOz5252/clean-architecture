using FluentValidation;

namespace ChatServer.Application.Users.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.Username)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(12);
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(command => command.Password)
            .NotEmpty()
            .MinimumLength(4);
    }
}
