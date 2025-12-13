using FastEndpoints;

namespace Flightware.Api.Groups;

public sealed class UsersEndpointGroup : Group
{
    public UsersEndpointGroup()
    {
        Configure("/users", _ => {});
    }
}
