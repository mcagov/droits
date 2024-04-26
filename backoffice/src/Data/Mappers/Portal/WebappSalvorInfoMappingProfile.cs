using AutoMapper;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs.Webapp;
using Droits.Models.Entities;

namespace Droits.Data.Mappers.Portal
{
    public class WebappSalvorInfoMappingProfile : Profile
    {
        public WebappSalvorInfoMappingProfile()
        {
            CreateMap<Salvor, SalvorInfoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src =>  src.Email.ValueOrEmpty() ))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>  src.Name.ValueOrEmpty() ))
                .ForMember(dest => dest.TelephoneNumber, opt => opt.MapFrom(src =>  src.TelephoneNumber.ValueOrEmpty() ))
                .ForPath(dest => dest.Address.Line1, opt => opt.MapFrom(src =>  src.Address.Line1.ValueOrEmpty() ))
                .ForPath(dest => dest.Address.Line2, opt => opt.MapFrom(src =>  src.Address.Line2.ValueOrEmpty() ))
                .ForPath(dest => dest.Address.Town, opt => opt.MapFrom(src =>  src.Address.Town.ValueOrEmpty() ))
                .ForPath(dest => dest.Address.County, opt => opt.MapFrom(src =>  src.Address.County.ValueOrEmpty() ))
                .ForPath(dest => dest.Address.Postcode, opt => opt.MapFrom(src =>  src.Address.Postcode.ValueOrEmpty() ))
                .ForPath(dest => dest.Reports, opt => opt.MapFrom(src =>  src.Droits ));
        }
    }
}