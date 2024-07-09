using System.Net;

namespace Journey.Exception.ExceptionsBase;

public class ErrorOnValidationException : JourneyException
{
    private readonly IList<string> _errors = [];

    public ErrorOnValidationException()
    {
    }

    public ErrorOnValidationException(string message)
        : base(message)
    {
    }

    public ErrorOnValidationException(string message, System.Exception inner)
        : base(message, inner)
    {
    }

    public ErrorOnValidationException(IList<string> messages)
    {
        _errors = messages;
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.BadRequest;
    }

    public override IList<string> GetErrorMessages()
    {
        return _errors;
    }
}
