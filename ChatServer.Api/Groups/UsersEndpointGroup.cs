using FastEndpoints;

namespace ChatServer.Api.Groups;

public sealed class UsersEndpointGroup : Group
{
    public UsersEndpointGroup()
    {
        Configure("/users", _ => {});
    }
}
