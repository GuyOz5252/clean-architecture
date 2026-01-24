namespace ChatServer.Application.Users.Login;

public record LoginResponse(
    string Token,
    string UserId,
    string Username,
    string Email,
    List<string> Roles
);
