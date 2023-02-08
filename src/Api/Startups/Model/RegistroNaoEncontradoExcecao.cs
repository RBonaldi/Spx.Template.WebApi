namespace Api
{
    public class RegistroNaoEncontradoExcecao : BaseExcecao
    {
        public const string MENSAGEM_FILTROS_BASE = "Não foi encontrado registro de {0} com os filtros informados.";
        public const string MENSAGEM_ID_BASE = "Não foi encontrado registro de {0} com o id informado {1}.";

        public RegistroNaoEncontradoExcecao(string nome, int id) : base(string.Format(MENSAGEM_ID_BASE, nome, id)) { }
        public RegistroNaoEncontradoExcecao(string nome) : base(string.Format(MENSAGEM_FILTROS_BASE, nome)) { }

    }
}
