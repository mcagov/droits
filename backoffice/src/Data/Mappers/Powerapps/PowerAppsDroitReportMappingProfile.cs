using AutoMapper;
using Droits.Models.Entities;
using System;
using Droits.Helpers;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs.Powerapps;

namespace Droits.Data.Mappers.Powerapps
{
    public class PowerAppsDroitReportMappingProfile : Profile
    {
        public PowerAppsDroitReportMappingProfile()
        {
            CreateMap<PowerappsDroitReportDto, Droit>()
                .ForMember(dest => dest.PowerappsDroitId, opt => opt.MapFrom(src =>
                    src.Mcawreckreportid))
                .ForMember(dest => dest.PowerappsWreckId, opt => opt.MapFrom(src =>
                    src.WreckValue))
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(src =>
                        src.ReportReference))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    src.GetDroitStatus()))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src =>
                            src.CreatedOn))
                .ForMember(dest => dest.ReportedDate,
                    opt => opt.MapFrom(src =>
                        src.DateReported))
                .ForMember(dest => dest.DateFound,
                    opt => opt.MapFrom(src =>
                        src.DateFound))
                
                .ForMember(dest => dest.InUkWaters,
                    opt => opt.MapFrom(src =>
                        src.InUkWaters))
                .ForMember(dest => dest.IsHazardousFind,
                    opt => opt.MapFrom(src =>
                        src.HazardousFind))
                
                .ForMember(dest => dest.ServicesDuration,
                    opt => opt.MapFrom(src =>
                        src.ServicesDuration))
                
                .ForMember(dest => dest.ServicesDescription,
                    opt => opt.MapFrom(src =>
                        src.ServicesDescription))
                
                .ForMember(dest => dest.ServicesEstimatedCost,
                    opt => opt.MapFrom(src =>
                        src.ServicesEstimatedCost))
                
                
                .ForMember(dest => dest.Latitude,
                    opt => opt.MapFrom(src =>
                        src.Latitude))
                .ForMember(dest => dest.Longitude,
                    opt => opt.MapFrom(src =>
                        src.Longitude))
                .ForMember(dest => dest.LocationRadius,
                    opt => opt.MapFrom(src =>
                        src.LocationRadius))
                .ForMember(dest => dest.Depth,
                    opt => opt.MapFrom(src =>
                        src.Depth))
                .ForMember(dest => dest.LocationDescription,
                    opt => opt.MapFrom(src =>
                        src.LocationDescription.ValueOrEmpty()))
                .ForMember(dest => dest.RecoveredFrom,
                    opt => opt.MapFrom(src =>
                        src.GetRecoveredFrom()))
                .ForMember(dest => dest.SalvageAwardClaimed,
                    opt => opt.MapFrom(src =>
                        src.SalvageAwardClaimed))
                .ForMember(dest => dest.SalvageClaimAwarded,
                    opt => opt.MapFrom(src =>
                        src.SalvageClaimAwarded))
                .ForMember(dest => dest.MmoLicenceRequired,
                    opt => opt.MapFrom(src =>
                        src.MmoLicenseRequired))
                .ForMember(dest => dest.MmoLicenceProvided,
                    opt => opt.MapFrom(src =>
                        src.MmoLicenseProvided))
                
                .ForMember(dest => dest.ReportedWreckName,
                    opt => opt.MapFrom(src =>
                        src.VesselName))
                .ForMember(dest => dest.ReportedWreckYearSunk,
                    opt =>     opt.MapFrom(src => src.VesselYearSunk!= null ? src.VesselYearSunk.AsInt() : null))
                        .ForMember(dest => dest.ReportedWreckConstructionDetails,
                    opt => opt.MapFrom(src =>
                        src.WreckConstructionDetails))
                .ForMember(dest => dest.ReportedWreckYearConstructed,
                    opt => opt.MapFrom(src => src.VesselYearConstructed!= null ? src.VesselYearConstructed.AsInt() : null))

                .ForMember(dest => dest.RecoveredFromLegacy,
                    opt => opt.MapFrom(src =>
                        src.RecoveredFromLegacy))
                .ForMember(dest => dest.Agent,
                    opt => opt.MapFrom(src =>
                        src.AgentLegacy))
                .ForMember(dest => dest.DateDelivered,
                    opt => opt.MapFrom(src =>
                        src.DateDelivered))
                .ForMember(dest => dest.GoodsDischargedBy,
                    opt => opt.MapFrom(src =>
                        src.GoodsDischargedBy))
                .ForMember(dest => dest.LegacyFileReference,
                    opt => opt.MapFrom(src =>
                        src.LegacyFileReference))
                .ForMember(dest => dest.District,
                    opt => opt.MapFrom(src =>
                        src.District))
                .ForMember(dest => dest.ImportedFromLegacy,
                    opt => opt.MapFrom(src =>
                        src.ImportedFromLegacy))
                .ForMember(dest => dest.LegacyRemarks,
                    opt => opt.MapFrom(src =>
                        StringHelper.JoinWithSeparator(" | ",src.RemarksLegacy1, src.RemarksLegacy2)))
            
                
                
                //Need to match up wreck from WreckValue (Powerapps Id)
                .ForMember(dest => dest.Wreck, opt => opt.Ignore())
                .ForMember(dest => dest.WreckMaterials, opt => opt.Ignore());

        }
    }
}