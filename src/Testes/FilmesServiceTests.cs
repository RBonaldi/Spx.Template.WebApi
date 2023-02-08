using ExceptionsLibrary;
using Domain.Adapters;
using Domain.Models;
using Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests
{
    public class FilmesServiceTests
    {
        private readonly IFilmesService filmesService;
        private readonly Mock<ITmdbAdapter> tmdbAdapterMock;

        public FilmesServiceTests()
        {
            tmdbAdapterMock = new Mock<ITmdbAdapter>();
            filmesService = new FilmesService(
                tmdbAdapterMock.Object,
                new LoggerFactory());
        }

        [Fact]
        [Trait(nameof(IFilmesService.ObterFilmesAsync), "Sucesso")]
        public async Task ObterFilmesAsync_Sucesso()
        {
            // Objeto que sera utilizado para retorno do Mock
            var expected = new List<Filme>()
                {
                    new Filme()
                    {
                        Id = 10447,
                        Descricao = "descricao_teste",
                        Nome = "nome_teste"
                    }
                };

            tmdbAdapterMock
                .Setup(m => m.GetFilmesAsync(It.IsAny<Pesquisa>(), "pt-BR"))
                .ReturnsAsync(expected);

            var filmes = await filmesService.ObterFilmesAsync(new Pesquisa()
            {
                TermoPesquisa = "teste"
            });

            var exepectedSingle = expected.Single();

            Assert.Contains(filmes, f => 
                    f.Id == exepectedSingle.Id && 
                    f.Descricao == exepectedSingle.Descricao && 
                    f.Nome == exepectedSingle.Nome);
        }
    }
}
