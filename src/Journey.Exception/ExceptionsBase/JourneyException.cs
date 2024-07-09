namespace Journey.Exception.ExceptionsBase;

public class JourneyException : System.Exception
{

    public JourneyException()
    {
    }

    public JourneyException(string message)
        : base(message)
    {
    }

    public JourneyException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}
