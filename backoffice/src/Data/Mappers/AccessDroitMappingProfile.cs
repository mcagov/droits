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
                        src.DateReported.IsNullOrEmpty()
                            ? DateTime.MinValue.Date
                            : src.DateReported.AsDateTime()))
                .ForMember(dest => dest.DateFound,
                    opt => opt.MapFrom(src =>
                        src.DateFound.IsNullOrEmpty()
                            ? DateTime.MinValue.Date
                            : src.DateFound.AsDateTime()))
                .ForMember(dest => dest.LocationDescription,
                    opt => opt.MapFrom(src =>
                        src.GetLocationDescription()))
                .ForMember(dest => dest.InUkWaters,
                    opt => opt.MapFrom(src =>
                        src.UkWaters.AsBoolean()))
                .ForMember(dest => dest.SalvageAwardClaimed,
                    opt => opt.MapFrom(src =>
                        src.SalvageAwardClaimed.AsBoolean()))
                .ForMember(dest => dest.ServicesDescription,
                    opt => opt.MapFrom(src =>
                        src.NatureOfServices))
                .ForMember(dest => dest.ServicesDuration,
                    opt => opt.MapFrom(src =>
                        src.Duration))
                .ForMember(dest => dest.ServicesEstimatedCost,
                    opt => opt.MapFrom(src =>
                        src.EstimatedCostOfServices.AsDouble()))
                .ForMember(dest => dest.ReportedWreckName,
                    opt => opt.MapFrom(src =>
                        src.WreckName))
                .ForMember(dest => dest.ReportedWreckYearSunk,
                    opt => opt.MapFrom(src =>
                        src.YearOfLoss.AsInt()))
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
                        src.ClosureOfDroits.IsNullOrEmpty()
                            ? DateTime.MinValue.Date
                            : src.ClosureOfDroits.AsDateTime()))
                .ForMember(dest => dest.Depth,
                    opt => opt.MapFrom(src =>
                        src.GetDepth()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    src.ClosureOfDroits.AsDateTime() == null
                        ? DroitStatus.Received
                        : DroitStatus.Closed
                ))
                .ForMember(dest => dest.LegacyRemarks,
                    opt => opt.MapFrom(src =>
                        src.Remarks))
                .ForMember(dest => dest.LegacyFileReference,
                    opt => opt.MapFrom(src =>
                        src.FileRef))
                .ForMember(dest => dest.RecoveredFromLegacy,
                    opt => opt.MapFrom(src =>
                        src.RecoveredFrom))
                .ForMember(dest => dest.RecoveredFrom,
                    opt => opt.MapFrom(src =>
                        src.RecoveredFrom != null ? src.RecoveredFrom.AsRecoveredFromEnum() : null))
                .ForMember(dest => dest.WreckMaterials, opt => opt.Ignore());


        }
    }
}
