using Cor.IntegracaoImdb.Api.TmdbAdapter;
using Cor.IntegracaoImdb.Api.TmdbAdapter.Clients;
using Domain.Adapters;
using Refit;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TmdbAdapterServiceCollectionExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddTmdbAdapter(
            this IServiceCollection services,
            TmdbAdapterConfiguration tmdbAdapterConfiguration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (tmdbAdapterConfiguration == null)
            {
                throw new ArgumentNullException(nameof(tmdbAdapterConfiguration));
            }

            // Registra a instancia do objeto de configuracoes desta camanda.
            services.AddSingleton(tmdbAdapterConfiguration);

            // Configura os parametros para chamada na TMDb API e registra a
            // interface ITmdbApi.
            services.AddScoped(serviceProvider =>
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress =
                    new Uri(tmdbAdapterConfiguration.TmdbApiUrlBase);

                return RestService.For<ITmdbApi>(httpClient);
            });

            // Registra a implementacao do ITmdbAdapter para ser utilizado na
            // camada de aplicacao.
            services.AddScoped<ITmdbAdapter, TmdbAdapter>();

            return services;
        }
    }
}
