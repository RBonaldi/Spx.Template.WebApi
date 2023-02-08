using Api.Dtos;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using static Cor.IntegracaoImdb.Api.TmdbAdapter.AutoMapperTmdb;

namespace Api
{
    public static class AutoMapperApiStartup
    {
        public class WebApiMapperProfile : Profile
        {
            public WebApiMapperProfile()
            {
                CreateMap<Filme, FilmesGetResult>();
                CreateMap<FilmesGet, Pesquisa>();
            }
        }
        public static void AddAutoMapperCustomizadoApi(this IServiceCollection servicos)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AllowNullDestinationValues = true;
                mc.AllowNullCollections = true;
                mc.AddProfile(new WebApiMapperProfile());
                mc.AddProfile(new TmdbMapperProfile());
        });

            var mapper = mappingConfig.CreateMapper();
            servicos.AddSingleton(mapper);
        }
    }
}