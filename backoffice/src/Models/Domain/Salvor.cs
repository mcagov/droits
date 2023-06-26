namespace Droits.Models;

public class Salvor
{
    public Guid Id { get; set; }
    public String Email { get; set; } = string.Empty;
    public String Name { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
}