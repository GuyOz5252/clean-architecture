namespace Flightware.Domain.Models;

public record OrderParameters
{
    public string Name { get; init; }
    public string Type { get; init; }
    public string Validation { get; init; }
}
