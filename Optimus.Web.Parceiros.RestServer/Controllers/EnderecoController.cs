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
    [RoutePrefix("endereco")]
    public class EnderecoController : ApiController
    {
        [AcceptVerbs("GET")]
        [Route("contato/{cdcontato}/")]
        public HttpResponseMessage RetornaEnderecoByCdContato(string cdcontato)
        {
            List<Endereco> cfret = new List<Endereco>();
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {
                cfret = EnderecoADO.RetornaEnderecoByCdContato(cdcontato, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaEnderecoByCdContato: " + e.ToString(), "SEMCNPJ", Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, cfret);
        }
        [AcceptVerbs("GET")]
        [Route("get/{cdentidade}/")]
        public HttpResponseMessage RetornaEnderecoByCdEntidade(string cdentidade)
        {
            List<Endereco> cfret = new List<Endereco>();
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {
                cfret = EnderecoADO.RetornaEnderecoByCdEntidade(cdentidade, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaEnderecoByCdEntidade: " + e.ToString(), "SEMCNPJ", Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, cfret);
        }
    }
}
