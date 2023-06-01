namespace Droits.Models
{
    public class DroitItem {
        public Guid Id{get;set;}
        public Guid DroitId{get;set;}
        public Guid SalvorId{get;set;}
        public Guid? WreckId{get;set;}

        public string Name {get;set;} = string.Empty;
        public string Description{get;set;} = string.Empty;
        public int Quantity {get;set;} = 1;
        public float? Value {get;set;} = 0;
        public float? ReceiverValuation {get;set;} = 0;
        public bool ValueConfirmed {get;set;} = false;
        public string[] Images {get;set;} = new string[0];
        // public string StorageAddress {get;set;} = string.Empty;
        public DateTime Created{get;set;}
        public DateTime Modified{get;set;}

        // Legacy fields ..

        public string WreckMaterialOwner {get;set;} = string.Empty;
        public string Purchaser {get;set;} = string.Empty;
        public string Outcome {get;set;} = string.Empty;
        public string WhereSecured {get;set;} = string.Empty;
        public bool ImportedFromLegacy {get;set;} = false;
    }

}
