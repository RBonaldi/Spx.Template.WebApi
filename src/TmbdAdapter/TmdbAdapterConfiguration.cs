namespace Cor.IntegracaoImdb.Api.TmdbAdapter
{
    public class TmdbAdapterConfiguration 
    {
        public string TmdbApiUrlBase { get; set; }
        public string TmdbApiKey { get; set; }
        public int TempoDeCacheDaPesquisaEmSegundos { get; set; } = 20;
        public string Idioma { get; set; }
    }
}
