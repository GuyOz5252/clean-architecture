namespace ChatServer.Core.Abstract;

public interface ITokenGenerator
{
    string Generate(string userId, string username, string email, List<string> roles);
}
