using Optimus.Web.Parceiros.RestServer.Database;
using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Optimus.Web.Parceiros.RestServer.Controllers
{
    [EnableCors("*", "*", "GET, POST")]
    [RoutePrefix("pedido")]
    public class PedidoController : ApiController
    {
        //[AcceptVerbs("GET")]
        //[Route("{email}/")]
        //public HttpResponseMessage RetornaPedidoByEmail(string email)
        //{
        //    List<Pedido> lstReturn = new List<Pedido>();

        //    ConfigAppMembers cf = new ConfigAppMembers();

        //    try
        //    {
        //        cf = Util.ConfigApp.getConfig();
        //        lstReturn = PedidosADO.RetornaPedidos(cf.cdentifilial, email, cf.datasource, cf.schema);
        //    }
        //    catch (Exception e)
        //    {
        //        Util.LogUtil.GravaLog(this, "RetornaPedidoByEmail: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, lstReturn);
        //}

        //[AcceptVerbs("GET")]
        //[Route("{email}/{nrpedido}")]
        //public HttpResponseMessage RetornaPedidoByEmailNrpedido(string email, string nrpedido)
        //{
        //    Pedido PedReturn = new Pedido();

        //    ConfigAppMembers cf = new ConfigAppMembers();
        //    try
        //    {
        //        cf = Util.ConfigApp.getConfig();
        //        PedReturn = PedidosADO.RetornaPedido(cf.cdentifilial, email, Convert.ToInt32(nrpedido), cf.datasource, cf.schema);
        //    }
        //    catch (Exception e)
        //    {
        //        Util.LogUtil.GravaLog(this, "RetornaPedidoByEmailNrpedido: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, PedReturn);
        //}

        [AcceptVerbs("POST")]
        [Route("")]
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

       
        //



    }

}
