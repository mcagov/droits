namespace Droits.Models
{
      public class DroitItemOwner {
        public Guid Id{get;set;}
        public Guid DroitId{get;set;}
        public Guid DroitItemId{get;set;}

        public DateTime? OwnerContactedDate{get;set;}
        public DateTime? OwnerRespondedDate{get;set;}

        public DateTime? OwnerReceiptsReceivedDate{get;set;}

        public bool OwnerHasResponded => OwnerRespondedDate.HasValue;

        private int OwnerResponseWindowDays = 30;
        public bool OwnerResponseWindowClosed => OwnerRespondedDate != null && !OwnerHasResponded && ((DateTime.UtcNow - OwnerRespondedDate.Value).Days > OwnerResponseWindowDays);

    }

}
