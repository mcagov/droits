namespace Droits.Models.ViewModels.ListViews;

public class UserListView : ListView<object>
{
    public UserListView()
    {
    }

    public UserListView(IList<UserView> users)
    {
        Items = users.Cast<object>().ToList();
    }
}