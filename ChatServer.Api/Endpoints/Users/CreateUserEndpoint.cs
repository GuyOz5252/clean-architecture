using Ardalis.Result.AspNetCore;
using ChatServer.Api.Groups;
using FastEndpoints;
using ChatServer.Application.Users.Create;
using Mediator;

namespace ChatServer.Api.Endpoints.Users;

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
            Password = createUserRequest.Password,
            DisplayName = createUserRequest.DisplayName
        };
        var result = await Mediator.Send(command, ct);
        
        if (!result.IsSuccess)
        {
            await Send.ResultAsync(result.ToMinimalApiResult());
            return;
        }
        
        await Send.OkAsync(new CreateUserResponse(result.Value), ct);
    }
}

public record CreateUserRequest(string Username, string Email, string Password, string DisplayName);

public record CreateUserResponse(string Id);
