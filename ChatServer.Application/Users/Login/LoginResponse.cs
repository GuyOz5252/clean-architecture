namespace ChatServer.Application.Users.Login;

public record LoginResponse(
    string Token,
    Guid UserId,
    string Username,
    string Email,
    List<string> Roles
);
