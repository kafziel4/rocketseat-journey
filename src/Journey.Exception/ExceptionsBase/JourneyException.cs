using System.Net;

namespace Journey.Exception.ExceptionsBase;

public abstract class JourneyException : System.Exception
{
    protected JourneyException()
    {
    }

    protected JourneyException(string message)
        : base(message)
    {
    }

    protected JourneyException(string message, System.Exception inner)
        : base(message, inner)
    {
    }

    public abstract HttpStatusCode GetStatusCode();
    public abstract IList<string> GetErrorMessages();
}
