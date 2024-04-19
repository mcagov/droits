using AutoMapper;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs;
using Droits.Models.Entities;

namespace Droits.Data.Mappers
{
    public class DroitMappingProfile : Profile
    {
        public DroitMappingProfile()
        {
            CreateMap<SubmittedReportDto, Droit>()
                .ForMember(dest => dest.ReportedDate,
                    opt => opt.MapFrom(src =>
                        src.ReportDate == null ? DateTime.UtcNow : src.ReportDate.AsDateTime()))
                .ForMember(dest => dest.DateFound,
                    opt => opt.MapFrom(src =>
                        src.WreckFindDate == null ? DateTime.UtcNow : src.WreckFindDate.AsDateTime()))
                .ForMember(dest => dest.Latitude,
                    opt => opt.MapFrom(src =>
                        src.Latitude))
                .ForMember(dest => dest.Longitude,
                    opt => opt.MapFrom(src =>
                        src.Longitude))
                .ForMember(dest => dest.LocationRadius,
                    opt => opt.MapFrom(src =>
                        src.LocationRadius))
                .ForMember(dest => dest.LocationDescription,
                    opt => opt.MapFrom(src =>
                        src.LocationDescription == null
                            ? string.Empty
                            : src.LocationDescription.ValueOrEmpty()))
                .ForMember(dest => dest.Depth,
                    opt => opt.MapFrom(src =>
                        src.VesselDepth.AsInt()))
                .ForMember(dest => dest.RecoveredFrom,
                    opt => opt.MapFrom(src =>
                        src.RemovedFrom != null ? src.RemovedFrom.Replace(" ", "").ToLower() : null))
                .ForMember(dest => dest.RecoveredFromLegacy,
                    opt => opt.MapFrom(src =>
                        src.RemovedFrom))
                .ForMember(dest => dest.SalvageAwardClaimed,
                    opt => opt.MapFrom(src =>
                        src.ClaimSalvage.AsBoolean()))
                .ForMember(dest => dest.ServicesDescription,
                    opt => opt.MapFrom(src =>
                        src.SalvageServices))
                .ForMember(dest => dest.ReportedWreckName,
                    opt => opt.MapFrom(src =>
                        src.VesselName))
                .ForMember(dest => dest.ReportedWreckYearConstructed,
                    opt => opt.MapFrom(src =>
                        src.VesselConstructionYear!= null ? src.VesselConstructionYear.AsInt() : null))
                .ForMember(dest => dest.ReportedWreckConstructionDetails,
                    opt => opt.MapFrom(src =>
                        src.WreckDescription))
                .ForMember(dest => dest.ReportedWreckYearSunk,
                    opt => opt.MapFrom(src =>
                        src.VesselSunkYear!= null ? src.VesselSunkYear.AsInt() : null))
                .ForMember(dest => dest.WreckMaterials, opt => opt.Ignore());

        }
    }
}
