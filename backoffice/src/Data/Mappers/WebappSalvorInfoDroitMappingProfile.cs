using AutoMapper;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs.Webapp;
using Droits.Models.Entities;

namespace Droits.Data.Mappers
{
    public class WebappSalvorInfoDroitMappingProfile : Profile
    {
        public WebappSalvorInfoDroitMappingProfile()
        {
            CreateMap<Droit, SalvorInfoReportDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Reference,
                    opt => opt.MapFrom(src => src.Reference.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.RecoveredFrom,
                    opt => opt.MapFrom(src => src.RecoveredFrom.ToString()))
                .ForMember(dest => dest.Coordinates,
                    opt => opt.MapFrom(src => $"{src.Latitude}°,{src.Longitude}°"))
                .ForMember(dest => dest.DateFound,
                    opt => opt.MapFrom(src => src.DateFound.ToString()))
                .ForMember(dest => dest.DateReported,
                    opt => opt.MapFrom(src => src.ReportedDate.ToString()))
                .ForMember(dest => dest.LastUpdated,
                    opt => opt.MapFrom(src => src.LastModified.ToString()))
            .ForPath(dest => dest.WreckMaterials, opt => opt.MapFrom(src => src.WreckMaterials));
        }
    }
}