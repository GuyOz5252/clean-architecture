using Flightware.Domain.Models;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Flightware.Application.MaterialTypeOrders.Create;

public class CreateMaterialTypeOrderCommandValidator : AbstractValidator<CreateMaterialTypeOrderCommand>
{
    public CreateMaterialTypeOrderCommandValidator(IOptionsMonitor<List<MaterialType>> materialTypes)
    {
        RuleFor(command => command)
            .Custom((command, context) =>
            {
                var materialType = materialTypes.CurrentValue
                    .FirstOrDefault(materialType => materialType.Name == command.MaterialType);
                if (materialType is null)
                {
                    context.AddFailure(
                        "MaterialType",
                        $"MaterialType: {command.MaterialType} not found");
                    return;
                }

                var nonExistingParameters = new List<string>();
                command.Parameters.Keys
                    .ToList()
                    .ForEach(parameter =>
                    {
                        var exists = materialType.OrderParameters.Select(orderParameters => orderParameters.Name)
                            .Contains(parameter, StringComparer.OrdinalIgnoreCase);
                        if (!exists)
                        {
                            nonExistingParameters.Add(parameter);
                        }
                    });
                nonExistingParameters.ForEach(parameter =>
                {
                    context.AddFailure(
                        "Parameter",
                        $"Parameter: {parameter} not configured for MaterialType: {command.MaterialType}");
                });
            });
    }
}
