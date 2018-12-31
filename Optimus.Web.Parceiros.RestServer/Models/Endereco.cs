using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class Endereco
    {
        public int cdendereco { get; set; }
        public int cdcontato { get; set; }
        public string cduf { get; set; }
        public string cdcep { get; set; }
        public string txcidade { get; set; }
        public string txlogradouro { get; set; }
        public string txbairro { get; set; }
        public string txnumero { get; set; }
        public string txcomplemento { get; set; }
        public int cdmunicipioibge { get; set; }
        public int cdpais { get; set; }
    }
}