using Ardalis.Result;

namespace Flightware.Application.MaterialTypeOrders.Create;

public class CreateMaterialTypeOrderCommandHandler : ICommandHandler<CreateMaterialTypeOrderCommand, Guid>
{
    public async ValueTask<Result<Guid>> Handle(
        CreateMaterialTypeOrderCommand command,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(command.MaterialType);
        await Task.CompletedTask;
        return Guid.NewGuid();
    }
}
