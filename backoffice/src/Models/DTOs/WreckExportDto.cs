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
        public DateTime? LastModified { get; set; }
        public string Name { get; set; }
        public string? VesselConstructionDetails { get; set; }
        public int? VesselYearConstructed { get; set; }
        public DateTime? DateOfLoss { get; set; }
        public bool InUkWaters { get; set; }
        public bool IsWarWreck { get; set; }
        public bool IsAnAircraft { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public bool IsProtectedSite { get; set; }
        public string? ProtectionLegislation { get; set; }

        public string? OwnerName { get; set; }
        public string? OwnerEmail { get; set; }
        public string? OwnerNumber { get; set; }

        public string? AdditionalInformation { get; set; }
    }
}
