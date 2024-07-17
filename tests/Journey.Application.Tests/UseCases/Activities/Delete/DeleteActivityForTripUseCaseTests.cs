using FluentAssertions;
using Journey.Application.UseCases.Activities.Delete;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.TestHelpers;
using Microsoft.EntityFrameworkCore;

namespace Journey.Application.Tests.UseCases.Activities.Delete;

public class DeleteActivityForTripUseCaseTests : IDisposable
{
    private readonly DbContextTestHelper _dbContextTestHelper;

    public DeleteActivityForTripUseCaseTests()
    {
        _dbContextTestHelper = new DbContextTestHelper();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        _dbContextTestHelper.Dispose();
    }

    [Fact]
    public async Task Execute_ShouldDeleteActivity_WhenActivityExists()
    {
        // Arrange
        using var context = _dbContextTestHelper.CreateContext();
        var useCase = new DeleteActivityForTripUseCase(context);

        var tripId = TestDataHelper.Trips[0].Id;
        var activityId = TestDataHelper.Trips[0].Activities[0].Id;

        // Act
        await useCase.ExecuteAsync(tripId, activityId);

        context.ChangeTracker.Clear();

        var activity = await context
            .Activities
            .FirstOrDefaultAsync(a => a.Id == activityId);

        // Assert
        activity.Should().BeNull();
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenActivityDoesNotExist()
    {
        // Arrange
        using var context = _dbContextTestHelper.CreateContext();
        var useCase = new DeleteActivityForTripUseCase(context);

        var tripId = TestDataHelper.Trips[0].Id;
        var activityId = Guid.Empty;

        // Act
        var action = async () => await useCase.ExecuteAsync(tripId, activityId);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage(ResourceErrorMessages.ACTIVITY_NOT_FOUND);
    }
}
