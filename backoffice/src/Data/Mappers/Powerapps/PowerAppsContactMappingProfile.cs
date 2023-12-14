using AutoMapper;
using Droits.Models.Entities;
using System;
using Droits.Helpers;
using Droits.Models.DTOs.Powerapps;

namespace Droits.Data.Mappers.Powerapps
{
    public class PowerAppsContactMappingProfile : Profile
    {
        public PowerAppsContactMappingProfile()
        {
            CreateMap<PowerappsContactDto, Salvor>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PowerappsContactId, opt => opt.MapFrom(src => src.ContactId))
                .ForMember(dest => dest.TelephoneNumber, opt => opt.MapFrom(src => StringHelper.JoinWithSeparator(" / ", src.Telephone1, src.Telephone2, src.Telephone3)))
                .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.MobilePhone))
                .ForPath(dest => dest.Address.Line1, opt => opt.MapFrom(src => src.AddressLine1))
                .ForPath(dest => dest.Address.Line2, opt => opt.MapFrom(src => StringHelper.JoinWithSeparator(", ", src.AddressLine2, src.AddressLine3)))
                .ForPath(dest => dest.Address.Town, opt => opt.MapFrom(src => src.AddressCity))
                .ForPath(dest => dest.Address.County, opt => opt.MapFrom(src => src.AddressCounty))
                .ForPath(dest => dest.Address.Postcode, opt => opt.MapFrom(src => src.AddressPostalCode));

        }
    }
}