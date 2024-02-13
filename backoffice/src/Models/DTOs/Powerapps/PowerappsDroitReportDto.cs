using System.Text.Json.Serialization;
using Droits.Models.Enums;

// https://reportwreckmaterial.crm11.dynamics.com/api/data/v9.2/crf99_mcawreckreports?$top=10&$select=crf99_mcawreckreportid,crf99_reportreference,createdon,crf99_recoveredfrom,crf99_datereported,crf99_datefound,crf99_inukwaters,crf99_hazardousfind,crf99_servicesduration,crf99_servicesdescription,crf99_servicesestimatedcost_base,crf99_servicesestimatedcost,crf99_negotiatingsalvageaward,crf99_locationdescription,crf99_locationradius,crf99_depth,crf99_latitude,crf99_longitude,_crf99_wreck_value,crf99_vesselname,crf99_vesselyearsunk,crf99_wreckconstructiondetails,crf99_vesselyearconstructed,crf99_salvageclaimawarded,crf99_salvageawardclaimed,crf99_salvageclaimawarded_base,crf99_mmolicencerequired,crf99_mmolicenceprovided,crf99_recoveredfromlegacy,crf99_importedfromlegacy,crf99_remarkslegacy1,crf99_remarkslegacy2,crf99_agentlegacy,crf99_datedelivered,crf99_goodsdischargedby,crf99_legacyfilereference,crf99_district,crf99_acknowledgementsent,crf99_inprogress,crf99_investigationcomplete,crf99_closeddate,crf99_underinvestigation,crf99_researchingreport,crf99_requestinginformation&$expand=crf99_Reporter($select=contactid,fullname,emailaddress1,telephone1,telephone2,telephone3,mobilephone,address1_line1,address1_line2,address1_line3,address1_city,address1_county,address1_country,address1_postalcode,address1_composite)
namespace Droits.Models.DTOs.Powerapps
{
    public class PowerappsDroitReportsDto
    {
        [JsonPropertyName("value")]
        public List<PowerappsDroitReportDto>? Value { get; set; }
    }
    
    public class PowerappsDroitReportDto
    {
        
        [JsonPropertyName("crf99_inprogress")]
        public int? InProgress { get; set; }

        [JsonPropertyName("crf99_investigationcomplete")]
        public bool? InvestigationComplete { get; set; }
        [JsonPropertyName("crf99_servicesestimatedcost_base")]
        public decimal? ServicesEstimatedCostBase { get; set; }
        [JsonPropertyName("crf99_salvageclaimawarded_base")]
        public decimal? SalvageClaimAwardedBase { get; set; }
        
        
        [JsonPropertyName("crf99_mcawreckreportid")]
        public string? Mcawreckreportid { get; set; }

        [JsonPropertyName("crf99_reportreference")]
        public string? ReportReference { get; set; }

        [JsonPropertyName("crf99_Reporter")]
        public PowerappsContactDto? Reporter { get; set; }
        
        [JsonPropertyName("crf99_Receiver")]
        public PowerappsUserDto? Receiver { get; set; }
        
        [JsonPropertyName("modifiedby")]
        public PowerappsUserDto? ModifiedBy { get; set; }

        [JsonPropertyName("createdon")]
        public DateTime? CreatedOn { get; set; }

        [JsonPropertyName("crf99_recoveredfrom")]
        public int? RecoveredFrom { get; set; }

        [JsonPropertyName("crf99_datereported")]
        public DateTime? DateReported { get; set; }

        [JsonPropertyName("crf99_datefound")]
        public DateTime? DateFound { get; set; }

        [JsonPropertyName("crf99_inukwaters")]
        public bool? InUkWaters { get; set; }

        [JsonPropertyName("crf99_hazardousfind")]
        public bool? HazardousFind { get; set; }

        [JsonPropertyName("crf99_servicesduration")]
        public string? ServicesDuration { get; set; }

        [JsonPropertyName("crf99_servicesdescription")]
        public string? ServicesDescription { get; set; }
        
        [JsonPropertyName("crf99_servicesestimatedcost")]
        public decimal? ServicesEstimatedCost { get; set; }

        [JsonPropertyName("crf99_negotiatingsalvageaward")]
        public int? NegotiatingSalvageAward { get; set; }

        [JsonPropertyName("crf99_locationdescription")]
        public string? LocationDescription { get; set; }

        [JsonPropertyName("crf99_locationradius")]
        public int? LocationRadius { get; set; }

        [JsonPropertyName("crf99_depth")]
        public decimal? Depth { get; set; }

