namespace Droits.Models;

public class SalvorForm
{
    public SalvorForm()
    {
        
    }

    public SalvorForm(Salvor salvor)
    {
        Id = salvor.Id;
    }

    public Guid Id { get; set; }

    public Salvor ApplyChanges(Salvor salvor)
    {
        salvor.Id = Id;

        return salvor;
    }
}