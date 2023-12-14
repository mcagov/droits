namespace Droits.Exceptions;

public class WreckMaterialNotFoundException : Exception
{
    public WreckMaterialNotFoundException() : base()
    {
    }
    public WreckMaterialNotFoundException(string message) : base(message)
    {
    }
}