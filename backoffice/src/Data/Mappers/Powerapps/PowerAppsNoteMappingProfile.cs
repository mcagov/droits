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
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.NoteText??string.Empty))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Subject??"Note"))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dest => dest.LastModifiedByUserId, opt => opt.MapFrom(src => default(Guid?)));

        }
    }
}