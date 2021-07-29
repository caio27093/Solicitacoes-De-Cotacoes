using System;

namespace Projeto.Models
{
    public class CadLoginViewModel
    {
        public string NOME { get; set; }
        public string EMAIL { get; set; }
        public string TELEFONE { get; set; }
        public string SENHA { get; set; }
        public string CONFIRMA_SENHA { get; set; }
        public string TEXTO_CONFIRMA { get; set; }
        public int TIPO_USER { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string DATANASC { get; set; }
        public string NUMCOVENIO { get; set; }
    }
}
