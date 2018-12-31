using Newtonsoft.Json;
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
    [RoutePrefix("config")]
    public class ConfigController : ApiController
    {
        [AcceptVerbs("GET")]
        [Route("{pkgname}/")]
        public HttpResponseMessage RetornaConfigApp(string pkgname)
        {
            ConfigAppobj cfret = new ConfigAppobj();
            try
            {
                cfret = ConfigADO.RetornaConfig(pkgname);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaConfigApp: " + e.ToString(), "SEMCNPJ", Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(cfret));
        }
    }
}
