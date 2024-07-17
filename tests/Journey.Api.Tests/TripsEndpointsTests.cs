using FluentAssertions;
using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Journey.Exception;
using Journey.Infrastructure;
using Journey.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace Journey.Api.Tests;

public class TripsEndpointsTests : IClassFixture<ApplicationFactory>
{
    private readonly HttpClient _client;

    public TripsEndpointsTests(ApplicationFactory factory)
    {
        factory.FakeTime.SetUtcNow(new DateTimeOffset(new DateTime(2024, 7, 14, 0, 0, 0, DateTimeKind.Utc)));

        _client = factory.CreateClient();

        using var scope = factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<JourneyDbContext>();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        context.AddRange(TestDataHelper.Trips);
        context.SaveChanges();
    }

    [Fact]
    public async Task Register_ShouldReturnCreated_WhenRequestIsValid()
    {
        // Arrange
        var request = new RequestRegisterTripJson
        {
            Name = "Test",
            StartDate = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 7, 30, 0, 0, 0, DateTimeKind.Utc),
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/trips", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var tripJson = await response.Content.ReadFromJsonAsync<ResponseShortTripJson>();
        tripJson.Id.Should().NotBeEmpty();
        tripJson.Name.Should().Be(request.Name);
        tripJson.StartDate.Should().Be(request.StartDate);
        tripJson.EndDate.Should().Be(request.EndDate);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new RequestRegisterTripJson
        {
            Name = "",
            StartDate = new DateTime(2024, 7, 13, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 7, 12, 0, 0, 0, DateTimeKind.Utc),
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/trips", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorsJson = await response.Content.ReadFromJsonAsync<ResponseErrorsJson>();
        errorsJson.Errors.Should().Contain(ResourceErrorMessages.NAME_EMPTY)
            .And.Contain(ResourceErrorMessages.DATE_TRIP_MUST_BE_LATER_THAN_TODAY)
            .And.Contain(ResourceErrorMessages.END_DATE_TRIP_MUST_BE_LATER_THAN_START_DATE);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkAndAllTrips()
    {
        // Arrange
        var firstTrip = TestDataHelper.Trips[0];

        // Act
        var response = await _client.GetAsync("/api/trips");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tripsJson = await response.Content.ReadFromJsonAsync<ResponseTripsJson>();
        var firstTripJson = tripsJson.Trips[0];

        tripsJson.Trips.Should().HaveCount(TestDataHelper.Trips.Count);

        firstTripJson.Id.Should().Be(firstTrip.Id);
        firstTripJson.Name.Should().Be(firstTrip.Name);
        firstTripJson.StartDate.Should().Be(firstTrip.StartDate);
        firstTripJson.EndDate.Should().Be(firstTrip.EndDate);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkAndTrip_WhenTripExists()
    {
        // Arrange
        var trip = TestDataHelper.Trips[0];
        var firstActivity = trip.Activities[0];

        // Act
        var response = await _client.GetAsync($"/api/trips/{trip.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tripJson = await response.Content.ReadFromJsonAsync<ResponseTripJson>();
        var firstActivityJson = tripJson.Activities[0];

        tripJson.Id.Should().Be(trip.Id);
        tripJson.Name.Should().Be(trip.Name);
        tripJson.StartDate.Should().Be(trip.StartDate);
        tripJson.EndDate.Should().Be(trip.EndDate);
        tripJson.Activities.Should().HaveCount(trip.Activities.Count);

        firstActivityJson.Id.Should().Be(firstActivity.Id);
        firstActivityJson.Name.Should().Be(firstActivity.Name);
        firstActivityJson.Date.Should().Be(firstActivity.Date);
        firstActivityJson.Status.Should().Be((Communication.Enums.ActivityStatus)firstActivity.Status);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenTripDoesNotExist()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var response = await _client.GetAsync($"/api/trips/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorsJson = await response.Content.ReadFromJsonAsync<ResponseErrorsJson>();
        errorsJson.Errors.Should().Contain(ResourceErrorMessages.TRIP_NOT_FOUND);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenTripExists()
    {
        // Arrange
        var id = TestDataHelper.Trips[0].Id;

        // Act
        var response = await _client.DeleteAsync($"/api/trips/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenTripDoesNotExist()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var response = await _client.DeleteAsync($"/api/trips/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorsJson = await response.Content.ReadFromJsonAsync<ResponseErrorsJson>();
        errorsJson.Errors.Should().Contain(ResourceErrorMessages.TRIP_NOT_FOUND);
    }

    [Fact]
    public async Task RegisterActivity_ShouldReturnCreated_WhenTripExistsAndActivityIsValid()
    {
        // Arrange
        var tripId = TestDataHelper.Trips[0].Id;

        var request = new RequestRegisterActivityJson
        {
            Name = "Test",
            Date = new DateTime(2024, 7, 5, 0, 0, 0, DateTimeKind.Utc),
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/trips/{tripId}/activities", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var activityJson = await response.Content.ReadFromJsonAsync<ResponseActivityJson>();
        activityJson.Id.Should().NotBeEmpty();
        activityJson.Name.Should().Be(request.Name);
        activityJson.Date.Should().Be(request.Date);
        activityJson.Status.Should().Be(Communication.Enums.ActivityStatus.Pending);
    }

    [Fact]
    public async Task RegisterActivity_ShouldReturnBadRequest_WhenTripExistsAndActivityIsInvalid()
    {
        // Arrange
        var tripId = TestDataHelper.Trips[0].Id;

        var request = new RequestRegisterActivityJson
        {
            Name = "",
            Date = new DateTime(2024, 8, 5, 0, 0, 0, DateTimeKind.Utc),
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/trips/{tripId}/activities", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorJson = await response.Content.ReadFromJsonAsync<ResponseErrorsJson>();
        errorJson.Errors.Should().Contain(ResourceErrorMessages.NAME_EMPTY)
            .And.Contain(ResourceErrorMessages.DATE_NOT_WITHIN_TRAVEL_PERIOD);
    }

    [Fact]
    public async Task RegisterActivity_ShouldReturnNotFound_WhenTripDoesNotExist()
    {
        // Arrange
        var tripId = Guid.Empty;

        var request = new RequestRegisterActivityJson
        {
            Name = "Test",
            Date = new DateTime(2024, 7, 5, 0, 0, 0, DateTimeKind.Utc),
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/trips/{tripId}/activities", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorJson = await response.Content.ReadFromJsonAsync<ResponseErrorsJson>();
        errorJson.Errors.Should().Contain(ResourceErrorMessages.TRIP_NOT_FOUND);
    }

    [Fact]
    public async Task CompleteActivity_ShouldReturnNoContent_WhenActivityExists()
    {
        // Arrange
        var tripId = TestDataHelper.Trips[0].Id;
        var activityId = TestDataHelper.Trips[0].Activities[0].Id;

        // Act
        var response = await _client.PutAsync($"/api/trips/{tripId}/activities/{activityId}/complete", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task CompleteActivity_ShouldReturnNotFound_WhenActivityDoesNotExist()
    {
        // Arrange
        var tripId = TestDataHelper.Trips[0].Id;
        var activityId = Guid.Empty;

        // Act
        var response = await _client.PutAsync($"/api/trips/{tripId}/activities/{activityId}/complete", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorsJson = await response.Content.ReadFromJsonAsync<ResponseErrorsJson>();
        errorsJson.Errors.Should().Contain(ResourceErrorMessages.ACTIVITY_NOT_FOUND);
    }

    [Fact]
    public async Task DeleteActivity_ShouldReturnNoContent_WhenActivityExists()
    {
        // Arrange
        var tripId = TestDataHelper.Trips[0].Id;
        var activityId = TestDataHelper.Trips[0].Activities[0].Id;

        // Act
        var response = await _client.DeleteAsync($"/api/trips/{tripId}/activities/{activityId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteActivity_ShouldReturnNotFound_WhenActivityDoesNotExist()
    {
        // Arrange
        var tripId = TestDataHelper.Trips[0].Id;
        var activityId = Guid.Empty;

        // Act
        var response = await _client.DeleteAsync($"/api/trips/{tripId}/activities/{activityId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorsJson = await response.Content.ReadFromJsonAsync<ResponseErrorsJson>();
        errorsJson.Errors.Should().Contain(ResourceErrorMessages.ACTIVITY_NOT_FOUND);
    }
}
