using AutoMapper;
using Droits.Models.Entities;
using System;
using Droits.Helpers;
using Droits.Models.DTOs.Powerapps;

namespace Droits.Data.Mappers.Powerapps
{
    public class PowerAppsUserMappingProfile : Profile
    {
        public PowerAppsUserMappingProfile()
        {
            CreateMap<PowerappsUserDto, ApplicationUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
        }
    }
}