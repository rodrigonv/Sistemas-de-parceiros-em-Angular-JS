using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class Promocao
    {
        public string cdpromocao { get; set; }
        public string txurlimagem { get; set; }
        public string txurlredirect { get; set; }
        public string txdetalhes { get; set; }

        public string nmpromocao { get; set; }
    }
}