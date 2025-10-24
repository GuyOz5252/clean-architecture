using Flightware.Application.MaterialTypeOrders.Create;
using FluentValidation;

namespace Flightware.Application.Validators;

public abstract class OrderValidatorBase : AbstractValidator<CreateMaterialTypeOrderCommand>
{
    public string MaterialType { get; init; }
}
