namespace Droits.Models.DTOs.Imports;

public class WMRowDto
{
    
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; } = string.Empty;
        
        public string? Quantity { get; set; }
        
        public string? SalvorValuation { get; set; }
        
        public string? ReceiverValuation { get; set; }
        
        public string? ValueConfirmed { get; set; }
        
        public string? StorageLine1 { get; set; }

        public string? StorageLine2 { get; set; }

        public string? StorageCityTown { get; set; }

        public string? StorageCounty { get; set; }
        
        public string? StoragePostcode { get; set; }

        public string? WreckMaterialOwner { get; set; } = string.Empty;
        
        public string? WreckMaterialOwnerContactDetails { get; set; } = string.Empty;
        
        public string? Purchaser { get; set; } = string.Empty;
        
        public string? PurchaserContactDetails { get; set; } = string.Empty;
        

}