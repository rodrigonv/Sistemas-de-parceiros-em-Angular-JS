using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class Produto
    {
        public bool checado { get; set; }
        public string cdproduto { get; set; }
        public string cdprodparceiro { get; set; }
        public string nmproduto { get; set; }
        public string estoque { get; set; }
        public string cdentifilial { get; set; }
        public string cdprodlegado { get; set; }
        public string preco { get; set; }
        public string precoview { get; set; }

        public string codforn { get; set; }
        public double peso { get; set; }
        public Produto()
        {
            this.checado = false;
            this.cdprodparceiro = "";
        }

    }
    public class produtosearch
    {
        public string nome { get; set; }
        public string codigoproduto { get; set; }

        public string codforn { get; set; }

        public string codigolegado { get; set; }
    }
    
}