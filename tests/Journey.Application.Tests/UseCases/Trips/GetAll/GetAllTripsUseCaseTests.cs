using FluentAssertions;
using Journey.Application.UseCases.Trips.GetAll;
using Journey.TestHelpers;

namespace Journey.Application.Tests.UseCases.Trips.GetAll;

public class GetAllTripsUseCaseTests : IDisposable
{
    private readonly DbContextTestHelper _dbContextTestHelper;

    public GetAllTripsUseCaseTests()
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
    public async Task Execute_ShouldReturnTrips()
    {
        // Arrange
        using var context = _dbContextTestHelper.CreateContext();
        var useCase = new GetAllTripsUseCase(context);

        var firstTrip = TestDataHelper.Trips[0];

        // Act
        var result = await useCase.ExecuteAsync();

        // Assert
        var firstTripJson = result.Trips[0];

        result.Trips.Should().HaveCount(TestDataHelper.Trips.Count);

        firstTripJson.Id.Should().Be(firstTrip.Id);
        firstTripJson.Name.Should().Be(firstTrip.Name);
        firstTripJson.StartDate.Should().Be(firstTrip.StartDate);
        firstTripJson.EndDate.Should().Be(firstTrip.EndDate);
    }
}