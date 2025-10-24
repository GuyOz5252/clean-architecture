namespace Flightware.Domain.Models;

public record MaterialType
{
    public string Name { get; init; }
    public List<OrderParameters> OrderParameters { get; init; }
}

public record OrderParameters
{
    public string Name { get; init; }
    public string Type { get; init; }
    public string Validation { get; init; }
}
