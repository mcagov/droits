using AutoMapper;
using Droits.Models.Entities;
using System;
using Droits.Helpers;
using Droits.Models.DTOs.Powerapps;

namespace Droits.Data.Mappers.Powerapps
{
    public class PowerAppsNoteMappingProfile : Profile
    {
        public PowerAppsNoteMappingProfile()
        {
            CreateMap<PowerappsNoteDto, Note>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.NoteText)?string.Empty:src.NoteText.Replace("src=\"/api/data", "src=\"https://reportwreckmaterial.crm11.dynamics.com/api/data")))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Subject??"Note"))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dest => dest.LastModifiedByUserId, opt => opt.MapFrom(src => default(Guid?)));

        }
    }
}