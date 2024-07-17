using FluentAssertions;
using Journey.Exception.ExceptionsBase;
using System.Net;

namespace Journey.Exception.Tests.ExceptionsBase;

public class NotFoundExceptionTests
{
    [Fact]
    public void GetStatusCode_ShouldReturnNotFound()
    {
        // Arrange
        var exception = new NotFoundException();

        //Act
        var statusCode = exception.GetStatusCode();

        // Assert
        statusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public void GetErrorMessages_ShouldReturnListOfMessages()
    {
        // Arrange
        var errorMessage = "errorMessage";
        var exception = new NotFoundException(errorMessage);

        //Act
        var errorMessages = exception.GetErrorMessages();

        // Assert
        errorMessages.Should().HaveCount(1)
            .And.Contain(errorMessage);
    }
}