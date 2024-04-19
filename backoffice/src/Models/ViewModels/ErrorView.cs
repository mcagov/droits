namespace Droits.Models.ViewModels;

public class ErrorView
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string? Error { get; set; }

}