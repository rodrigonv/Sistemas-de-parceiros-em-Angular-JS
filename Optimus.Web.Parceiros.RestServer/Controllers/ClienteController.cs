using Newtonsoft.Json;
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
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;


namespace Optimus.Web.Parceiros.RestServer.Controllers
{
    [EnableCors("*", "*", "GET, POST")]
    [RoutePrefix("cliente")]
    public class ClienteController : ApiController
    {
        [AcceptVerbs("POST")]
        [Route("insert")]
        public HttpResponseMessage ClienteInsert(HttpRequestMessage request, [FromBody] cliente cli)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            double ret = 0;
            try
            {
                Util.LogUtil.GravaLog(this, "Cliente post: " + JsonConvert.SerializeObject(cli), cf.Cnpj, Log.TipoLog.info);

                ret = Database.ClienteADO.GravaClienteRest(cli, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "Cliente post: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("POST")]
        [Route("search")]
        public HttpResponseMessage ClienteBuscaByParam(HttpRequestMessage request, [FromBody] clientesearch cli)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<cliente> ret = new List<cliente>();
            try
            {
                ret = Database.ClienteADO.RetornaClientByParam(cli, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "Cliente search: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("GET")]
        [Route("search/{cdentidade}")]
        public HttpResponseMessage ClienteBuscaByCdEntidade(string cdentidade)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            cliente ret = new cliente();
            try
            {
                ret = Database.ClienteADO.RetornaClientById(cdentidade, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "Cliente search BY ID: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("GET")]
        [Route("contato/{cdentidade}")]
        public HttpResponseMessage RetornaContatoByCdEntidade(string cdentidade)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<ContatoRet> ret = new List<ContatoRet>();
            try
            {
                ret = Database.ClienteADO.RetornaContatoByCdEntidade(cdentidade, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaContatoByCdEntidade: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("GET")]
        [Route("contato/validartelefone/{cdentidade}")]
        public HttpResponseMessage RetornaRetornaSeTemTelefone(string cdentidade)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            bool ret = false;
            try
            {
                ret = Database.ClienteADO.RetornaRetornaSeTemTelefone(cdentidade, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaRetornaSeTemTelefone: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("GET")]
        [Route("verificarcpfcnpj/{cpfcnpj}")]
        public HttpResponseMessage RetornaClienteByCpfCnpj(string cpfcnpj)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            cliente ret = new cliente();
            try
            {
                ret = Database.ClienteADO.RetornaClientByCpfCnpj(cpfcnpj, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaClienteByCpfCnpj: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }


        [AcceptVerbs("GET")]
        [Route("todos")]
        public HttpResponseMessage retprod()
        {
            List<prodteste> ret = new List<prodteste>();
            string q = "SELECT TO_CHAR(CDENTIDADE) AS CDENTIDADE,CNPJ_CPF_FORMATADO,CNPJ_CPF,RAZAO_NOME FROM VW_ENTIAFILIADO";

            //?$top=10&$filter=substringof(%27igrej%27,tolower(d))&$orderby=d

            var filter = Request.GetQueryNameValuePairs();


            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn("multivisi", "localhost")))
            {
                OracleCommand command = new OracleCommand(q, connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    prodteste cli = null;
                    while (reader.Read())
                    {
                        cli = new prodteste();
                        cli.a = reader["CDENTIDADE"].ToString();
                        cli.b = reader["CNPJ_CPF"].ToString();
                        cli.c = reader["CNPJ_CPF_FORMATADO"].ToString();
                        cli.d = reader["RAZAO_NOME"].ToString();

                        ret.Add(cli);
                    }
                }
            }
            string output = JsonConvert.SerializeObject(ret);


            //var httpResponseMessage = new HttpResponseMessage();
            //Request.Content = new StringContent(output);
            //Request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");


            //var response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Content = new StringContent(output);
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            //return response;

            return Request.CreateResponse(HttpStatusCode.OK, output);
        }
    }
    public class prodteste
    {
        public string a { get; set; }
        public string b { get; set; }
        public string c { get; set; }
        public string d { get; set; }
    }
}
