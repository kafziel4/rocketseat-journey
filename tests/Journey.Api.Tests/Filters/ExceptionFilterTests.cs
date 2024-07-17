using FluentAssertions;
using Journey.Api.Filters;
using Journey.Communication.Responses;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Journey.Api.Tests.Filters;

public class ExceptionFilterTests
{
    [Fact]
    public void OnException_ShouldGetStatusCodeAndErrorMessages_WhenExceptionIsJourneyException()
    {
        // Arrange
        var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());

        var errorMessage = "errorMessage";
        JourneyException journeyException = new NotFoundException(errorMessage);

        var context = new ExceptionContext(actionContext, [])
        {
            Exception = journeyException
        };

        var filter = new ExceptionFilter();

        // Act
        filter.OnException(context);

        // Assert
        context.HttpContext.Response.StatusCode.Should().Be((int)journeyException.GetStatusCode());
        context.Result.Should().BeOfType<ObjectResult>()
            .Which.Value.Should().BeOfType<ResponseErrorsJson>()
            .Which.Errors.Should().Contain(errorMessage);
    }

    [Fact]
    public void OnException_ShouldSetGenericStatusCodeAndErrorMessages_WhenExceptionIsNotJourneyException()
    {
        // Arrange
        var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());

        var context = new ExceptionContext(actionContext, [])
        {
            Exception = new System.Exception()
        };

        var filter = new ExceptionFilter();

        // Act
        filter.OnException(context);

        // Assert
        context.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        context.Result.Should().BeOfType<ObjectResult>()
            .Which.Value.Should().BeOfType<ResponseErrorsJson>()
            .Which.Errors.Should().Contain(ResourceErrorMessages.UNKNOWN_ERROR);
    }
}
