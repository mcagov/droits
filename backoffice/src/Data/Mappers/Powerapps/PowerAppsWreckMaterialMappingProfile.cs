using AutoMapper;
using Droits.Models.Entities;
using System;
using Droits.Helpers;
using Droits.Models.DTOs.Powerapps;

namespace Droits.Data.Mappers.Powerapps
{
    public class PowerAppsWreckMaterialMappingProfile : Profile
    {
        public PowerAppsWreckMaterialMappingProfile()
        {
            CreateMap<PowerappsWreckMaterialDto, WreckMaterial>()
                .ForMember(dest => dest.PowerappsWreckMaterialId,
                    opt => opt.MapFrom(src => src.PowerappsWreckMaterialId))
                .ForMember(dest => dest.PowerappsDroitId,
                    opt => opt.MapFrom(src => src.PowerappsDroitId))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src =>
                    src.CreatedOn))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ReceiverValuation,
                    opt => opt.MapFrom(src => src.ReceiverValuation))
                .ForMember(dest => dest.ValueConfirmed,
                    opt => opt.MapFrom(src => src.ValueConfirmed))
                .ForMember(dest => dest.Quantity,
                    opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.Value))
                
                .ForPath(dest => dest.StorageAddress.Line1, opt => opt.MapFrom(src => src.StorageAddress != null ? src.StorageAddress.AddressLine1 : src.WhereSecuredLegacy))
                .ForPath(dest => dest.StorageAddress.Line2, opt => opt.MapFrom(src => src.StorageAddress != null ? StringHelper.JoinWithSeparator(", ", src.StorageAddress.AddressLine2) : string.Empty))
                .ForPath(dest => dest.StorageAddress.Town, opt => opt.MapFrom(src => src.StorageAddress != null ? src.StorageAddress.City : string.Empty))
                .ForPath(dest => dest.StorageAddress.County, opt => opt.MapFrom(src => src.StorageAddress != null ? src.StorageAddress.County : string.Empty))
                .ForPath(dest => dest.StorageAddress.Postcode, opt => opt.MapFrom(src => src.StorageAddress != null ? src.StorageAddress.Postcode : string.Empty))
                
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description))


                .ForMember(dest => dest.WreckMaterialOwner,
                    opt => opt.MapFrom(src =>
                        src.WreckMaterialOwner != null
                            ? src.WreckMaterialOwner.FullName
                            : string.Empty))
                .ForMember(dest => dest.WreckMaterialOwnerContactDetails,
                    opt => opt.MapFrom(src =>
                        src.WreckMaterialOwner != null
                            ? src.WreckMaterialOwner.GetContactDetails()
                            : src.WreckMaterialOwnerLegacy))

                .ForMember(dest => dest.Purchaser,
                    opt => opt.MapFrom(src =>
                        src.Purchaser != null
                            ? src.Purchaser.FullName
                            : string.Empty))
                .ForMember(dest => dest.PurchaserContactDetails,
                    opt => opt.MapFrom(src =>
                        src.Purchaser != null
                            ? src.Purchaser.GetContactDetails()
                            : src.PurchaserLegacy))

                .ForMember(dest => dest.Outcome,
                    opt => opt.MapFrom(src =>
                        src.GetOutcome()))
                .ForMember(dest => dest.OutcomeRemarks,
                    opt => opt.MapFrom(src =>
                        StringHelper.JoinWithSeparator(" | ", src.OutcomeRemarks,
                            src.OutcomeLegacy)));
        }
    }
}