using AutoMapper;
using System;
using ZavrsniTestDjordjeVojcanin.Models.DTO;

namespace ZavrsniTestDjordjeVojcanin.Models
{
    public class TelefonProfile : Profile
    {
        public TelefonProfile()
        {
            CreateMap<Telefon, TelefonDTO>();
        }
    }
}
