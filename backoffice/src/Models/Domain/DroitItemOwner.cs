namespace Droits.Models;

public class DroitItemOwner
{
    private readonly int _ownerResponseWindowDays = 30;
    public Guid Id { get; set; }
    public Guid DroitId { get; set; }
    public Guid DroitItemId { get; set; }

    public DateTime? OwnerContactedDate { get; set; }
    public DateTime? OwnerRespondedDate { get; set; }

    public DateTime? OwnerReceiptsReceivedDate { get; set; }

    public bool OwnerHasResponded => OwnerRespondedDate.HasValue;

    public bool OwnerResponseWindowClosed => OwnerRespondedDate != null && !OwnerHasResponded &&
                                             ( DateTime.UtcNow - OwnerRespondedDate.Value ).Days >
                                             _ownerResponseWindowDays;
}