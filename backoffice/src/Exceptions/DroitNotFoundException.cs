namespace Droits.Exceptions;

public class DroitNotFoundException : Exception
{
    public DroitNotFoundException() : base()
    {
    }
    public DroitNotFoundException(string message) : base(message)
    {
    }
}