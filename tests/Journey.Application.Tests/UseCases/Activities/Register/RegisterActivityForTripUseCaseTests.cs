using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Journey.Application.UseCases.Activities.Register;
using Journey.Communication.Requests;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.TestHelpers;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Journey.Application.Tests.UseCases.Activities.Register;

public class RegisterActivityForTripUseCaseTests : IDisposable
{
    private readonly DbContextTestHelper _dbContextTestHelper;

    public RegisterActivityForTripUseCaseTests()
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

    [Theory]
    [InlineData(15)]
    [InlineData(20)]
    [InlineData(25)]
    public async Task Execute_ShouldCreateActivity_WhenActivityIsValid(int day)
    {
        // Arrange
        var request = new RequestRegisterActivityJson
        {
            Name = "Test",
            Date = new DateTime(2024, 8, day, 0, 0, 0, DateTimeKind.Utc),
        };

        var validator = Substitute.For<IValidator<RequestRegisterActivityJson>>();
        validator.Validate(request).Returns(new ValidationResult());

        using var context = _dbContextTestHelper.CreateContext();
        var useCase = new RegisterActivityForTripUseCase(context, validator);

        var tripId = TestDataHelper.Trips[1].Id;

        // Act
        var result = await useCase.ExecuteAsync(tripId, request);

        context.ChangeTracker.Clear();

        var activity = await context
            .Activities
            .FirstOrDefaultAsync(a => a.Id == result.Id && a.TripId == tripId);

        // Assert
        activity.Name.Should().Be(request.Name);
        activity.Date.Should().Be(request.Date);
        activity.Status.Should().Be(Infrastructure.Enums.ActivityStatus.Pending);

        result.Name.Should().Be(request.Name);
        result.Date.Should().Be(request.Date);
        result.Status.Should().Be(Communication.Enums.ActivityStatus.Pending);
    }

    [Theory]
    [InlineData(14)]
    [InlineData(26)]
    public async Task Execute_ShouldThrowErrorOnValidationException_WhenActivityDateIsNotBetweenTripDates(int day)
    {
        // Arrange
        var request = new RequestRegisterActivityJson
        {
            Name = "Test",
            Date = new DateTime(2024, 8, day, 0, 0, 0, DateTimeKind.Utc),
        };

        var validator = Substitute.For<IValidator<RequestRegisterActivityJson>>();
        validator.Validate(request).Returns(new ValidationResult());

        using var context = _dbContextTestHelper.CreateContext();
        var useCase = new RegisterActivityForTripUseCase(context, validator);

        var tripId = TestDataHelper.Trips[1].Id;

        // Act
        var action = async () => await useCase.ExecuteAsync(tripId, request);

        // Assert
        await action.Should().ThrowAsync<ErrorOnValidationException>()
            .Where(e => e.GetErrorMessages().Contains(ResourceErrorMessages.DATE_NOT_WITHIN_TRAVEL_PERIOD));
    }

    [Fact]
    public async Task Execute_ShouldThrowErrorOnValidationException_WhenActivityIsInvalid()
    {
        // Arrange
        var request = new RequestRegisterActivityJson
        {
            Name = "Test",
            Date = new DateTime(2024, 8, 20, 0, 0, 0, DateTimeKind.Utc),
        };

        var errorMessage = "errorMessage";
        var validationResult = new ValidationResult([new ValidationFailure("propertyName", errorMessage)]);

        var validator = Substitute.For<IValidator<RequestRegisterActivityJson>>();
        validator.Validate(request).Returns(validationResult);

        using var context = _dbContextTestHelper.CreateContext();
        var useCase = new RegisterActivityForTripUseCase(context, validator);

        var tripId = TestDataHelper.Trips[1].Id;

        // Act
        var action = async () => await useCase.ExecuteAsync(tripId, request);

        // Assert
        await action.Should().ThrowAsync<ErrorOnValidationException>()
            .Where(e => e.GetErrorMessages().Contains(errorMessage));
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenTripDoesNotExist()
    {
        // Arrange
        var request = new RequestRegisterActivityJson
        {
            Name = "Test",
            Date = new DateTime(2024, 8, 20, 0, 0, 0, DateTimeKind.Utc),
        };

        var validator = Substitute.For<IValidator<RequestRegisterActivityJson>>();
        validator.Validate(request).Returns(new ValidationResult());

        using var context = _dbContextTestHelper.CreateContext();
        var useCase = new RegisterActivityForTripUseCase(context, validator);

        var tripId = Guid.Empty;

        // Act
        var action = async () => await useCase.ExecuteAsync(tripId, request);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage(ResourceErrorMessages.TRIP_NOT_FOUND);
    }
}