        [JsonPropertyName("crf99_latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("crf99_longitude")]
        public double? Longitude { get; set; }

        [JsonPropertyName("_crf99_wreck_value")]
        //Powerapps Wreck ID
        public string? WreckValue { get; set; }

        //Reported Wreck fields
        [JsonPropertyName("crf99_vesselname")]
        public string? VesselName { get; set; }

        [JsonPropertyName("crf99_vesselyearsunk")]
        public string? VesselYearSunk { get; set; }

        [JsonPropertyName("crf99_wreckconstructiondetails")]
        public string? WreckConstructionDetails { get; set; }

        [JsonPropertyName("crf99_vesselyearconstructed")]
        public string?  VesselYearConstructed { get; set; }
        
//Salvage
        [JsonPropertyName("crf99_salvageclaimawarded")]
        public decimal? SalvageClaimAwarded { get; set; }

        [JsonPropertyName("crf99_salvageawardclaimed")]
        public bool? SalvageAwardClaimed { get; set; }



        [JsonPropertyName("crf99_mmolicencerequired")]
        public bool? MmoLicenseRequired { get; set; }

        [JsonPropertyName("crf99_mmolicenceprovided")]
        public bool? MmoLicenseProvided { get; set; }


        //Legacy
        [JsonPropertyName("crf99_remarkslegacy1")]
        public string? RemarksLegacy1 { get; set; }

        [JsonPropertyName("crf99_remarkslegacy2")]
        public string? RemarksLegacy2 { get; set; }

        [JsonPropertyName("crf99_agentlegacy")]
        public string? AgentLegacy { get; set; }

        [JsonPropertyName("crf99_datedelivered")]
        public DateTime? DateDelivered { get; set; }

        [JsonPropertyName("crf99_goodsdischargedby")]
        public string? GoodsDischargedBy { get; set; }

        [JsonPropertyName("crf99_legacyfilereference")]
        public string? LegacyFileReference { get; set; }

        [JsonPropertyName("crf99_district")]
        public string? District { get; set; }
        [JsonPropertyName("crf99_recoveredfromlegacy")]
        public string? RecoveredFromLegacy { get; set; }

        [JsonPropertyName("crf99_importedfromlegacy")]
        public bool? ImportedFromLegacy { get; set; }

        
        //Status flags
        [JsonPropertyName("crf99_acknowledgementsent")]
        public bool? AcknowledgementSent { get; set; }

        [JsonPropertyName("crf99_closeddate")]
        public DateTime? ClosedDate { get; set; }

        [JsonPropertyName("crf99_underinvestigation")]
        public int? UnderInvestigation { get; set; }

        [JsonPropertyName("crf99_researchingreport")]
        public int? ResearchingReport { get; set; }

        [JsonPropertyName("crf99_requestinginformation")]
        public int? RequestingInformation { get; set; }


        public RecoveredFrom? GetRecoveredFrom() => RecoveredFrom switch
        {
            614880000 => Enums.RecoveredFrom.Shipwreck,
            614880001 => Enums.RecoveredFrom.Seabed,
            614880002 => Enums.RecoveredFrom.Afloat,
            614880003 => Enums.RecoveredFrom.SeaShore,
            _ => null
        };



        private static bool LookupBooleanValue(int value) => value switch
        {
            614880000 => true,
            614880001 => true,
            614880002 => false,
            _ => false
        };


        public DroitStatus GetDroitStatus()
        {
            if ( ClosedDate != null )
            {
                return DroitStatus.Closed;
                // If it has a closed date, it's the final value.
            }

            if ( AcknowledgementSent == null || !AcknowledgementSent.Value )
                return DroitStatus.Received;

            var isUnderInvestigation = UnderInvestigation != null &&
                                       LookupBooleanValue(UnderInvestigation.Value);
            var isRequestingInformation = RequestingInformation != null &&
                                          LookupBooleanValue(RequestingInformation.Value);
            var isResearchingReport =
                ResearchingReport != null && LookupBooleanValue(ResearchingReport.Value);
            var isNegotiatingSalvageAward = NegotiatingSalvageAward != null &&
                                            LookupBooleanValue(NegotiatingSalvageAward.Value);

            return ( isUnderInvestigation, isRequestingInformation, isResearchingReport,
                    isNegotiatingSalvageAward ) switch
                {
                    (true, true, true, true) => DroitStatus.NegotiatingSalvageAward,
                    (true, true, true, false) => DroitStatus.Research,
                    (true, _, false, false) => DroitStatus.InitialResearch,
                    (_, true, false, false) => DroitStatus.InitialResearch,
                    (false, false, false, false) => DroitStatus.AcknowledgementLetterSent,
                    (_, _, _, _) => DroitStatus.Received
                };
        }

    }

}
