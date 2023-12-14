using AutoMapper;
using Droits.Models.Entities;
using System;
using Droits.Models.DTOs.Powerapps;

namespace Droits.Data.Mappers.Powerapps
{
    public class PowerAppsWreckMappingProfile : Profile
    {
        public PowerAppsWreckMappingProfile()
        {
            CreateMap<PowerappsWreckDto, Wreck>()
                .ForMember(dest => dest.PowerappsWreckId,
                    opt => opt.MapFrom(src => src.Mcawrecksid))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.WreckType,
                    opt => opt.MapFrom(src => src.GetWreckType()))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.CreatedOn))

                .ForMember(dest => dest.DateOfLoss, opt => opt.MapFrom(src => src.DateOfLoss))

                .ForMember(dest => dest.IsWarWreck, opt => opt.MapFrom(src => src.IsWarWreck))
                .ForMember(dest => dest.IsAnAircraft,
                    opt => opt.MapFrom(src => src.IsAircraft))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.IsProtectedSite,
                    opt => opt.MapFrom(src => src.ProtectedSite))
                .ForMember(dest => dest.ProtectionLegislation,
                    opt => opt.MapFrom(src => src.GetProtectedLegislation()))
                .ForMember(dest => dest.OwnerName,
                    opt => opt.MapFrom(src => src.WreckOwner != null ? src.WreckOwner.FullName :""))
                .ForMember(dest => dest.OwnerEmail, opt => opt.MapFrom(src => src.WreckOwner != null ? src.WreckOwner.EmailAddress :""))
                .ForMember(dest => dest.OwnerNumber, opt => opt.MapFrom(src => src.WreckOwner != null ? src.WreckOwner.MobilePhone :""))
                .ForMember(dest => dest.OwnerAddress, opt => opt.MapFrom(src => src.WreckOwner != null ? src.WreckOwner.AddressComposite :""))
                .ForMember(dest => dest.AdditionalInformation, opt => opt.Ignore()) //This isn't in the current system
                .ForMember(dest => dest.ConstructionDetails,
                    opt => opt.Ignore()) //This isn't in the current system
                .ForMember(dest => dest.YearConstructed,
                    opt => opt.Ignore()) //This isn't in the current system
                .ForMember(dest => dest.InUkWaters,
                    opt => opt.MapFrom(src => false)); //This isn't in the current system

        }
    }
}