using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Models.DTOs.Exports
{
    public class WreckExportDto
    {

        public WreckExportDto(Wreck wreck)
        {
            Id = wreck.Id;
            Created = wreck.Created;
            LastModified = wreck.LastModified;
            Name = wreck.Name;
            Type = wreck.WreckType;
            ConstructionDetails = wreck.ConstructionDetails;
            YearConstructed = wreck.YearConstructed;
            DateOfLoss = wreck.DateOfLoss?.ToString("dd/MM/yyyy");
            InUkWaters = wreck.InUkWaters;
            IsWarWreck = wreck.IsWarWreck;
            IsAnAircraft = wreck.IsAnAircraft;
            Latitude = wreck.Latitude;
            Longitude = wreck.Longitude;
            IsProtectedSite = wreck.IsProtectedSite;
            ProtectionLegislation = wreck.ProtectionLegislation;
            OwnerName = wreck.OwnerName;
            OwnerEmail = wreck.OwnerEmail;
            OwnerNumber = wreck.OwnerNumber;
            OwnerAddress = wreck.OwnerAddress;
            AdditionalInformation = wreck.AdditionalInformation;
            DroitCount = wreck.Droits.Count.ToString();
        }


        public Guid Id { get; set; }

        public DateTime? Created { get; set; }
        [DisplayName("Last Modified")]
        public DateTime? LastModified { get; set; }
        public string? Name { get; set; }
        
        [DisplayName("Wreck Type")]
        public WreckType? Type { get; set; }
        
        [DisplayName("Construction Details")]
        public string? ConstructionDetails { get; set; }
        [DisplayName("Year Constructed")]
        public int? YearConstructed { get; set; }
        [DisplayName("Date of Loss")]
        public string? DateOfLoss { get; set; }
        [DisplayName("In Uk Waters?")]
        public bool InUkWaters { get; set; }
        [DisplayName("Is War Wreck?")]
        public bool IsWarWreck { get; set; }
        [DisplayName("Is An Aircraft?")]
        public bool IsAnAircraft { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        [DisplayName("Is Protect Site?")]
        public bool IsProtectedSite { get; set; }
        [DisplayName("Protection Legislation")]
        public string? ProtectionLegislation { get; set; }

        [DisplayName("Additional Information")]
        public string? AdditionalInformation { get; set; }
        [DisplayName("Owner Name")]
        public string? OwnerName { get; set; }
        [DisplayName("Owner Email")]
        public string? OwnerEmail { get; set; }
        [DisplayName("Owner Number")]
        public string? OwnerNumber { get; set; }
        [DisplayName("Owner Address")]
        public string? OwnerAddress { get; set; }
        [DisplayName("Droit Count")]
        public string? DroitCount { get; set; }
        [DisplayName("Droit References")]
        public string? DroitRefs { get; set; }
    }
}
