using AutoMapper;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs;
using Droits.Models.Entities;

namespace Droits.Data.Mappers.Submission
{
    public class SalvorMappingProfile : Profile
    {
        public SalvorMappingProfile()
        {
            CreateMap<SubmittedReportDto, Salvor>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Personal != null ? src.Personal.Email.ValueOrEmpty() : string.Empty))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Personal != null ? src.Personal.FullName.ValueOrEmpty() : string.Empty))
                .ForMember(dest => dest.TelephoneNumber, opt => opt.MapFrom(src => src.Personal != null ? src.Personal.TelephoneNumber.ValueOrEmpty() : string.Empty))
                .ForPath(dest => dest.Address.Line1, opt => opt.MapFrom(src => src.Personal != null ? src.Personal.AddressLine1.ValueOrEmpty() : string.Empty))
                .ForPath(dest => dest.Address.Line2, opt => opt.MapFrom(src => src.Personal != null ? src.Personal.AddressLine2.ValueOrEmpty() : string.Empty))
                .ForPath(dest => dest.Address.Town, opt => opt.MapFrom(src => src.Personal != null ? src.Personal.AddressTown.ValueOrEmpty() : string.Empty))
                .ForPath(dest => dest.Address.County, opt => opt.MapFrom(src => src.Personal != null ? src.Personal.AddressCounty.ValueOrEmpty() : string.Empty))
                .ForPath(dest => dest.Address.Postcode, opt => opt.MapFrom(src => src.Personal != null ? src.Personal.AddressPostcode.ValueOrEmpty() : string.Empty));
        }
    }
}