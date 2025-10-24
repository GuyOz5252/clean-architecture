namespace Flightware.Application.MaterialTypeOrders.Create;

public class CreateMaterialTypeOrderCommand : ICommand<Guid>
{
    public string MaterialType { get; init; }
    public Dictionary<string, string> Parameters { get; init; }
}
