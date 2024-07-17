using FluentAssertions;
using Journey.Application.UseCases.Trips.Register;
using Journey.Communication.Requests;
using Journey.Exception;
using Microsoft.Extensions.Time.Testing;

namespace Journey.Application.Tests.UseCases.Trips.Register;

public class RegisterTripValidatorTests
{
    private readonly FakeTimeProvider _fakeTime;

    public RegisterTripValidatorTests()
    {
        _fakeTime = new FakeTimeProvider();
        _fakeTime.SetUtcNow(new DateTimeOffset(new DateTime(2024, 7, 14, 0, 0, 0, DateTimeKind.Utc)));
    }

    [Theory]
    [InlineData(14, 14)]
    [InlineData(14, 15)]
    [InlineData(15, 15)]
    [InlineData(15, 16)]
    public async Task Validate_ShouldBeValid_WhenRequestIsValid(int startDay, int endDay)
    {
        // Arrange
        var request = new RequestRegisterTripJson
        {
            Name = "Test",
            StartDate = new DateTime(2024, 7, startDay, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 7, endDay, 0, 0, 0, DateTimeKind.Utc)
        };

        var validator = new RegisterTripValidator(_fakeTime);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_ShouldBeInvalid_WhenNameIsEmpty()
    {
        // Arrange
        var request = new RequestRegisterTripJson
        {
            Name = "",
            StartDate = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 7, 30, 0, 0, 0, DateTimeKind.Utc)
        };

        var validator = new RegisterTripValidator(_fakeTime);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage)
            .Should().Contain(ResourceErrorMessages.NAME_EMPTY);
    }

    [Fact]
    public async Task Validate_ShouldBeInvalid_WhenStartDateIsBeforeUtcNow()
    {
        // Arrange
        var request = new RequestRegisterTripJson
        {
            Name = "Test",
            StartDate = new DateTime(2024, 7, 13, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 7, 30, 0, 0, 0, DateTimeKind.Utc)
        };

        var validator = new RegisterTripValidator(_fakeTime);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage)
            .Should().Contain(ResourceErrorMessages.DATE_TRIP_MUST_BE_LATER_THAN_TODAY);
    }

    [Fact]
    public async Task Validate_ShouldBeInvalid_WhenEndDateIsBeforeStartDate()
    {
        // Arrange
        var request = new RequestRegisterTripJson
        {
            Name = "Test",
            StartDate = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 7, 14, 0, 0, 0, DateTimeKind.Utc)
        };

        var validator = new RegisterTripValidator(_fakeTime);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage)
            .Should().Contain(ResourceErrorMessages.END_DATE_TRIP_MUST_BE_LATER_THAN_START_DATE);
    }
}
