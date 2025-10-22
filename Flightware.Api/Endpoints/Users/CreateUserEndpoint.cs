using FastEndpoints;
using Flightware.Api.Endpoints.Groups;
using Flightware.Application.Users.Create;
using Mediator;

namespace Flightware.Api.Endpoints.Users;

public class CreateUserEndpoint : Endpoint<CreateUserRequest, CreateUserResponse>
{
    public IMediator Mediator { get; init; }
    
    public override void Configure()
    {
        Post("/");
        Group<UsersEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest createUserRequest, CancellationToken ct)
    {
        var command = new CreateUserCommand
        {
            Username = createUserRequest.Username,
            Email = createUserRequest.Email,
            Password = createUserRequest.Password
        };
        var result = await Mediator.Send(command, ct);
        await Send.OkAsync(new CreateUserResponse(result), ct);
    }
}

public record CreateUserRequest(string Username, string Email, string Password);

public record CreateUserResponse(Guid Id);
