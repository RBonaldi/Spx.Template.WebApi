using System.Collections.Generic;
using System.Linq;

namespace Api
{
    public class ValidacaoFluentExcecao : BaseExcecao
    {
        public List<string> Erros { get; set; }

        public ValidacaoFluentExcecao(IEnumerable<string> erros) : base("")
        {
            Erros = erros.ToList();
        }
    }
}
