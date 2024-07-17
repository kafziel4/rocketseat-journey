using FluentValidation;
using FluentValidation.Results;
using Journey.Application.Mappers;
using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;
using Journey.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Journey.Application.UseCases.Activities.Register;

public class RegisterActivityForTripUseCase
{
    private readonly JourneyDbContext _dbContext;
    private readonly IValidator<RequestRegisterActivityJson> _validator;

    public RegisterActivityForTripUseCase(
        JourneyDbContext dbContext,
        IValidator<RequestRegisterActivityJson> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task<ResponseActivityJson> ExecuteAsync(Guid tripId, RequestRegisterActivityJson request)
    {
        var trip = await _dbContext
            .Trips
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip is null)
        {
            throw new NotFoundException(ResourceErrorMessages.TRIP_NOT_FOUND);
        }

        Validate(trip, request);

        var entity = request.ToEntity();
        entity.TripId = tripId;

        _dbContext.Activities.Add(entity);
        await _dbContext.SaveChangesAsync();

        return entity.ToActivityJson();
    }

    private void Validate(Trip trip, RequestRegisterActivityJson request)
    {
        var result = _validator.Validate(request);

        if (request.Date < trip.StartDate || request.Date > trip.EndDate)
        {
            result.Errors.Add(
                new ValidationFailure(nameof(request.Date), ResourceErrorMessages.DATE_NOT_WITHIN_TRAVEL_PERIOD));
        }

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
