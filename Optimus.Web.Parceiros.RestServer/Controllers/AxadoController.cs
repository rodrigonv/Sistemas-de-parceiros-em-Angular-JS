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

namespace Optimus.Web.Parceiros.RestServer.Controllers
{
    [EnableCors("*", "*", "GET, POST")]
    [RoutePrefix("axado")]
    public class AxadoController : ApiController
    {
        [AcceptVerbs("POST")]
        [Route("")]
        public HttpResponseMessage RetornaCotacaoAxado(HttpRequestMessage request, [FromBody] AxadoPostRest axadopost)
        {
            AxadoCotacoes ret = new AxadoCotacoes();
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            string outRest = string.Empty;
            string jsonRetorno = string.Empty;
            try
            {
                ret = AxadoADO.RetornaTabelaCotacao(axadopost.CepCliente, axadopost.VrTotalPedido, axadopost.PesoTotal, axadopost.TotalItens, axadopost.IdPedido, out outRest, out jsonRetorno);

                Util.LogUtil.GravaLog(this, "RetornaCotacaoAxado pedido: " + axadopost.IdPedido + jsonRetorno, cf.Cnpj, Log.TipoLog.info);

            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaCotacaoAxado: " + e.ToString(), "", Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, jsonRetorno);
        }

        [AcceptVerbs("GET")]
        [Route("tabela/{cdcesta}/{cdcepcliente}/")]
        public HttpResponseMessage RetornaCotacaoAxado(string cdcesta, string cdcepcliente)
        {
            AxadoCotacoes ret = new AxadoCotacoes();
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            string outRest = string.Empty;
            string jsonRetorno = string.Empty;
            try
            {
                ret = AxadoADO.RetornaTabelaCotacao(cdcesta, cdcepcliente, cf.datasource, cf.schema, out jsonRetorno);
                Util.LogUtil.GravaLog(this, string.Format("RetornaCotacaoAxado pedido:{0} - {1} ", cdcesta, jsonRetorno), cf.Cnpj, Log.TipoLog.info);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaCotacaoAxado: " + e.ToString(), "", Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }


    }
}
