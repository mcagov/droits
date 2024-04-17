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
    public class AccessDroitMappingProfile : Profile
    {
        public AccessDroitMappingProfile()
        {
            CreateMap<AccessDto, Droit>()
                .ForMember(dest => dest.Reference,
                    opt => opt.MapFrom(src =>
                        src.DroitNumber))
                .ForMember(dest => dest.ImportedFromLegacy,
                    opt => opt.MapFrom(src => true))
                 .ForMember(dest => dest.OriginalSubmission,
                    opt => opt.MapFrom(src =>
                        JsonConvert.SerializeObject(src)))
                .ForMember(dest => dest.ReportedDate,
                    opt => opt.MapFrom(src =>
                        src.DateReported.IsNullOrEmpty() ? DateTime.MinValue.Date : DateTime.Parse(src.DateReported).Date))
                .ForMember(dest => dest.DateFound,
                    opt => opt.MapFrom(src =>
                        src.DateFound.IsNullOrEmpty() ? DateTime.MinValue.Date : DateTime.Parse(src.DateFound).Date))
                .ForMember(dest => dest.LocationDescription,
                    opt => opt.MapFrom(src =>
                        src.GetLocationDescription()))
                .ForMember(dest => dest.RecoveredFromLegacy,
                    opt => opt.MapFrom(src =>
                        src.RecoveredFrom ))
                .ForMember(dest => dest.SalvageAwardClaimed,
                    opt => opt.MapFrom(src =>
                        src.SalvageAwardClaimed))
                .ForMember(dest => dest.ServicesDescription,
                    opt => opt.MapFrom(src =>
                        src.NatureOfServices))
                .ForMember(dest => dest.ReportedWreckName,
                    opt => opt.MapFrom(src =>
                        src.WreckName))
                .ForMember(dest => dest.ReportedWreckConstructionDetails,
                    opt => opt.MapFrom(src =>
                        src.WreckConstructionDetails))
                .ForMember(dest => dest.Agent,
                    opt => opt.MapFrom(src =>
                        src.Agent))
                .ForMember(dest => dest.District,
                    opt => opt.MapFrom(src =>
                        src.District))
                .ForMember(dest => dest.ClosedDate,
                    opt => opt.MapFrom(src =>
                        src.ClosureOfDroits.IsNullOrEmpty() ? DateTime.MinValue.Date : DateTime.Parse(src.ClosureOfDroits).Date))
                .ForMember(dest => dest.Depth,
                    opt => opt.MapFrom(src =>
                        src.Depth.AsInt()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 
                    src.ClosureOfDroits.IsNullOrEmpty() ? DroitStatus.Received : DroitStatus.Closed
                ))
                .ForMember(dest => dest.WreckMaterials, opt => opt.Ignore())
                .ForMember(dest => dest.RecoveredFrom, opt => opt.Ignore());

            

        }
    }
}
