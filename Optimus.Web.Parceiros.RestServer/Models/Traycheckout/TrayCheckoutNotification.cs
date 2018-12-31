using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Optimus.Web.Parceiros.RestServer.Models.Traycheckout
{
    public class TrayCheckoutNotification
    {
        private string tokenTransaction;
        private string xmlResponse;
        private string dataResponse;
        private string resultError;
        private string orderStore;
        private string orderNumber;
        private string totalPaid;
        private string statusId;
        private string statusName;
        private string message;
        private string dataRequest;

        public TrayCheckouttransactionRet ConfirmaRecebimento(string token, out int statusret)
        {

            setTokenTransaction(token);

            return this.Notification(out statusret);
        }

        private void setTokenTransaction(string token_transaction)
        {
            tokenTransaction = token_transaction;
        }
        private List<KeyValuePair<string, string>> formData()
        {
            var list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("token_account", ConfigTrayCheckout.tokenAccount));
            list.Add(new KeyValuePair<string, string>("token_transaction", tokenTransaction));
            return list;
        }
        private void formatForm(List<KeyValuePair<string, string>> form)
        {
            dataRequest = string.Join(",", form.ToArray());
            dataRequest = Regex.Replace(dataRequest, @"[,]", "\n");
        }

        private TrayCheckouttransactionRet Notification(out int statusret)
        {
            TrayCheckouttransactionRet rett = new TrayCheckouttransactionRet();

            statusret = 200;
            var form = formData();
            var content = new FormUrlEncodedContent(form);
            //Método pode ser excluido, utilizado apenas para quebrar linha dos dados do form para melhor visualização
            formatForm(form);
            TrayCheckout tc = new TrayCheckout();
            tc.RequestTrayCheckout(content, ConfigTrayCheckout.UrlGetByToken());
            if (tc.getHttpStatusCode().Equals(HttpStatusCode.OK))
            {
                //Response.StatusCode = 200;
                XmlDocument xml = new XmlDocument();
                xmlResponse = tc.getContent();
                xml.LoadXml(xmlResponse);
                tc.setXml(xml);
                if (!tc.sucessResponse())
                {
                    message = tc.getMessage();
                    rett.lsterros.AddRange(tc.getGeneralError());
                    rett.lsterros.AddRange( tc.getValidationError());
                    statusret = 206;
                }
                else
                {
                    message = tc.getMessage();
                    XmlNodeList transaction = xml.GetElementsByTagName("transaction");
                    rett.order_number = transaction[0]["order_number"].InnerText;
                    rett.transaction_id = transaction[0]["transaction_id"].InnerText;
                    rett.status_id = transaction[0]["status_id"].InnerText;
                    rett.status_name = transaction[0]["status_name"].InnerText;
                    XmlNodeList payment = xml.GetElementsByTagName("payment");
                    rett.price_original= payment[0]["price_original"].InnerText;
                    rett.payment_response = payment[0]["payment_response"].InnerText;
                    rett.payment_split= payment[0]["split"].InnerText;
                    rett.payment_payment_method_id = payment[0]["payment_method_id"].InnerText;
                    rett.price_original = payment[0]["price_original"].InnerText;
                    rett.payment_payment_method_name = payment[0]["payment_method_name"].InnerText;
                }
            }
            return rett;
        }

        private void dataPayment(XmlNode nodePayment)
        {
            string price_payment = nodePayment["price_payment"].InnerText;
            string payment_response = nodePayment["payment_response"].InnerText;
            string url_payment = nodePayment["url_payment"].InnerText;
            string tid = nodePayment["tid"].InnerText;
            string split = nodePayment["split"].InnerText;
            string payment_method_id = nodePayment["payment_method_id"].InnerText;
            string payment_method_name = nodePayment["payment_method_name"].InnerText;
            string linha_digitavel = nodePayment["price_payment"].InnerText;
            dataResponse += "\nMethod: " + payment_method_id + " - " + payment_method_name + " Split: " + split + " Price: " + price_payment;
            dataResponse += "\nResponse: " + payment_response + " \nUrl Payment:" + url_payment;
        }
        private void dataTransaction(XmlNode nodeTransaction)
        {
            string order_number = nodeTransaction["order_number"].InnerText;
            string status_id = nodeTransaction["status_id"].InnerText;
            string status_name = nodeTransaction["status_name"].InnerText;
            string transaction_id = nodeTransaction["transaction_id"].InnerText;
            dataResponse = "\nPedido: " + order_number;
            dataResponse += "\nStatus: " + status_id + " - " + status_name;
            dataResponse += "\nId Transação: " + transaction_id;
            //UpdateOrderStatus(order_number, status_id, status_name);
        }
    }

    public class TrayCheckouttransactionRet
    {
        public TrayCheckouttransactionRet() 
        {
            this.lsterros = new List<TrayCheckouttransactionRetErro>();
        }
        public string order_number { get; set; }
        public string transaction_id { get; set; }
        public string status_name { get; set; }
        public string status_id { get; set; }
        public string token_transaction { get; set; }
        public string price_original { get; set; }
        public string price_payment { get; set; }
        public string payment_response { get; set; }
        public string payment_split { get; set; }
        public string payment_payment_method_id { get; set; }
        public string payment_payment_method_name { get; set; }

        public List<TrayCheckouttransactionRetErro> lsterros { get; set; }
    }
    public class TrayCheckouttransactionRetErro
    {
        public string code { get; set; }
        public string message { get; set; }

        public string field { get; set; }

        public string message_complete { get; set; }
    }

    /*
<response>
    <data_response>
        <transaction>
            <order_number>74907</order_number>
            <free nil="true"/>
            <transaction_id type="integer">74907</transaction_id>
            <status_name>Em Recuperação</status_name>
            <status_id type="integer">88</status_id>
            <token_transaction>bbe797d96d26b923a6de4035978a51bd</token_transaction>
            <payment>
                <price_original type="decimal">127.3</price_original>
                <price_payment type="decimal">136.96</price_payment>
                <payment_response>Não autorizado. Verifique os dados de Pagamento (g 1201  Ação não permitida, tente novamente!)</payment_response>
                <url_payment></url_payment>
                <tid></tid>
                <split type="integer">4</split>
                <payment_method_id type="integer">3</payment_method_id>
                <payment_method_name>Visa</payment_method_name>
                <linha_digitavel nil="true"/>
            </payment>
            <customer>
                <name>NILTON CESAR</name>
                <cpf>08318679733</cpf>
                <email>nilton.cesar.2014@hotmail.com</email>
                <company_name></company_name>
                <trade_name></trade_name>
                <cnpj></cnpj>
                <addresses type="array">
                    <address>
                        <street>AVENIDA A</street>
                        <number>114</number>
                        <neighborhood>VISTA ALEGRE</neighborhood>
                        <postal_code>27320040</postal_code>
                        <completion></completion>
                        <city>BARRA MANSA</city>
                        <state>RJ</state>
                    </address>
                </addresses>
                <contacts type="array">
                    <contact>
                        <value>3333333333</value>
                        <type_contact>W</type_contact>
                    </contact>
                </contacts>
            </customer>
        </transaction>
    </data_response>
    <message_response>
        <message>success</message>
    </message_response>
</response>     
     */
}