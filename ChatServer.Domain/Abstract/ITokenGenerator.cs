namespace ChatServer.Domain.Abstract;

public interface ITokenGenerator
{
    string Generate(Guid userId, string username, string email, List<string> roles);
}
