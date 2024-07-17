using FluentAssertions;
using Journey.Application.UseCases.Trips.Delete;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.TestHelpers;
using Microsoft.EntityFrameworkCore;

namespace Journey.Application.Tests.UseCases.Trips.Delete;

public class DeleteTripByIdUseCaseTests : IDisposable
{
    private readonly DbContextTestHelper _dbContextTestHelper;

    public DeleteTripByIdUseCaseTests()
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
    public async Task Execute_ShouldDeleteTrip_WhenTripExists()
    {
        // Arrange
        using var context = _dbContextTestHelper.CreateContext();
        var useCase = new DeleteTripByIdUseCase(context);

        var id = TestDataHelper.Trips[1].Id;

        // Act
        await useCase.ExecuteAsync(id);

        context.ChangeTracker.Clear();

        var trip = await context
            .Trips
            .FirstOrDefaultAsync(t => t.Id == id);

        // Assert
        trip.Should().BeNull();
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenTripDoesNotExist()
    {
        // Arrange
        using var context = _dbContextTestHelper.CreateContext();
        var useCase = new DeleteTripByIdUseCase(context);

        var id = Guid.Empty;

        // Act
        var action = async () => await useCase.ExecuteAsync(id);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage(ResourceErrorMessages.TRIP_NOT_FOUND);
    }
}
