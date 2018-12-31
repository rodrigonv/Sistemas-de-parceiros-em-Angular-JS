using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class PaypalFinalize
    {
        public string payerid { get; set; }

        public string tokentransacao { get; set; }
        public string shiptoname { get; set; }
        public string shiptostreet { get; set; }
        public string shiptostreet2 { get; set; }
        public string shiptocity { get; set; }
        public string shiptostate { get; set; }
        public string shiptozip { get; set; }

        public string statusakc { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public string email { get; set; }
    }
}