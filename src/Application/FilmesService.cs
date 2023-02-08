using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Adapters;
using Domain.Models;
using Domain.Services;
using Microsoft.Extensions.Logging;

namespace Application
{
    public class FilmesService : IFilmesService
    {
        private readonly ITmdbAdapter tmdbAdapter;
        private readonly ILogger logger;

        public FilmesService(ITmdbAdapter tmdbAdapter,
            ILoggerFactory loggerFactory)
        {
            this.tmdbAdapter = tmdbAdapter ??
                throw new ArgumentNullException(nameof(tmdbAdapter));

            logger = loggerFactory?.CreateLogger<FilmesService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<IEnumerable<Filme>> ObterFilmesAsync(
            Pesquisa pesquisa)
        {
            logger.LogInformation("Realizando chamada ao TMDb com os seguintes " +
                "criterios de pesquisa: {@CriteriosPesquisa}",
                new { Criterios = pesquisa });

            IEnumerable<Filme> resultado = await tmdbAdapter
                .GetFilmesAsync(pesquisa, "pt-BR");

            logger.LogInformation("Chamada ao TMDb concluida com sucesso.");

            return resultado;
        }
    }
}
