namespace Droits.Exceptions;

public class LetterNotFoundException : Exception
{
    public LetterNotFoundException() : base()
    {
    }
    public LetterNotFoundException(string message) : base(message)
    {
    }
}