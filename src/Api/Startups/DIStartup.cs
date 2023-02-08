using Application;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public static class DIStartup
    {
        public static void AddDICustomizado(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IFilmesService, FilmesService>();
        }
    }
}
