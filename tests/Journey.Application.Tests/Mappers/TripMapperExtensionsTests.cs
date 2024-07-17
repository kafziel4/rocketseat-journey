using FluentAssertions;
using Journey.Application.Mappers;
using Journey.Communication.Requests;
using Journey.Infrastructure.Entities;

namespace Journey.Application.Tests.Mappers;

public class TripMapperExtensionsTests
{
    [Fact]
    public void ToEntity_ShouldConvertRequest()
    {
        // Arrange
        var request = new RequestRegisterTripJson
        {
            Name = "Test",
            StartDate = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 7, 30, 0, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var result = request.ToEntity();

        // Assert
        result.Name.Should().Be(request.Name);
        result.StartDate.Should().Be(request.StartDate);
        result.EndDate.Should().Be(request.EndDate);
    }

    [Fact]
    public void ToShortTripJson_ShouldConvertEntity()
    {
        // Arrange
        var entity = new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            StartDate = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 7, 30, 0, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var result = entity.ToShortTripJson();

        // Assert
        result.Id.Should().Be(entity.Id);
        result.Name.Should().Be(entity.Name);
        result.StartDate.Should().Be(entity.StartDate);
        result.EndDate.Should().Be(entity.EndDate);
    }

    [Fact]
    public void ToTripJson_ShouldConvertEntity()
    {
        // Arrange
        var activity = new Activity
        {
            Id = Guid.NewGuid(),
            Name = "Test Activity",
            Date = new DateTime(2024, 7, 20, 0, 0, 0, DateTimeKind.Utc),
            Status = Infrastructure.Enums.ActivityStatus.Pending
        };

        var entity = new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Test Trip",
            StartDate = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 7, 30, 0, 0, 0, DateTimeKind.Utc),
            Activities = [activity]
        };



        // Act
        var result = entity.ToTripJson();
        var resultActivity = result.Activities[0];

        // Assert
        result.Id.Should().Be(entity.Id);
        result.Name.Should().Be(entity.Name);
        result.StartDate.Should().Be(entity.StartDate);
        result.EndDate.Should().Be(entity.EndDate);
        result.Activities.Should().HaveCount(entity.Activities.Count);

        resultActivity.Id.Should().Be(activity.Id);
        resultActivity.Name.Should().Be(activity.Name);
        resultActivity.Date.Should().Be(activity.Date);
        resultActivity.Status.Should().Be(Communication.Enums.ActivityStatus.Pending);

    }
}