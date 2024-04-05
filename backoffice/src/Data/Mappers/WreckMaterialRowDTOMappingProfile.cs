using AutoMapper;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Imports;
using Droits.Models.Entities;
using Droits.Models.FormModels;

namespace Droits.Data.Mappers
{
    public class WreckMaterialRowDtoMappingProfile : Profile
    {
        public WreckMaterialRowDtoMappingProfile()
        {
            CreateMap<WMRowDto, WreckMaterialForm>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Quantity,
                    opt => opt.MapFrom(src => src.Quantity != null ? int.Parse(src.Quantity) : 0))
                .ForMember(dest => dest.SalvorValuation,
                    opt => opt.MapFrom(src => double.Parse(src.SalvorValuation ?? string.Empty)))
                .ForMember(dest => dest.ReceiverValuation,
                    opt => opt.MapFrom(src => double.Parse(src.ReceiverValuation ?? string.Empty)))
                .ForMember(dest => dest.ValueConfirmed,
                    opt => opt.MapFrom(src => src.ValueConfirmed.AsBoolean()))
                .ForMember(dest => dest.WreckMaterialOwner,
                    opt => opt.MapFrom(src => src.WreckMaterialOwner))
                .ForMember(dest => dest.WreckMaterialOwnerContactDetails,
                    opt => opt.MapFrom(src => src.WreckMaterialOwnerContactDetails))
                .ForMember(dest => dest.Purchaser, opt => opt.MapFrom(src => src.Purchaser))
                .ForMember(dest => dest.PurchaserContactDetails,
                    opt => opt.MapFrom(src => src.PurchaserContactDetails))
                .ForMember(dest => dest.StorageAddress,
                    opt => opt.MapFrom(src => new Address()
                    {
                        Line1 = src.StorageLine1,
                        Line2 = src.StorageLine2,
                        Town = src.StorageCityTown,
                        County = src.StorageCounty,
                        Postcode = src.StoragePostcode
                    }));

        }
    }
}