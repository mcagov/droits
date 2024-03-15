using AutoMapper;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs;
using Droits.Models.Entities;

namespace Droits.Data.Mappers
{
    public class WreckMaterialMappingProfile : Profile
    {
        public WreckMaterialMappingProfile()
        {
            CreateMap<SubmittedWreckMaterialDto, WreckMaterial>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Description == null ? string.Empty : src.Description.ValueOrEmpty()))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity!= null? int.Parse(src.Quantity): 0))
                .ForMember(dest => dest.SalvorValuation, opt => opt.MapFrom(src => src.Value ?? 0.0d))
                .ForMember(dest => dest.ValueKnown, opt => opt.MapFrom(src => src.ValueKnown.AsBoolean()))
                .ForPath(dest => dest.StorageAddress.Line1, opt => opt.MapFrom(src => src.AddressDetails != null ? src.AddressDetails.AddressLine1.ValueOrEmpty() : string.Empty))
                .ForPath(dest => dest.StorageAddress.Line2, opt => opt.MapFrom(src => src.AddressDetails != null ? src.AddressDetails.AddressLine2.ValueOrEmpty() : string.Empty))
                .ForPath(dest => dest.StorageAddress.Town, opt => opt.MapFrom(src => src.AddressDetails != null ? src.AddressDetails.AddressTown.ValueOrEmpty() : string.Empty))
                .ForPath(dest => dest.StorageAddress.County, opt => opt.MapFrom(src => src.AddressDetails != null ? src.AddressDetails.AddressCounty.ValueOrEmpty() : string.Empty))
                .ForPath(dest => dest.StorageAddress.Postcode, opt => opt.MapFrom(src => src.AddressDetails != null ? src.AddressDetails.AddressPostcode.ValueOrEmpty() : string.Empty));
        }
    }
}
