namespace Droits.Exceptions;

public class WreckNotFoundException : Exception
{
    public WreckNotFoundException() : base()
    {
    }
    public WreckNotFoundException(string message) : base(message)
    {
    }
}