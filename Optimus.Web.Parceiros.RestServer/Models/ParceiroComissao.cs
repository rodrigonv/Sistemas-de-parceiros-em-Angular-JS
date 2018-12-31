using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class ParceiroComissao
    {
        public double comissao { get; set; }

        public string dtinicio { get; set; }
        public string dtfim { get; set; }
            /*
            
             */
        public string dtpedido { get; set; }
        public double vrtotal { get; set; }
        public string cdfunc { get; set; }

        public int cdentidadepai { get; set; }

        public int cdstatuspedido { get; set; }
        public string func { get; set; }
    }

    public class ParceiroSearch
    {
        public double cdentidadepai { get; set; }
        public double cdfunc { get; set; }
        public string dtinicio { get; set; }
        public string dtfim { get; set; }
     }
}