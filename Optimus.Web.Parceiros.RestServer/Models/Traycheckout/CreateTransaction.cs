using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Optimus.Web.Parceiros.RestServer.Models.Traycheckout
{
    public class CreateTransaction
    {
        private string xmlResponse;
        private string dataResponse;
        private string resultError;
        private string dataRequest;
        private string message;
        private string methodPayment;
        private string linkPayment;
        const string PAYMENT_CARD = "card";
        const string PAYMENT_TRANSFER = "transfer";
        const string PAYMENT_BANK_SLIP = "bank_slip";

        private List<KeyValuePair<string, string>> formData(TrayCheckoutPayment tcp, string codpagamento, string opttoken)
        {
            Random rand = new Random();


            methodPayment = PAYMENT_CARD;
            var list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("token_account", ConfigTrayCheckout.tokenAccount));
            list.Add(new KeyValuePair<string, string>("transaction[url_notification]", string.Format(ConfigTrayCheckout.urlNotification, codpagamento, opttoken)));
            list.Add(new KeyValuePair<string, string>("transaction[order_number]", tcp.transaction_order_number));
            /*
             
                shipping_type	Texto / 15	Tipo de envio
                shipping_price	Decimal / Ex: 0.00	Valor do envio
             */
            //frete
            if (tcp.shipping_price > 0 )
            {
                list.Add(new KeyValuePair<string, string>("transaction[shipping_type]", tcp.shipping_type));
                list.Add(new KeyValuePair<string, string>("transaction[shipping_price]", tcp.shipping_price.ToString().Replace(',', '.')));
            }


            list.Add(new KeyValuePair<string, string>("customer[name]", tcp.customer_name));
            list.Add(new KeyValuePair<string, string>("customer[cpf]", tcp.customer_cpf));
            if (!string.IsNullOrEmpty(tcp.customer_email))
            {
                list.Add(new KeyValuePair<string, string>("customer[email]", tcp.customer_email));
            }


            list.Add(new KeyValuePair<string, string>("customer[contacts][][number_contact]", tcp.customer_contacts_number_contact));
            list.Add(new KeyValuePair<string, string>("customer[contacts][][type_contact]", tcp.customer_contacts_type_contact));
            /*type_contact
            Residencial	H
            Celular	M
            Comercial	W
             */
            list.Add(new KeyValuePair<string, string>("customer[addresses][][type_address]", tcp.customer_addresses_type_address));
            /*type_address
            Cobrança	B
            Entrega	D
             */
            list.Add(new KeyValuePair<string, string>("customer[addresses][][postal_code]", tcp.customer_addresses_postal_code));

            list.Add(new KeyValuePair<string, string>("customer[addresses][][street]", tcp.customer_addresses_street));
            list.Add(new KeyValuePair<string, string>("customer[addresses][][number]", tcp.customer_addresses_number));
            list.Add(new KeyValuePair<string, string>("customer[addresses][][neighborhood]", tcp.customer_addresses_neighborhood));
            list.Add(new KeyValuePair<string, string>("customer[addresses][][city]", tcp.customer_addresses_city));
            list.Add(new KeyValuePair<string, string>("customer[addresses][][state]", tcp.customer_addresses_state));


            //string price = "";
            // Consulte todas formas de pagamento em: http://wiki.traycheckout.com.br/index.php/Integra%C3%A7%C3%A3o#Tabelas_Auxiliares

            if (tcp.payment_payment_method_id == "6")//boleto
            {
                list.Add(new KeyValuePair<string, string>("payment[payment_method_id]", "6"));
                list.Add(new KeyValuePair<string, string>("payment[split]", "1"));
            }
            else
            {
                /*
                   Diners Club	2
                   Visa	3
                   Mastercard	4
                   American Express	5
                   Discover	15
                   Elo	16
                   Aura	18
                   JCB	19
                   Hipercard	20
                   Hiper (Itaú)	25
                */
                list.Add(new KeyValuePair<string, string>("payment[payment_method_id]", tcp.payment_payment_method_id));
                list.Add(new KeyValuePair<string, string>("payment[split]", tcp.payment_split));
                list.Add(new KeyValuePair<string, string>("payment[card_name]", tcp.payment_card_name));
                list.Add(new KeyValuePair<string, string>("payment[card_number]", tcp.payment_card_number));
                list.Add(new KeyValuePair<string, string>("payment[card_expdate_month]", tcp.payment_card_expdate_month));
                list.Add(new KeyValuePair<string, string>("payment[card_expdate_year]", tcp.payment_card_expdate_year));
                list.Add(new KeyValuePair<string, string>("payment[card_cvv]", tcp.payment_card_cvv));
            }
            //else if (methodPayment.Equals(PAYMENT_TRANSFER))
            //{
            //    price = "185.50"; //desconto para pagamento com transferência
            //    list.Add(new KeyValuePair<string, string>("payment[payment_method_id]", "7"));
            //    list.Add(new KeyValuePair<string, string>("payment[split]", "1"));
            //}
            //else if (methodPayment.Equals(PAYMENT_BANK_SLIP))
            //{
            //    price = "180.10"; //desconto para pagamento com boleto
            //    list.Add(new KeyValuePair<string, string>("payment[payment_method_id]", "6"));
            //    list.Add(new KeyValuePair<string, string>("payment[split]", "1"));
            //}

            for (int i = 0; i < tcp.Products.Count; i++)
            {
                list.Add(new KeyValuePair<string, string>("transaction_product[][description]", tcp.Products[i].description));
                list.Add(new KeyValuePair<string, string>("transaction_product[][quantity]", tcp.Products[i].quantity.ToString()));
                list.Add(new KeyValuePair<string, string>("transaction_product[][price_unit]", tcp.Products[i].price_unit.ToString().Replace(',', '.')));
                list.Add(new KeyValuePair<string, string>("transaction_product[][sku_code]", "AF" + tcp.Products[i].sku_code));
                list.Add(new KeyValuePair<string, string>("transaction_product[][code]", tcp.Products[i].sku_code));

            }



            return list;
        }

        private void formatForm(List<KeyValuePair<string, string>> form)
        {
            dataRequest = string.Join(",", form.ToArray());
            dataRequest = Regex.Replace(dataRequest, @"[,]", "\n");
        }

        public TrayCheckoutPaymentResponse CriarTransacao(TrayCheckoutPayment tcp, string codpagamento, string opttoken, out string formpost, out string urltrans, out string qstring)
        {
            TrayCheckoutPaymentResponse tcpret = new TrayCheckoutPaymentResponse();

            var form = formData(tcp, codpagamento, opttoken);
            var content = new FormUrlEncodedContent(form);
            formpost = string.Empty;
            foreach (KeyValuePair<string, string> item in form)
            {
                formpost += item.Key + " " + item.Value + Environment.NewLine;
            }

            qstring = getqueryparam(form);
            //Método pode ser excluido, utilizado apenas para quebrar linha dos dados do form para melhor visualização
            //formatForm(form);
            TrayCheckout tc = new TrayCheckout();
            urltrans = ConfigTrayCheckout.UrlCreateTransaction();
            tc.RequestTrayCheckout(content, string.Format(ConfigTrayCheckout.UrlCreateTransaction(), codpagamento, opttoken));
            if (tc.getHttpStatusCode().Equals(HttpStatusCode.OK))
            {
                XmlDocument xml = new XmlDocument();
                xmlResponse = tc.getContent();
                xml.LoadXml(xmlResponse);
                tc.setXml(xml);
                tcpret = this.BuildRequest(tc);
            }
            return tcpret;
        }

        public string getqueryparam(List<KeyValuePair<string, string>> form) 
        {
            string ret = string.Empty;


            string URLAuth = "https://api.sandbox.traycheckout.com.br/v2/transactions/pay_complete";

            NameValueCollection queryParameters = new NameValueCollection();

            foreach (var item in form)
            {
                queryParameters.Add(item.Key, item.Value);
            }

            List<string> items = new List<string>();

            foreach (String name in queryParameters)
                items.Add(String.Concat(name, "=", System.Web.HttpUtility.UrlEncode(queryParameters[name])));

            string postString = String.Join("&", items.ToArray());

            ret = postString;

            //const string contentType = "application/x-www-form-urlencoded";
            //System.Net.ServicePointManager.Expect100Continue = false;

            //CookieContainer cookies = new CookieContainer();
            //HttpWebRequest webRequest = WebRequest.Create(URLAuth) as HttpWebRequest;
            //webRequest.Method = "POST";
            //webRequest.ContentType = contentType;
            //webRequest.CookieContainer = cookies;
            //webRequest.ContentLength = postString.Length;
            //webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            //StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
            //requestWriter.Write(postString);
            //requestWriter.Close();

            //StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
            //string responseData = responseReader.ReadToEnd();

            //responseReader.Close();
            //webRequest.GetResponse().Close();



            return ret;

        
        }
        private TrayCheckoutPaymentResponse BuildRequest(TrayCheckout tc)
        {
            TrayCheckoutPaymentResponse tcpret = new TrayCheckoutPaymentResponse();

            tcpret.sucessResponse = tc.sucessResponse();

            XmlDocument xmlDocument = tc.getXml();

            if (!tc.sucessResponse())//erro
            {
                /* Generic Errors */
                XmlNodeList generalErrorList = xmlDocument.GetElementsByTagName("general_error");
                foreach (XmlNode pm in generalErrorList)
                {
                    validation_error vr = new validation_error();
                    vr.code = pm["code"].InnerText;
                    vr.message = pm["message"].InnerText;
                    tcpret.validation_errors.Add(vr);
                }

                XmlNodeList errorList = xmlDocument.GetElementsByTagName("error");
                foreach (XmlNode pm in errorList)
                {
                    validation_error vr = new validation_error();
                    vr.code = pm["code"].InnerText;
                    vr.message = pm["message"].InnerText;
                    tcpret.validation_errors.Add(vr);
                }

                XmlNodeList validationerrorList = xmlDocument.GetElementsByTagName("validation_error");
                foreach (XmlNode pm in validationerrorList)
                {
                    validation_error vr = new validation_error();
                    vr.code = pm["code"].InnerText;
                    vr.message = pm["message"].InnerText;
                    vr.field = pm["field"].InnerText;
                    vr.message_complete = pm["message_complete"].InnerText;
                    tcpret.validation_errors.Add(vr);
                }


            }
            else//ok
            {

                XmlNodeList transaction = xmlDocument.GetElementsByTagName("transaction");

                tcpret.order_number = transaction[0]["order_number"].InnerText;
                tcpret.status_id = transaction[0]["status_id"].InnerText;
                tcpret.status_name = transaction[0]["status_name"].InnerText;
                tcpret.transaction_id = transaction[0]["transaction_id"].InnerText;
                tcpret.token_transaction = transaction[0]["token_transaction"].InnerText;

                XmlNodeList payment = xmlDocument.GetElementsByTagName("payment");

                tcpret.price_payment = payment[0]["price_payment"].InnerText;
                tcpret.payment_response = payment[0]["payment_response"].InnerText;
                tcpret.url_payment = payment[0]["url_payment"].InnerText;
                tcpret.tid = payment[0]["tid"].InnerText;
                tcpret.split = payment[0]["split"].InnerText;
                tcpret.payment_method_id = payment[0]["payment_method_id"].InnerText;
                tcpret.payment_method_name = payment[0]["payment_method_name"].InnerText;
                tcpret.linha_digitavel = payment[0]["price_payment"].InnerText;
            }

            return tcpret;
        }
    }
}