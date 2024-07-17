using FluentAssertions;
using Journey.Exception.ExceptionsBase;
using System.Net;

namespace Journey.Exception.Tests.ExceptionsBase;

public class ErrorOnValidationExceptionTests
{
    [Fact]
    public void GetStatusCode_ShouldReturnBadRequest()
    {
        // Arrange
        var exception = new ErrorOnValidationException();

        //Act
        var statusCode = exception.GetStatusCode();

        // Assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public void GetErrorMessages_ShouldReturnListOfMessages()
    {
        // Arrange
        var errorMessage = "errorMessage";
        var exception = new ErrorOnValidationException([errorMessage]);

        //Act
        var errorMessages = exception.GetErrorMessages();

        // Assert
        errorMessages.Should().HaveCount(1)
            .And.Contain(errorMessage);
    }
}