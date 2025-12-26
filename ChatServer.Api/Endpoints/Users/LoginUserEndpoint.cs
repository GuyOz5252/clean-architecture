using Ardalis.Result.AspNetCore;
using ChatServer.Api.Groups;
using ChatServer.Application.Users.Login;
using FastEndpoints;
using Mediator;

namespace ChatServer.Api.Endpoints.Users;

public class LoginUserEndpoint : Endpoint<LoginUserRequest, LoginUserResponse>
{
    public IMediator Mediator { get; init; }
    
    public override void Configure()
    {
        Post("/login");
        Group<UsersEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginUserRequest request, CancellationToken ct)
    {
        var command = new LoginUserCommand
        {
            Email = request.Email,
            Password = request.Password
        };

        var result = await Mediator.Send(command, ct);
        
        if (!result.IsSuccess)
        {
            await Send.ResultAsync(result.ToMinimalApiResult());
            return;
        }

        var response = new LoginUserResponse(
            result.Value.Token,
            result.Value.UserId,
            result.Value.Username,
            result.Value.Email,
            result.Value.Roles
        );

        await Send.OkAsync(response, ct);
    }
}

public record LoginUserRequest(string Email, string Password);

public record LoginUserResponse(
    string Token,
    Guid UserId,
    string Username,
    string Email,
    List<string> Roles
);
