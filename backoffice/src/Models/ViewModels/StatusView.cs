using System;

namespace Droits.Models.ViewModels;

public class StatusView
{
    public Enum Status { get; }


    public StatusView(Enum status)
    {
        Status = status;
    }


    public string GetProgressCssClass(Enum statusToCheck)
    {
        if ( statusToCheck.Equals(Status) )
            return "bg-primary";

        if ( statusToCheck.CompareTo(Status) < 0 )
            return "bg-success";

        return "bg-light";
    }


    public string GetTextCssClass(Enum statusToCheck)
    {
        if ( statusToCheck.CompareTo(Status) > 0 )
            return "text-muted";

        return "";
    }
}