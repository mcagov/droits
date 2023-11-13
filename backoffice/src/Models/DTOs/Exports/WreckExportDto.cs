using System.ComponentModel;
using Droits.Models.Entities;

namespace Droits.Models.DTOs
{
    public class WreckExportDto
    {
        public WreckExportDto()
        {

        }

        public WreckExportDto(Wreck wreck)
        {
            Created = wreck.Created;
            LastModified = wreck.LastModified;
            Name = wreck.Name;
            VesselConstructionDetails = wreck.VesselConstructionDetails;
            VesselYearConstructed = wreck.VesselYearConstructed;
            DateOfLoss = wreck.DateOfLoss;
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
            AdditionalInformation = wreck.AdditionalInformation;
        }

        public DateTime? Created { get; set; }
        [DisplayName("Last Modified")]
        public DateTime? LastModified { get; set; }
        public string Name { get; set; }
        [DisplayName("Vessel Construction Details")]
        public string? VesselConstructionDetails { get; set; }
        [DisplayName("Vessel Year Constructed")]
        public int? VesselYearConstructed { get; set; }
        [DisplayName("Date of Loss")]
        public DateTime? DateOfLoss { get; set; }
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
        [DisplayName("Owner Name")]
        public string? OwnerName { get; set; }
        [DisplayName("Owner Email")]
        public string? OwnerEmail { get; set; }
        [DisplayName("Owner Number")]
        public string? OwnerNumber { get; set; }
        [DisplayName("Additional Information")]
        public string? AdditionalInformation { get; set; }
    }
}
