using System;

namespace Projeto.Models
{
    public class CotacaoViewModel
    {
        public int ID { get; set; }
        public string DATA_SOLICITACAO { get; set; }
        public string IND_STATUS { get; set; }
        public string DATA_ALTERACAO { get; set; }
        public string COTACAO { get; set; }
        public int ID_PESSOA { get; set; }
    }
}
