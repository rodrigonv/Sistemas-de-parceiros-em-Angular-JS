using Newtonsoft.Json;
using Optimus.Web.Parceiros.RestServer.Database;
using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Models.Traycheckout;
using Optimus.Web.Parceiros.RestServer.Util;
using Oracle.DataAccess.Client;
using RestSharp;
using RJS.Optimus.Biblioteca;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;
namespace Optimus.Web.Parceiros.RestServer.Controllers
{
    [EnableCors("*", "*", "GET, POST")]
    [RoutePrefix("cesta")]
    public class CestaController : ApiController
    {
        [AcceptVerbs("POST")]
        [Route("")]
        public HttpResponseMessage CestaInsert(HttpRequestMessage request, [FromBody] cestapost cesta)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<cesta> ret = new List<cesta>();
            try
            {
                ret = Database.CestaADO.GravaCesta(cf.cdentifilial, cesta.codped, cesta.cliente, cesta.produ, cesta.finaliza, cesta.codforn, cesta.codend, cesta.preco, cesta.subsquant, cesta.codforn, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "Grava cesta : " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
        [AcceptVerbs("GET")]
        [Route("criarcesta/{cdpedido}")]
        public HttpResponseMessage CestaCriar(string cdpedido)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<cesta> ret = new List<cesta>();
            try
            {
                ret = Database.CestaADO.CriarCesta(cdpedido, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "CestaCriar : " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }



        [AcceptVerbs("GET")]
        [Route("{cdcesta}/{cdforn}/")]
        public HttpResponseMessage CestaGet(string cdcesta, string cdforn)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<cesta> ret = new List<cesta>();
            try
            {
                ret = Database.CestaADO.RetornaCesta(cdcesta, cdforn, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "Retorna cesta : " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
        [AcceptVerbs("POST")]
        [Route("pedido")]
        public HttpResponseMessage updatePedido(HttpRequestMessage request, [FromBody] PedidoUpdateFrete ped)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            string ret = string.Empty;
            try
            {
                ret = Database.PedidosADO.AtualizaFrete(ped.cdpedido, ped.vrfrete, ped.txtipofrete, ped.tokenaxado, cf.cdentifilial, ped.cdenderecoentregra, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "updatePedido: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "ok");
        }

        [AcceptVerbs("GET")]
        [Route("split/{cdpedido}/{idbandeira}")]
        public HttpResponseMessage RetornaValorDividido(string cdpedido, int idbandeira)
        {
            List<splitListReturn> splitRet = new List<splitListReturn>();

            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {
                double vrtotalpedido = PedidosADO.RetornaValorTotalPedido(cdpedido, cf.datasource, cf.schema);
                var client = new RestClient(ConfigTrayCheckout.UrlTransactionRest());

                var request = new RestRequest("/simulate_splitting/", Method.POST);
                request.AddQueryParameter("token_account", ConfigTrayCheckout.tokenAccount);
                request.AddQueryParameter("price", vrtotalpedido.ToString().Replace(",", "."));
                // Json to post.
                //string jsonToSend = JsonHelper.ToJson(json);

                //request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                XmlDocument xmlDocument = new XmlDocument();
                try
                {
                    IRestResponse response = client.Execute(request);
                    string xmlResponse = response.Content;
                    xmlDocument.LoadXml(xmlResponse);

                }
                catch (Exception error)
                {
                    throw error;
                }
                XmlNodeList payment_method = xmlDocument.GetElementsByTagName("payment_method");

                splitListReturn splr = null;

                foreach (XmlNode pms in payment_method)
                {
                    splr = new splitListReturn();

                    XmlElement payment_methodElemento = (XmlElement)pms;

                    splr.payment_method_id = Convert.ToInt32(pms["payment_method_id"].InnerText);
                    splr.payment_method_name = pms["payment_method_name"].InnerText;

                    double price_customer = Convert.ToDouble(pms["price_customer"].InnerText.Replace(".", ","));

                    XmlNodeList splittings = payment_methodElemento.GetElementsByTagName("splitting");

                    foreach (XmlNode sp in splittings)
                    {
                        splitReturn s = new splitReturn();
                        double value_transaction = Convert.ToDouble(sp["value_transaction"].InnerText.Replace(".", ","));
                        s.descricao = string.Format("{0}x de R$ {1} {2}", sp["split"].InnerText, sp["value_split"].InnerText.Replace(".", ","), value_transaction > price_customer ? "com juros" : "sem juros");
                        s.valor = Convert.ToDouble(sp["value_split"].InnerText.Replace(".", ","));
                        s.parcela = Convert.ToInt32(sp["split"].InnerText);
                        s.payment_method_id = splr.payment_method_id;
                        splr.splits.Add(s);
                    }
                    splitRet.Add(splr);
                }
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaValorDividido: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }

            var ret = (from a in splitRet
                       where a.payment_method_id == idbandeira
                       select a).FirstOrDefault();

            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("GET")]
        [Route("pedido/total/{nrpedido}")]
        public HttpResponseMessage RetornaValorTotalPedido(string nrpedido)
        {
            double vrtotalpedido = 0;

            ConfigAppMembers cf = new ConfigAppMembers();

            try
            {
                cf = Util.ConfigApp.getConfig();

                vrtotalpedido = PedidosADO.RetornaValorTotalPedido(nrpedido, cf.datasource, cf.schema);

            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaValorTotalPedido: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, vrtotalpedido);
        }
        [AcceptVerbs("POST")]
        [Route("pedido/consulta")]
        public HttpResponseMessage GetPedidosByForn(HttpRequestMessage request, [FromBody] PedidoPesquisa ped)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<PedidoConsulta> ret = new List<PedidoConsulta>();
            string query = string.Empty;

            try
            {
                ret = Database.PedidosADO.RetornaPedidosForn(ped, cf.cdentifilial, cf.datasource, cf.schema, out query);
                Util.LogUtil.GravaLog(this, "GetPedidosByForn QUERY: " + query, cf.Cnpj, Log.TipoLog.erro);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "GetPedidosByForn: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("GET")]
        [Route("pedido/detalhe/{cdpedido}")]
        public HttpResponseMessage GetPedidosDetalhe(string cdpedido)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            PedidoDetalheRet ret = new PedidoDetalheRet();
            try
            {
                ret = Database.PedidosADO.RetornaPedidoDetalhe(cdpedido, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "GetPedidosDetalhe: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        public class splitListReturn
        {
            public splitListReturn()
            {
                this.splits = new List<splitReturn>();
            }
            public int payment_method_id { get; set; }
            public string payment_method_name { get; set; }
            public List<splitReturn> splits { get; set; }
        }

        public class splitReturn
        {
            public string descricao { get; set; }
            public double valor { get; set; }

            public int parcela { get; set; }

            public int payment_method_id { get; set; }
        }
        //[AcceptVerbs("GET")]
        //[Route("totais/{cdcesta}/")]
        //public HttpResponseMessage CestaGetTotais(string cdcesta, string cdforn)
        //{
        //    ConfigAppMembers cf = Util.ConfigApp.getConfig();
        //    List<cesta> ret = new List<cesta>();
        //    try
        //    {
        //        ret = Database.CestaADO.RetornaCesta(cdcesta, cdforn, cf.datasource, cf.schema);
        //    }
        //    catch (Exception e)
        //    {
        //        Util.LogUtil.GravaLog(this, "Retorna cesta : " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, ret);
        //}

        public static string FormataValorDecimal(double valor)
        {
            string retorno = "";
            bool virgula = false;
            bool ponto = false;
            bool acrescentazero = false;
            if (valor > 0)
            {


                int idx = valor.ToString().IndexOf(',');

                if (idx == -1)
                {
                    ponto = true;
                    idx = valor.ToString().IndexOf('.');
                }
                else
                {
                    virgula = true;
                }
                string[] vals = null;
                if (virgula)
                {
                    vals = valor.ToString("N3").Split(',');
                }
                else if (ponto)
                {
                    vals = valor.ToString("N3").Split('.');
                }

                if (vals[1].Count() == 1)
                {
                    acrescentazero = true;
                }

                for (int i = 0; i < valor.ToString("N3").Length; i++)
                {
                    if (i <= (idx + 2))
                    {
                        retorno += valor.ToString()[i];
                    }

                }
            }
            if (acrescentazero)
            {
                retorno += "0";
            }

            return retorno;
        }
    }


}
