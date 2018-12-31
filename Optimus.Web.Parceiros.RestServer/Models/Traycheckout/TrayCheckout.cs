using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Xml;

namespace Optimus.Web.Parceiros.RestServer.Models.Traycheckout
{
    public class TrayCheckout
    {
        private string message;
        private XmlDocument xmlDocument;
        private HttpStatusCode httpStatusCode;
        private string xmlResponse;
        private string generalError;
        private string validationError;


        public void setMessage(string msg)
        {
            message = msg;
        }

        public string getMessage()
        {
            return message;
        }

        public void setXml(XmlDocument x)
        {
            xmlDocument = x;
        }

        public XmlDocument getXml()
        {
            return xmlDocument;
        }

        private void setContent(string content)
        {
            xmlResponse = content;
        }

        public string getContent()
        {
            return xmlResponse;
        }

        private void setHttpCodeStatus(HttpStatusCode code)
        {
            httpStatusCode = code;
        }

        public HttpStatusCode getHttpStatusCode()
        {
            return httpStatusCode;
        }

        public bool sucessResponse()
        {
            XmlNodeList message_response = xmlDocument.GetElementsByTagName("message_response");
            XmlNode nodeMessage = message_response[0];
            setMessage(nodeMessage["message"].InnerText);
            if (getMessage().Equals("error"))
            {
                return false;
            }
            return true;
        }

        public List<TrayCheckouttransactionRetErro> getGeneralError()
        {
            generalError = "";
            TrayCheckouttransactionRetErro err = new TrayCheckouttransactionRetErro();
            List<TrayCheckouttransactionRetErro> lstret = new List<TrayCheckouttransactionRetErro>();
            /* Generic Errors */
            XmlNodeList generalErrorList = xmlDocument.GetElementsByTagName("general_error");
            foreach (XmlNode pm in generalErrorList)
            {
                err.code = pm["code"].InnerText;
                err.message = pm["message"].InnerText;
                lstret.Add(err);
            }

            XmlNodeList errorList = xmlDocument.GetElementsByTagName("error");
            foreach (XmlNode pm in errorList)
            {
                err.code = pm["code"].InnerText;
                err.message = pm["message"].InnerText;
                lstret.Add(err);
            }
            return lstret;
        }

        public List<TrayCheckouttransactionRetErro>  getValidationError()
        {
            TrayCheckouttransactionRetErro err = new TrayCheckouttransactionRetErro();
            List<TrayCheckouttransactionRetErro> lstret = new List<TrayCheckouttransactionRetErro>();
            XmlNodeList validationErrorList = xmlDocument.GetElementsByTagName("validation_error");
            foreach (XmlNode pm in validationErrorList)
            {
                string code = pm["code"].InnerText;
                string msg = pm["message"].InnerText;
                string field = pm["field"].InnerText;
                string messageComplete = pm["message_complete"].InnerText;
                err.code = pm["code"].InnerText;
                err.message = pm["message"].InnerText;
                err.field = pm["field"].InnerText;
                err.message_complete = pm["message_complete"].InnerText;
                lstret.Add(err);
            }
            return lstret;
        }

        public void RequestTrayCheckout(FormUrlEncodedContent content, string url)
        {
            setContent("");
            setHttpCodeStatus(HttpStatusCode.NoContent);
            using (var client = new HttpClient())
            {
                try
                {
                    var result = client.PostAsync(url, content).Result;
                    setHttpCodeStatus(result.StatusCode);
                    if (!result.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        generalError += "\nRequisição inválida. Http code: " + result.StatusCode;
                        return;
                    }
                    string xmlResponse = result.Content.ReadAsStringAsync().Result;
                    setContent(xmlResponse);
                }
                catch (HttpRequestException e)
                {
                    setHttpCodeStatus(HttpStatusCode.NotFound);
                    generalError += "\nRequisição inválida. Erro:" + e;
                }
            }
        }
    }
}