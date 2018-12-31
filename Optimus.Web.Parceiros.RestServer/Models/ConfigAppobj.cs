using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    [Serializable]
    public class ConfigAppobj
    {
        public ConfigAppobj()
        {
            this.opttelefones = new List<telefone>();
            this.filial = new FilialReturn();
        }
        public string optpkgname { get; set; }
        public string opttoken { get; set; }
        public string optfilial { get; set; }
        public string optcodapp { get; set; }
        public string optcsstopo { get; set; }
        public string optcssfundotopo { get; set; }
        public string opturllogo { get; set; }
        public string opttxhoraatendimento { get; set; }
        public string optemailcontato { get; set; }
        public List<telefone> opttelefones { get; set; }
        public string optdadosempresa { get; set; }
        public string optsenderidios { get; set; }
        public string optsenderidandroid { get; set; }

        public FilialReturn filial { get; set; }

    }
    [Serializable]
    public class telefone
    {
        public telefone(string _phone)
        {
            this.phone = _phone;
        }
        public string phone { get; set; }
    }
    public class FilialReturn
    {
        public string NmRazao { get; set; }
        public string cnpj { get; set; }

        public string logradouro { get; set; }
        public string numero { get; set; }

        public string complemento { get; set; }

        public string bairro { get; set; }

        public string uf { get; set; }

        public string cep { get; set; }

        public string cidade { get; set; }
    }


 
}