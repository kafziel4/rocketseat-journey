using Journey.Application.Mappers;
using Journey.Communication.Responses;
using Journey.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Journey.Application.UseCases.Trips.GetAll;

public class GetAllTripsUseCase
{
    private readonly JourneyDbContext _dbContext;

    public GetAllTripsUseCase(JourneyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseTripsJson> ExecuteAsync()
    {
        var trips = await _dbContext
            .Trips
            .Select(t => t.ToShortTripJson())
            .ToListAsync();

        return new ResponseTripsJson
        {
            Trips = trips
        };
    }
}
