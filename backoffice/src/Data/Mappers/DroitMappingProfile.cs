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
                    opt => opt.MapFrom(src => src.LocationDescription == null ? string.Empty : src.LocationDescription.ValueOrEmpty()));
            // .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => int.Parse(src.Quantity.ValueOrEmpty() ?? string.Empty)))
            // .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value.HasValue ? (float)src.Value.Value : 0.0f))
            // .ForMember(dest => dest.ValueKnown, opt => opt.MapFrom(src => src.ValueKnown.AsBoolean()))
            // .ForPath(dest => dest.StorageAddress.Line1, opt => opt.MapFrom(src => src.AddressDetails != null ? src.AddressDetails.AddressLine1.ValueOrEmpty() : string.Empty))
            // .ForPath(dest => dest.StorageAddress.Line2, opt => opt.MapFrom(src => src.AddressDetails != null ? src.AddressDetails.AddressLine2.ValueOrEmpty() : string.Empty))
            // .ForPath(dest => dest.StorageAddress.Town, opt => opt.MapFrom(src => src.AddressDetails != null ? src.AddressDetails.AddressTown.ValueOrEmpty() : string.Empty))
            // .ForPath(dest => dest.StorageAddress.County, opt => opt.MapFrom(src => src.AddressDetails != null ? src.AddressDetails.AddressCounty.ValueOrEmpty() : string.Empty))
            // .ForPath(dest => dest.StorageAddress.Postcode, opt => opt.MapFrom(src => src.AddressDetails != null ? src.AddressDetails.AddressPostcode.ValueOrEmpty() : string.Empty));

        }
    }
}
