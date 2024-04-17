using AutoMapper;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs;
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
                .ForPath(dest => dest.Address.Line1,
                    opt => opt.MapFrom(src =>
                        src.GetAddressLine(1).IsNullOrEmpty() ? "" :
                        src.GetAddressLine(5).IsNullOrEmpty()
                            ? src.GetAddressLine(1)
                            : src.GetAddressLine(1) + " " + src.GetAddressLine(2)
                    ))
                .ForPath(dest => dest.Address.Line2,
                    opt => opt.MapFrom(src =>
                        src.GetAddressLine(2).IsNullOrEmpty() ? "" :
                            src.GetAddressLine(3).IsNullOrEmpty() ? src.GetAddressLine(2) :
                        src.GetAddressLine(5).IsNullOrEmpty()
                            ? src.GetAddressLine(4).IsNullOrEmpty() ? "" : src.GetAddressLine(2)
                            : src.GetAddressLine(3)
                    ))
                .ForPath(dest => dest.Address.Town,
                    opt => opt.MapFrom(src =>
                        src.GetAddressLine(2).IsNullOrEmpty() ? "" :
                        src.GetAddressLine(4).IsNullOrEmpty()
                            ? src.GetAddressLine(5).IsNullOrEmpty()
                                ? src.GetAddressLine(2)
                                : src.GetAddressLine(3)
                            : src.GetAddressLine(4)
                    ))
                .ForPath(dest => dest.Address.County,
                    opt => opt.MapFrom(src =>
                        src.GetAddressLine(2).IsNullOrEmpty() ? "" :
                        src.GetAddressLine(4).IsNullOrEmpty()
                            ? src.GetAddressLine(5).IsNullOrEmpty()
                                ? src.GetAddressLine(3)
                                : src.GetAddressLine(4)
                            : src.GetAddressLine(5)))
                .ForPath(dest => dest.Address.Postcode,
                    opt => opt.MapFrom(src => src.PostCode.ValueOrEmpty()));

        }
        
    }
}
