using AutoMapper;
using Cor.IntegracaoImdb.Api.TmdbAdapter.Clients;
using Domain.Adapters;
using Domain.Exceptions;
using Domain.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cor.IntegracaoImdb.Api.TmdbAdapter
{
    internal class TmdbAdapter : ITmdbAdapter
    {
        private readonly ITmdbApi tmdbApi;
        private readonly TmdbAdapterConfiguration tmdbAdapterConfiguration;
        private readonly IMapper mapper;

        public TmdbAdapter(ITmdbApi tmdbApi,
            TmdbAdapterConfiguration tmdbAdapterConfiguration,
            IMapper mapper)
        {
            this.tmdbApi = tmdbApi ??
                throw new ArgumentNullException(nameof(tmdbApi));

            this.tmdbAdapterConfiguration = tmdbAdapterConfiguration ??
                throw new ArgumentNullException(nameof(tmdbAdapterConfiguration));

            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<Filme>> GetFilmesAsync(
            Pesquisa pesquisa, string idioma)
        {
            try
            {
                var tmdbSearchMoviesGet =
                    mapper.Map<TmdbSearchMoviesGet>(pesquisa);

                tmdbSearchMoviesGet.ApiKey =
                    tmdbAdapterConfiguration.TmdbApiKey;

                tmdbSearchMoviesGet.Language = idioma;

                var tmdbSearchMoviesGetResult = await tmdbApi
                    .SearchMovies(tmdbSearchMoviesGet);

                return mapper
                    .Map<IEnumerable<Filme>>(tmdbSearchMoviesGetResult.Results);
            }
            catch (ApiException e)
            {
                switch (e.StatusCode)
                {
                    case (HttpStatusCode)429: // TooManyRequests
                        throw new BuscarFilmesCoreException(
                            BuscarFilmesCoreError.LimiteDeRequisicoesAtingido);
                }

                // Qualquer outro codigo de retorno esta sendo considerado como
                // uma situacao nao prevista.  A excecao sera relancada e caso
                // nao tratada, acarretara em um erro interno. 
                // Obs.: Deixar essa excecao sem tratamento, a principio nao eh
                // errado, pois eh uma condicao nao prevista, ou seja,
                // desconhecida. Como este projeto implementa um ponto central
                // de tratamento de erros (por meio das bibliotecas
                // Otc.ExceptionHandler e Otc.Mvc.Filters) este erro sera
                // devidamente registrado (logs) e um identificador do registro
                // sera fornecido na resposta. Note que em ambientes de
                // desenvolvimento, (variavel de ambiente ASPNETCORE_ENVIRONMENT
                // definida como Development) a excecao sera exposta na resposta,
                // no entanto, em ambientes produtivos,
                // apenas o identificador do log do erro sera fornecido.
                throw;
            }
        }
    }
}
