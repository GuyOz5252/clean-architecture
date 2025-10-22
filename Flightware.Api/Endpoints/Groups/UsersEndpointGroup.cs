using FastEndpoints;

namespace Flightware.Api.Endpoints.Groups;

public sealed class UsersEndpointGroup : Group
{
    public UsersEndpointGroup()
    {
        Configure("/users", _ => {});
    }
}
