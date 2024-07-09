
using AutoMapper;
using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;
using MagicVilla_API.Modelos.DTO;

namespace MagicVilla_API
{
    public class MapingConfig: Profile
    {
        public MapingConfig()
        {
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();

            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();
        }
    }
}
