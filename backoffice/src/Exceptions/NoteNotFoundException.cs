namespace Droits.Exceptions;

public class NoteNotFoundException : Exception
{
    public NoteNotFoundException() : base()
    {
    }
    public NoteNotFoundException(string message) : base(message)
    {
    }
}