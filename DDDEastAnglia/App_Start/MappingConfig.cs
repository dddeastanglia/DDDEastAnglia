using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

using DDDEastAnglia.Models;

namespace DDDEastAnglia
{
    public class MappingConfig
    {
        public static void RegisterMaps()
        {
            Mapper.CreateMap<UserProfile, SpeakerDisplayModel>()
                .ForMember(dest => dest.GravatarUrl, opt => opt.MapFrom(src => src.GravitarUrl(50)));

            Mapper.CreateMap<Session, SessionDisplayModel>()
                  .ForMember(dest => dest.SessionTitle, opt => opt.MapFrom(src => src.Title))
                  .ForMember(dest => dest.SessionAbstract, opt => opt.MapFrom(src => src.Abstract));
        }
    }
}