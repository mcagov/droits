namespace Droits.Models.ViewModels;

public class DroitListView
{
    public DroitListView()
    {
    }

    public DroitListView(List<DroitView> droits, bool includeAssociations = false)
    {
        Droits = droits;
        IncludeAssociations = includeAssociations;
    }


    public List<DroitView> Droits { get; } = new();
    public bool IncludeAssociations { get; }
}