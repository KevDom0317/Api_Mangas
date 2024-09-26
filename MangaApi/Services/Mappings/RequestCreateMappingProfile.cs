using AutoMapper;
using MangaApi.Dtos;
using MangaApi.Models;

namespace MangaApi.Services.Mappings
{
    public class RequestCreateMappingProfile : Profile
    {
        public RequestCreateMappingProfile()
        {
            CreateMap<CreateMangaDTO, Manga>()
                .AfterMap((src, dest) => dest.PublicationDate = src.PublicationDate);
        }
    }
}
