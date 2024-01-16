using AutoMapper;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs.Webapp;
using Droits.Models.Entities;

namespace Droits.Data.Mappers
{
    public class WebappSalvorInfoWreckMaterialMappingProfile : Profile
    {
        public WebappSalvorInfoWreckMaterialMappingProfile()
        {
            CreateMap<WreckMaterial, SalvorInfoWreckMaterialDto>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Outcome, opt => opt.MapFrom(src => src.Outcome))
                .ForPath(dest => dest.StorageAddress.Line1,
                    opt => opt.MapFrom(src => src.StorageAddress.Line1))
            .ForPath(dest => dest.StorageAddress.Line2,
                    opt => opt.MapFrom(src => src.StorageAddress.Line2))
            .ForPath(dest => dest.StorageAddress.Town,
                    opt => opt.MapFrom(src => src.StorageAddress.Town))
            .ForPath(dest => dest.StorageAddress.County,
                    opt => opt.MapFrom(src => src.StorageAddress.County))
            .ForPath(dest => dest.StorageAddress.Postcode,
            opt => opt.MapFrom(src => src.StorageAddress.Postcode));
        }
    }
}