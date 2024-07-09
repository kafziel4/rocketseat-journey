using System.Net;

namespace Journey.Exception.ExceptionsBase;

public class NotFoundException : JourneyException
{
    public NotFoundException()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, System.Exception inner)
        : base(message, inner)
    {
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.NotFound;
    }

    public override IList<string> GetErrorMessages()
    {
        return [Message];
    }
}
