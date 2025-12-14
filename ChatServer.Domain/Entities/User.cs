using ChatServer.Domain.Abstract;

namespace ChatServer.Domain.Entities;

public class User : EntityBase
{
    public string Username { get; init; }
    public string Email { get; init; }
}
