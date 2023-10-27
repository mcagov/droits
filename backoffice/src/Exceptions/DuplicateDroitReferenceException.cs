namespace Droits.Exceptions;


public class DuplicateDroitReferenceException : Exception
{
    public DuplicateDroitReferenceException() {
    }


    public DuplicateDroitReferenceException(string message) : base(message)
    {
    }
}
