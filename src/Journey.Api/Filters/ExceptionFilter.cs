﻿using Journey.Communication.Responses;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Journey.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is JourneyException journeyException)
        {
            context.HttpContext.Response.StatusCode = (int)journeyException.GetStatusCode();

            var responseJson = new ResponseErrorsJson
            {
                Errors = journeyException.GetErrorMessages()
            };

            context.Result = new ObjectResult(responseJson);
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var responseJson = new ResponseErrorsJson
            {
                Errors = [ResourceErrorMessages.UNKNOWN_ERROR]
            };

            context.Result = new ObjectResult(responseJson);
        }
    }
}
