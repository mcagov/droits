using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;

namespace Droits.Tests.UnitTests.Model.ViewModels;

public class DashboardViewTests
{
    [Fact]
    public void DashboardView_ShouldFilterClosedDroits()
    {
        var droits = new List<DroitView>
        {
            new(new Droit {Status = DroitStatus.Closed}),
            new(new Droit {Status = DroitStatus.Received}),
            new(new Droit {Status = DroitStatus.Received}),
        };

        var originalDroitListView = new DroitListView(droits);
        var letters = new LetterListView();

        var dashboardView = new DashboardView(originalDroitListView, letters);
        
        Assert.Equal(2, dashboardView.Droits.TotalCount);
        Assert.Equal(2, dashboardView.Droits.Items.Count);

        var filteredDroits = dashboardView.Droits.Items.Cast<DroitView>().ToList();
        Assert.All(filteredDroits, d => Assert.NotEqual(DroitStatus.Closed, d.Status));
    }
}