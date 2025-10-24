using Flightware.Domain.Models;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Flightware.Application.MaterialTypeOrders.Create;

public class CreateMaterialTypeCommandValidator : AbstractValidator<CreateMaterialTypeOrderCommand>
{
    public CreateMaterialTypeCommandValidator(IServiceProvider serviceProvider)
    {
        RuleFor(command => command)
            .Custom((command, context) =>
            {
                var materialType = serviceProvider.GetKeyedService<MaterialType>(command.MaterialType);
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
                        var found = materialType.OrderParameters.Select(orderParameters => orderParameters.Name)
                            .Contains(parameter, StringComparer.OrdinalIgnoreCase);
                        if (!found)
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
