namespace Droits.Exceptions;

public class SalvorNotFoundException : Exception
{
    public SalvorNotFoundException() : base()
    {
    }
    public SalvorNotFoundException(string message) : base(message)
    {
    }
}