using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

namespace Droits.Models.ViewModels
{
    public class WreckMaterialView
    {
        public WreckMaterialView()
        {
        }

        public WreckMaterialView(WreckMaterial wreckMaterial)
        {
            Id = wreckMaterial.Id;
            DroitId = wreckMaterial.DroitId;
            Name = wreckMaterial.Name;
            Description = wreckMaterial.Description;
            Quantity = wreckMaterial.Quantity;
            Value = wreckMaterial.Value;
            ReceiverValuation = wreckMaterial.ReceiverValuation;
            ValueConfirmed = wreckMaterial.ValueConfirmed;
            // Images = wreckMaterial.Images;
            WreckMaterialOwner = wreckMaterial.WreckMaterialOwner;
            Purchaser = wreckMaterial.Purchaser;
            Outcome = wreckMaterial.Outcome;
            WhereSecured = wreckMaterial.WhereSecured;
            ImportedFromLegacy = wreckMaterial.ImportedFromLegacy;
            Created = wreckMaterial.Created;
            LastModified = wreckMaterial.LastModified;
        }

        public Guid Id { get; }
        public Guid DroitId { get; }
        public string Name { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public int Quantity { get; } = 1;
        public float? Value { get; } = 0;


        [Display(Name = "Receiver Valuation")]
        public float? ReceiverValuation { get; }

        [Display(Name = "Value Confirmed")]
        public bool ValueConfirmed { get; }
        // public List<string> Images { get; } = new List<string>();

        [Display(Name = "Wreck Material Owner")]
        public string? WreckMaterialOwner { get; } = string.Empty;
        public string? Purchaser { get; } = string.Empty;
        public string? Outcome { get; } = string.Empty;

        [Display(Name = "Where Secured")]
        public string? WhereSecured { get; } = string.Empty;

        [Display(Name = "Imported From Legacy")]
        public bool ImportedFromLegacy { get; }
        public DateTime Created { get; }

        [Display(Name = "Last Modified")]
        public DateTime LastModified { get; }
    }
}
