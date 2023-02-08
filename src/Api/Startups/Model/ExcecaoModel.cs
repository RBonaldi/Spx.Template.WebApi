using System.Collections.Generic;

namespace Api
{
    public class ExcecaoModel
    {
        public ExcecaoModel()
        {
            Mensagens = new List<string>();
            CodigoErro = new List<string>();
        }

        public int HttpStatus { get; set; }
        public List<string> Mensagens { get; set; }
        public List<string> CodigoErro { get; set; }
        public string Detalhes { get; set; }
    }
}
