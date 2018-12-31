using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models.Traycheckout
{
    public class TrayCheckoutPost
    {

        public string cdforn { get; set; }
        public string cdcesta { get; set; }
        public string order_number { get; set; }
        public int split { get; set; }
        public string card_name { get; set; }
        public string card_number { get; set; }
        public string card_expdate_month { get; set; }
        public string card_expdate_year { get; set; }
        public string card_cvv { get; set; }
        public string payment_method_id { get; set; }

        public string cdcontato { get; set; }
    }

    public class TrayCheckoutNotificacao 
    {
        public string token { get; set; }

        public string token_transaction { get; set; }
    }
}