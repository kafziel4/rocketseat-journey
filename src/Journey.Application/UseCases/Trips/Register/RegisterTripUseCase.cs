using FluentValidation;
using Journey.Application.Mappers;
using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;

namespace Journey.Application.UseCases.Trips.Register;

public class RegisterTripUseCase
{
    private readonly JourneyDbContext _dbContext;
    private readonly IValidator<RequestRegisterTripJson> _validator;

    public RegisterTripUseCase(
        JourneyDbContext dbContext,
        IValidator<RequestRegisterTripJson> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task<ResponseShortTripJson> ExecuteAsync(RequestRegisterTripJson request)
    {
        Validate(request);

        var entity = request.ToEntity();

        _dbContext.Trips.Add(entity);
        await _dbContext.SaveChangesAsync();

        return entity.ToShortTripJson();
    }

    private void Validate(RequestRegisterTripJson request)
    {
        var result = _validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result
                .Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
