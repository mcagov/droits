using AutoMapper;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs.Webapp;
using Droits.Models.Entities;

namespace Droits.Data.Mappers.Portal
{
    public class WebappSalvorInfoDroitMappingProfile : Profile
    {
        public WebappSalvorInfoDroitMappingProfile()
        {
            CreateMap<Droit, SalvorInfoReportDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Reference,
                    opt => opt.MapFrom(src => src.Reference.ToString()))
                .ForMember(dest => dest.SalvorId, opt => opt.MapFrom(src => src.SalvorId.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.RecoveredFrom,
                    opt => opt.MapFrom(src => src.RecoveredFrom.ToString()))
                .ForMember(dest => dest.Coordinates,
                    opt => opt.MapFrom(src => $"{src.Latitude}°,{src.Longitude}°"))
                .ForMember(dest => dest.Depth,
                    opt => opt.MapFrom(src => src.Depth.ToString()))
                .ForMember(dest => dest.DateFound,
                    opt => opt.MapFrom(src => src.DateFound.ToString()))
                .ForMember(dest => dest.DateReported,
                    opt => opt.MapFrom(src => src.ReportedDate.ToString()))
                .ForMember(dest => dest.LastUpdated,
                    opt => opt.MapFrom(src => src.LastModified.ToString()))
                .ForMember(dest => dest.ReportedWreckName,
                    opt => opt.MapFrom(src => src.ReportedWreckName))
                .ForMember(dest => dest.ReportedWreckYearSunk,
                    opt => opt.MapFrom(src => src.ReportedWreckYearSunk.ToString()))
                .ForMember(dest => dest.ReportedWreckYearConstructed,
                    opt => opt.MapFrom(src => src.ReportedWreckYearConstructed.ToString()))
                .ForMember(dest => dest.ReportedWreckConstructionDetails,
                    opt => opt.MapFrom(src => src.ReportedWreckConstructionDetails))
                .ForMember(dest => dest.LocationDescription,
                    opt => opt.MapFrom(src => src.LocationDescription))
                .ForMember(dest => dest.Latitude,
                    opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude,
                    opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.LocationRadius,
                    opt => opt.MapFrom(src => src.LocationRadius))
                .ForMember(dest => dest.ServicesDescription,
                    opt => opt.MapFrom(src => src.ServicesDescription))
                .ForMember(dest => dest.SalvageAwardClaimed,
                    opt => opt.MapFrom(src => src.SalvageAwardClaimed))
                .ForPath(dest => dest.WreckMaterials, opt => opt.MapFrom(src => src.WreckMaterials));
        }
    }
}