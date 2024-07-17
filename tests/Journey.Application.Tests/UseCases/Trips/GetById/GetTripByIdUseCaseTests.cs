using FluentAssertions;
using Journey.Application.UseCases.Trips.GetById;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.TestHelpers;

namespace Journey.Application.Tests.UseCases.Trips.GetById;

public class GetTripByIdUseCaseTests : IDisposable
{
    private readonly DbContextTestHelper _dbContextTestHelper;

    public GetTripByIdUseCaseTests()
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
    public async Task Execute_ShouldReturnTripWithActivities_WhenTripExists()
    {
        // Arrange
        using var context = _dbContextTestHelper.CreateContext();
        var useCase = new GetTripByIdUseCase(context);

        var trip = TestDataHelper.Trips[0];
        var firstActivity = trip.Activities[0];

        // Act
        var result = await useCase.ExecuteAsync(trip.Id);

        // Assert
        var firstActivityJson = result.Activities[0];

        result.Id.Should().Be(trip.Id);
        result.Name.Should().Be(trip.Name);
        result.StartDate.Should().Be(trip.StartDate);
        result.EndDate.Should().Be(trip.EndDate);
        result.Activities.Should().HaveCount(trip.Activities.Count);

        firstActivityJson.Id.Should().Be(firstActivity.Id);
        firstActivityJson.Name.Should().Be(firstActivity.Name);
        firstActivityJson.Date.Should().Be(firstActivity.Date);
        firstActivityJson.Status.Should().Be((Communication.Enums.ActivityStatus)firstActivity.Status);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenTripDoesNotExist()
    {
        // Arrange
        using var context = _dbContextTestHelper.CreateContext();
        var useCase = new GetTripByIdUseCase(context);

        var id = Guid.Empty;

        // Act
        var action = async () => await useCase.ExecuteAsync(id);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage(ResourceErrorMessages.TRIP_NOT_FOUND);
    }
}