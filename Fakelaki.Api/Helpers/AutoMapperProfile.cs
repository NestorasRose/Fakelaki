﻿using AutoMapper;
using Fakelaki.Api.Lib.Models;
using Fakelaki.Api.Models;

namespace Fakelaki.Api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<Event, EventModel>().ReverseMap();
            CreateMap<Lib.Models.Fakelaki, FakelakiModel>().ReverseMap();
        }
    }
}
