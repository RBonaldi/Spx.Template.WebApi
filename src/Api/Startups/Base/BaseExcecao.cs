using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Api
{
    [Serializable]
    public abstract class BaseExcecao : Exception
    {
        #region Construtores

        protected BaseExcecao(string mensagem) : base(mensagem) { }

        protected BaseExcecao(SerializationInfo informacoes, StreamingContext contexto)
            : base(informacoes, contexto) { }

        #endregion

        #region Overrides

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo informacoes, StreamingContext contexto)
        {
            if (informacoes == null)
                throw new ArgumentNullException("informacoes");

            base.GetObjectData(informacoes, contexto);
        }

        #endregion
    }
}
