using FluentAssertions;
using Journey.Application.UseCases.Activities.Register;
using Journey.Communication.Requests;
using Journey.Exception;

namespace Journey.Application.Tests.UseCases.Activities.Register;

public class RegisterActivityValidatorTests
{
    [Fact]
    public async Task Validate_ShouldBeValid_WhenRequestIsValid()
    {
        // Arrange
        var request = new RequestRegisterActivityJson
        {
            Name = "Test",
            Date = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Utc)
        };

        var validator = new RegisterActivityValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_ShouldBeInvalid_WhenNameIsEmpty()
    {
        // Arrange
        var request = new RequestRegisterActivityJson
        {
            Name = "",
            Date = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Utc)
        };

        var validator = new RegisterActivityValidator();

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage)
            .Should().Contain(ResourceErrorMessages.NAME_EMPTY);
    }
}
