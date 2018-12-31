using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class UploadParceiro
    {
        public int CdUploadParceiro { get; set; }

        public int Cdfornecedor { get; set; }

        public string txinfo { get; set; }

        public string txpath { get; set; }

        public int cdentifilial { get; set; }

        public int stexcluido { get; set; }
        public string nmfornecedor { get; set; }

        public string dtinclusao { get; set; }
    }

    public class UploadParceiroPesquisa 
    {
        public string dtinicio { get; set; }

        public string dtfim { get; set; }
    }
}