using AutoMapper;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Imports;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Droits.Data.Mappers
{
    public class AccessWreckMaterialMappingProfile : Profile
    {
        public AccessWreckMaterialMappingProfile()
        {
            CreateMap<AccessDto, WreckMaterial>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src =>
                        $"{src.DroitNumber} Access Import"))
                .ForMember(dest => dest.SalvorValuation,
                    opt => opt.MapFrom(src =>
                        src.Value.AsDouble()))
                .ForMember(dest => dest.Purchaser,
                    opt => opt.MapFrom(src =>
                        src.Purchaser))
                .ForMember(dest => dest.Outcome,
                    opt => opt.MapFrom(src =>
                        src.Outcome != null? src.Outcome.AsWreckMaterialOutcomeEnum(): null))
                .ForMember(dest => dest.OutcomeRemarks,
                    opt => opt.MapFrom(src =>
                        src.Outcome))
                .ForMember(dest => dest.StorageAddress,
                    opt => opt.MapFrom(src =>
                        src.GetStorageAddress()))
                .ForMember(dest => dest.DroitId, opt => opt.Ignore());


        }
    }
}
