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
    [RoutePrefix("basico")]
    public class BasicoController : ApiController
    {
        /*
            select cdtipotelefone,nmtipotelefone from tipotelefone where stexcluido = 0
            select cdtipoemail,nmtipoemail from tipoemail where stexcluido = 0
            select cdtipocontato,nmtipocontato from tipocontato where stexcluido=0
            select cdtipoendereco,nmtipoendereco from tipoendereco where stexcluido=0
         */

        [AcceptVerbs("GET")]
        [Route("tipotelefone")]
        public HttpResponseMessage RetornaTipoTelefoneRest()
        {
            List<Basico> bret = new List<Basico>();
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {
                bret = BasicoADO.RetornaTipoTelefone(cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                //Util.LogUtil.GravaLog(this, "RetornaConfigApp: " + e.ToString(), "SEMCNPJ", Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, bret);
        }

        [AcceptVerbs("GET")]
        [Route("tipoemail")]
        public HttpResponseMessage RetornaTipoEmailRest()
        {
            List<Basico> bret = new List<Basico>();
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {
                bret = BasicoADO.RetornaTipoEmail(cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                //Util.LogUtil.GravaLog(this, "RetornaConfigApp: " + e.ToString(), "SEMCNPJ", Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, bret);
        }

        [AcceptVerbs("GET")]
        [Route("tipocontato")]
        public HttpResponseMessage RetornaTipoContatoRest()
        {
            List<Basico> bret = new List<Basico>();
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {
                bret = BasicoADO.RetornaTipoContato(cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                //Util.LogUtil.GravaLog(this, "RetornaConfigApp: " + e.ToString(), "SEMCNPJ", Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, bret);
        }

        [AcceptVerbs("GET")]
        [Route("tipoendereco")]
        public HttpResponseMessage RetornaTipoEnderecoRest()
        {
            List<Basico> bret = new List<Basico>();
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {
                bret = BasicoADO.RetornaTipoEndereco(cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                //Util.LogUtil.GravaLog(this, "RetornaConfigApp: " + e.ToString(), "SEMCNPJ", Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, bret);
        }

        [AcceptVerbs("GET")]
        [Route("statuspedido")]
        public HttpResponseMessage RetornaStatusPedidoRest()
        {
            List<Basico> bret = new List<Basico>();
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {
                bret = BasicoADO.RetornaStatusPedido(cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                //Util.LogUtil.GravaLog(this, "RetornaConfigApp: " + e.ToString(), "SEMCNPJ", Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, bret);
        }
    }
}
