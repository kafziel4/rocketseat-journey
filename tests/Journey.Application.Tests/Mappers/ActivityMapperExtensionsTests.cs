using FluentAssertions;
using Journey.Application.Mappers;
using Journey.Communication.Requests;
using Journey.Infrastructure.Entities;

namespace Journey.Application.Tests.Mappers;

public class ActivityMapperExtensionsTests
{
    [Fact]
    public void ToEntity_ShouldConvertRequest()
    {
        // Arrange
        var request = new RequestRegisterActivityJson
        {
            Name = "Test",
            Date = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var result = request.ToEntity();

        // Assert
        result.Name.Should().Be(request.Name);
        result.Date.Should().Be(request.Date);
    }

    [Fact]
    public void ToActivityJson_ShouldConvertEntity()
    {
        // Arrange
        var entity = new Activity
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Date = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            Status = Infrastructure.Enums.ActivityStatus.Pending
        };

        // Act
        var result = entity.ToActivityJson();

        // Assert
        result.Id.Should().Be(entity.Id);
        result.Name.Should().Be(entity.Name);
        result.Date.Should().Be(entity.Date);
        result.Status.Should().Be(Communication.Enums.ActivityStatus.Pending);
    }
}
