using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;
using Journey.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace Journey.Application.UseCases.Activities.Complete;

public class CompleteActivityForTripUseCase
{
    private readonly JourneyDbContext _dbContext;

    public CompleteActivityForTripUseCase(JourneyDbContext dbContext)
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

        activity.Status = ActivityStatus.Done;

        await _dbContext.SaveChangesAsync();
    }
}
