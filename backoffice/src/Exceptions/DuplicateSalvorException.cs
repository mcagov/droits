namespace Droits.Exceptions;

public class DuplicateSalvorException : Exception
{
    public DuplicateSalvorException() : base()
    {
    }
    public DuplicateSalvorException(string message) : base(message)
    {
    }
}