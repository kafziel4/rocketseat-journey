using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Journey.Application.UseCases.Activities.Delete;

public class DeleteActivityForTripUseCase
{
    private readonly JourneyDbContext _dbContext;

    public DeleteActivityForTripUseCase(JourneyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(Guid tripId, Guid activityId)
    {
        var activity = await _dbContext
            .Activities
            .FirstOrDefaultAsync(a => a.Id == activityId && a.TripId == tripId);

        if (activity is null)
        {
            throw new NotFoundException(ResourceErrorMessages.ACTIVITY_NOT_FOUND);
        }

        _dbContext.Activities.Remove(activity);
        await _dbContext.SaveChangesAsync();
    }
}
