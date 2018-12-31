using Optimus.Web.Parceiros.RestServer.Database;
using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Optimus.Web.Parceiros.RestServer.Controllers
{
    [EnableCors("*", "*", "GET")]
    [RoutePrefix("promocao")]
    public class PromocaoController : ApiController
    {
        [AcceptVerbs("GET")]
        [Route("")]
        public HttpResponseMessage RetornaPromocaoByIdFilial()
        {
            List<Promocao> lstReturn = new List<Promocao>();

            ConfigAppMembers cf = new ConfigAppMembers();

            try
            {
                cf = Util.ConfigApp.getConfig();
                lstReturn = PromocaoADO.GetPromocao(cf.cdentifilial, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaPromocaoByIdFilial: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, lstReturn);
        }

        [AcceptVerbs("GET")]
        [Route("retorna/{cdpromocao}")]
        public HttpResponseMessage RetornaPromocaoByIdFilialCdpromocao(string cdpromocao)
        {
            Promocao promoReturn = new Promocao();

            ConfigAppMembers cf = new ConfigAppMembers();
            Util.LogUtil.GravaLog(this, "RetornaPromocaoByIdFilialCdpromocao: cdpromocao " + cdpromocao, cf.Cnpj, Log.TipoLog.info);
            try
            {
                cf = Util.ConfigApp.getConfig();
                promoReturn = PromocaoADO.GetPromocaoByCdpromocao(cf.cdentifilial, cdpromocao, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaPromocaoByIdFilialCdpromocao: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, promoReturn);
        }
    }
}
