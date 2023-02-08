namespace Api
{
    public class ListaSemRegistrosExcecao : BaseExcecao
    {
        public const string MENSAGEM_BASE = "A lista de {0} encontra-se sem registros para o filtro informado.";

        public ListaSemRegistrosExcecao(string nome) : base(string.Format(MENSAGEM_BASE, nome)) { }
    }
}
