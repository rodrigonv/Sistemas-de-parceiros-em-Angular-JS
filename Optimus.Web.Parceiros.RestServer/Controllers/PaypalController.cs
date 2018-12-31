using Newtonsoft.Json;
using Optimus.Web.Parceiros.RestServer.Database;
using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Util;
using Oracle.DataAccess.Client;
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
using PayPal.Api;
using Optimus.Web.Parceiros.Model.Optimus.Web.Parceiros;
using Optimus.Web.Parceiros.Model.PayPal.Nvp;
using System.Web;
using System.Collections.Specialized;

namespace Optimus.Web.Parceiros.RestServer.Controllers
{
    [EnableCors("*", "*", "GET, POST")]
    [RoutePrefix("paypal")]
    public class PaypalController : ApiController
    {

        [AcceptVerbs("GET")]
        [Route("{cdcesta}/{codpedpagar}/{cdforn}")]
        public HttpResponseMessage RetornaPedidoPagamentoPaypal(string cdcesta, string codpedpagar, string cdforn)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            RetPaypal ret = new RetPaypal();
            try
            {
                //pega a cesta 
                List<PedidoPaypal> lstRet = PedidosADO.RetornaPedidoPagamento(codpedpagar, cf.datasource, cf.schema);

                double vrtotalpedido = 0;

                Cart cart = new Cart();
                Product prd;
                foreach (var ped in lstRet)
                {
                    prd = new Product();
                    prd.Id = Convert.ToInt32(ped.cdproduto);
                    prd.Name = ped.nmproduto;
                    prd.Price = ped.preco;
                    vrtotalpedido += Convert.ToInt32(ped.quantidade) * ped.preco;
                    //prd.Quantidade = ped.quantidade;
                    cart.Add(prd, Convert.ToInt32(ped.quantidade));
                }
                double frete = lstRet[0].frete;
                vrtotalpedido += frete;

                List<pedidopagamento> lstpag = PagamentoADO.GravaPagamento(codpedpagar, "5", vrtotalpedido.ToString(), "N", "N", "1", "N", "0", cdforn, "0", "", "","","","", cf.datasource, cf.schema,"2");

                string codpag = lstpag[0].cdpag;

                ret.urlRedirect = Checkout.start("http://localhost:47905/PaypalReturn.aspx?codpag=" + codpag + "&codpedpagar=" + codpedpagar, "http://localhost:47905/PaypalCancel.aspx?codped=" + cdcesta + "&codpedpagar=" + codpedpagar, cart, frete, codpedpagar);

                Uri uri = new Uri(ret.urlRedirect);
                string tokenpaypal = HttpUtility.ParseQueryString(uri.Query).Get("token");
                //Lancar o pagamento com o token
                PagamentoADO.GravaPagamento(codpedpagar, "5", vrtotalpedido.ToString(), "N", "N", "1", "N", codpag, cdforn, "0", tokenpaypal, "","","","", cf.datasource, cf.schema,"2");


            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaPedidoPagamentoPaypal : " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("GET")]
        [Route("finalize/{token}/{PayerID}/{codpag}")]
        public HttpResponseMessage RetornaFinalizaPagamentoPaypal(string token, string PayerID, string codpag)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            PaypalFinalize ret = new PaypalFinalize();
            try
            {

                NameValueCollection nvp = Checkout.finalize(token, PayerID);
                ret.payerid = PayerID;
                ret.tokentransacao = token;
                ret.shiptoname = nvp["PAYMENTREQUEST_0_SHIPTONAME"];
                ret.shiptocity = nvp["PAYMENTREQUEST_0_SHIPTOCITY"];
                ret.shiptostate = nvp["PAYMENTREQUEST_0_SHIPTOSTATE"];
                ret.shiptostreet = nvp["PAYMENTREQUEST_0_SHIPTOSTREET"];
                ret.shiptostreet2 = nvp["PAYMENTREQUEST_0_SHIPTOSTREET2"];
                ret.shiptozip = nvp["PAYMENTREQUEST_0_SHIPTOZIP"];
                ret.statusakc = nvp["ACK"];
                ret.firstname = nvp["FIRSTNAME"];
                ret.lastname = nvp["LASTNAME"];
                ret.email = nvp["EMAIL"];

                //Finaliza o pagamento
                if (ret.statusakc == "Success")
                {
                    List<pedidopagamento> lstpag = PagamentoADO.BuscaPagamentoTituloByCodpag(codpag, cf.datasource, cf.schema);

                    PagamentoADO.GravaPagamento(lstpag[0].codped, "5", lstpag[0].valorpago, "N", "S", "1", "N", codpag, "", "A", token, PayerID,"","","", cf.datasource, cf.schema,"2");
                }
                

            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaFinalizaPagamentoPaypal : " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
       
    }
    public class RetPaypal
    {
        public string urlRedirect { get; set; }
    }
}
