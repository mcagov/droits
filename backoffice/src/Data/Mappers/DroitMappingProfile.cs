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
                .ForMember(dest => dest.LocationDescription,
                    opt => opt.MapFrom(src =>
                        src.LocationDescription == null
                            ? string.Empty
                            : src.LocationDescription.ValueOrEmpty()))
                .ForMember(dest => dest.WreckMaterials, opt => opt.Ignore());

        }
    }
}
