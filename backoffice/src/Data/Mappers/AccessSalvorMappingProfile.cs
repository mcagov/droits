using AutoMapper;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Imports;
using Droits.Models.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Droits.Data.Mappers
{
    public class AccessSalvorMappingProfile : Profile
    {
        public AccessSalvorMappingProfile()
        {
            CreateMap<AccessDto, Salvor>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src =>
                        src.SalvorName))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src =>
                        $"{Guid.NewGuid()}@GeneratedSalvorEmail.com"))
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(src => src.GetAddress()
                    ))
                .ForPath(dest => dest.Address.Postcode,
                    opt => opt.MapFrom(src => src.PostCode.ValueOrEmpty()));

        }
        
    }
}
