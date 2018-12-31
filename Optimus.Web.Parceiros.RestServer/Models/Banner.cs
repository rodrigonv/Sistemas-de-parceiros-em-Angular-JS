using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class Banner
    {
        public int cdbanner { get; set; }
        public string nmbanner { get; set; }
        public string dtinicio { get; set; }

        public string dtfim { get; set; }

        public int cdentifilial { get; set; }

        public bool stativo { get; set; }

        public string txurlimagem { get; set; }

        public string txurlredirect { get; set; }

    }
}