namespace Droits.Exceptions;

public class ImageNotFoundException : Exception
{
    public ImageNotFoundException() : base()
    {
    }
    public ImageNotFoundException(string message) : base(message)
    {
    }
}