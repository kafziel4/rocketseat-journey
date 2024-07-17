using Journey.Application.Mappers;
using Journey.Communication.Responses;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Journey.Application.UseCases.Trips.GetById;

public class GetTripByIdUseCase
{
    private readonly JourneyDbContext _dbContext;

    public GetTripByIdUseCase(JourneyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseTripJson> ExecuteAsync(Guid id)
    {
        var trip = await _dbContext
            .Trips
            .Include(t => t.Activities)
            .Where(t => t.Id == id)
            .Select(t => t.ToTripJson())
            .FirstOrDefaultAsync();

        if (trip is null)
        {
            throw new NotFoundException(ResourceErrorMessages.TRIP_NOT_FOUND);
        }

        return trip;
    }
}
