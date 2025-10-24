namespace Flightware.Domain.Models;

public record MaterialType
{
    public string Name { get; init; }
    public List<OrderParameters> OrderParameters { get; init; }
}
