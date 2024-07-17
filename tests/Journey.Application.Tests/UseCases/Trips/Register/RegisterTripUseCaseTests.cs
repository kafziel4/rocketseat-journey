using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Journey.Application.UseCases.Trips.Register;
using Journey.Communication.Requests;
using Journey.Exception.ExceptionsBase;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Journey.Application.Tests.UseCases.Trips.Register;

public class RegisterTripUseCaseTests : IDisposable
{
    private readonly DbContextTestHelper _dbContextTestHelper;

    public RegisterTripUseCaseTests()
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
    public async Task Execute_ShouldCreateTrip_WhenTripIsValid()
    {
        // Arrange
        var request = new RequestRegisterTripJson
        {
            Name = "Test",
            StartDate = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 7, 30, 0, 0, 0, DateTimeKind.Utc)
        };

        var validator = Substitute.For<IValidator<RequestRegisterTripJson>>();
        validator.Validate(request).Returns(new ValidationResult());

        using var context = _dbContextTestHelper.CreateContext();
        var useCase = new RegisterTripUseCase(context, validator);

        // Act
        var result = await useCase.ExecuteAsync(request);

        context.ChangeTracker.Clear();

        var trip = await context
            .Trips
            .FirstOrDefaultAsync(t => t.Id == result.Id);

        // Assert
        trip.Name.Should().Be(request.Name);
        trip.StartDate.Should().Be(request.StartDate);
        trip.EndDate.Should().Be(request.EndDate);

        result.Name.Should().Be(request.Name);
        result.StartDate.Should().Be(request.StartDate);
        result.EndDate.Should().Be(request.EndDate);
    }

    [Fact]
    public async Task Execute_ShouldThrowErrorOnValidationException_WhenTripIsInvalid()
    {
        // Arrange
        var request = new RequestRegisterTripJson
        {
            Name = "Test",
            StartDate = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 7, 30, 0, 0, 0, DateTimeKind.Utc)
        };

        var errorMessage = "errorMessage";
        var validationResult = new ValidationResult([new ValidationFailure("propertyName", errorMessage)]);

        var validator = Substitute.For<IValidator<RequestRegisterTripJson>>();
        validator.Validate(request).Returns(validationResult);

        using var context = _dbContextTestHelper.CreateContext();
        var useCase = new RegisterTripUseCase(context, validator);

        // Act
        var action = async () => await useCase.ExecuteAsync(request);

        // Assert
        await action.Should().ThrowAsync<ErrorOnValidationException>()
            .Where(e => e.GetErrorMessages().Contains(errorMessage));
    }
}
