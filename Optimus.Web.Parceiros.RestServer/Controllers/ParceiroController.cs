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
    [RoutePrefix("parceiro")]
    public class ParceiroController : ApiController
    {

        [AcceptVerbs("POST")]
        [Route("search")]
        public HttpResponseMessage ParceiroBuscaByParam(HttpRequestMessage request, [FromBody] clientesearch cli)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<cliente> ret = new List<cliente>();
            try
            {
                ret = Database.ClienteADO.RetornaParceiroByParam(cli, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "ParceiroBuscaByParam: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("POST")]
        [Route("comissaoagg")]
        public HttpResponseMessage ParceiroBuscaByParam(HttpRequestMessage request, [FromBody] ParceiroSearch parc)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<ParceiroComissao> ret = new List<ParceiroComissao>();
            try
            {
                ret = Database.ParceiroADO.RetornaComissaoFuncAggByParam(parc, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "ParceiroBuscaByParam: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
    }
}
