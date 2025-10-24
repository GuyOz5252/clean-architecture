using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using Flightware.Application.MaterialTypeOrders.Create;
using Flightware.Application.Validators;
using Mediator;

namespace Flightware.Application.Pipeline;

public class MaterialTypeOrderValidationPipelineBehavior 
    : IPipelineBehavior<CreateMaterialTypeOrderCommand, Result<Guid>>
{
    private readonly IEnumerable<OrderValidatorBase> _validators;

    public MaterialTypeOrderValidationPipelineBehavior(
        IEnumerable<OrderValidatorBase> validators)
    {
        _validators = validators;
    }

    public async ValueTask<Result<Guid>> Handle(
        CreateMaterialTypeOrderCommand message,
        MessageHandlerDelegate<CreateMaterialTypeOrderCommand, Result<Guid>> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(message, cancellationToken);
        }

        var validationResults = await Task.WhenAll(_validators
            .Where(validator => validator.MaterialType == message.MaterialType)
            .Select(validator => validator.ValidateAsync(message, cancellationToken)));

        if (validationResults.All(validationResult => validationResult.IsValid))
        {
            return await next(message, cancellationToken);
        }

        var validationErrors = validationResults
            .SelectMany(validationResult => validationResult.AsErrors())
            .DistinctBy(validationError => new { validationError.Identifier, validationError.ErrorMessage })
            .ToList();
        
        return Result.Invalid(validationErrors);
    }
}
