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
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Optimus.Web.Parceiros.RestServer.Controllers
{
    [EnableCors("*", "*", "GET, POST")]
    [RoutePrefix("cep")]
    public class CepController : ApiController
    {

        [AcceptVerbs("GET")]
        [Route("{cep}/")]
        public HttpResponseMessage RetornaEnderecoByCep(string cep)
        {
            RetornoCep cfret = new RetornoCep();
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {
                cfret = RetornaEndereco(cep, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                //Util.LogUtil.GravaLog(this, "RetornaConfigApp: " + e.ToString(), "SEMCNPJ", Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, cfret);
        }
        [AcceptVerbs("GET")]
        [Route("busca/{cep}")]
        public HttpResponseMessage RetornaEnderecoByCep2(string cep)
        {
            RetornoCep cfret = new RetornoCep();
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {
                cfret = RetornaEndereco(cep, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                //Util.LogUtil.GravaLog(this, "RetornaConfigApp: " + e.ToString(), "SEMCNPJ", Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, cfret);
        }
        public static RetornoCep RetornaEndereco(string cep, string datasource, string schema)
        {
            List<RetornoCep> lstResult = new List<RetornoCep>();

            string queryCep =
                string.Format("select cdcep, txcidade, cduf, txlogradouro, txbairro,cdpais, txpais, cdmunicipioibge, cdufibge, cdpaisibge,cdtipocep, cdtipocidade, cdcepid from optbdcomum.cep where cdcep = '{0}'", cep);
            string _CONN = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Ora"].ConnectionString;
            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryCep, connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    RetornoCep pr = null;
                    while (reader.Read())
                    {
                        pr = new RetornoCep();
                        pr.Cidade = reader["txcidade"].ToString();
                        pr.Bairro = reader["txbairro"].ToString();
                        pr.Cep = reader["cdcep"].ToString();
                        pr.Endereco = reader["txlogradouro"].ToString();
                        pr.Uf = reader["cduf"].ToString();
                        pr.CdmunicipioIbge = reader["cdmunicipioibge"].ToString();
                        lstResult.Add(pr);
                    }
                }
            }
            return lstResult.FirstOrDefault();
        }
    }
}