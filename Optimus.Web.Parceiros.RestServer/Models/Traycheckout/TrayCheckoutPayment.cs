using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models.Traycheckout
{
    public class TrayCheckoutPayment
    {

        public TrayCheckoutPayment()
        {
            this.Products = new List<transaction_product>();
        }
        public string token_account { get; set; }
        public string transaction_url_notification { get; set; }
        public string transaction_order_number { get; set; }
        public string shipping_type { get; set; }

        /// <summary>
        /// Decimal / Ex: 0.00	Valor do envio
        /// </summary>
        public double shipping_price { get; set; }

        public string customer_name { get; set; }
        public string customer_cpf { get; set; }
        public string customer_email { get; set; }
        public string customer_contacts_number_contact { get; set; }
        public string customer_contacts_type_contact { get; set; }
        public string customer_addresses_type_address { get; set; }
        public string customer_addresses_postal_code { get; set; }
        public string customer_addresses_street { get; set; }
        public string customer_addresses_number { get; set; }
        public string customer_addresses_neighborhood { get; set; }
        public string customer_addresses_city { get; set; }
        public string customer_addresses_state { get; set; }

        public string payment_payment_method_id { get; set; }
        public string payment_split { get; set; }
        public string payment_card_name { get; set; }
        public string payment_card_number { get; set; }
        public string payment_card_expdate_month { get; set; }
        public string payment_card_expdate_year { get; set; }
        public string payment_card_cvv { get; set; }

        public List<transaction_product> Products { get; set; }
    }

    public class transaction_product
    {
        public string description { get; set; }
        public int quantity { get; set; }
        public double price_unit { get; set; }

        public string sku_code { get; set; }
    }

    public class TrayCheckoutPaymentResponse
    {
        public TrayCheckoutPaymentResponse() { this.validation_errors = new List<validation_error>(); }

        public List<validation_error> validation_errors { get; set; }
        public string price_payment { get; set; }
        public string payment_response { get; set; }
        public string url_payment { get; set; }
        public string tid { get; set; }
        public string split { get; set; }
        public string payment_method_id { get; set; }
        public string payment_method_name { get; set; }
        public string linha_digitavel { get; set; }

        public bool sucessResponse { get; set; }

        public string order_number { get; set; }
        public string status_id { get; set; }
        public string status_name { get; set; }
        public string transaction_id { get; set; }
        public string token_transaction { get; set; }

        public string codpagamento { get; set; }
    }

    public class validation_error
    {
        public string code { get; set; }
        public string message { get; set; }
        public string field { get; set; }
        public string message_complete { get; set; }
    }


}