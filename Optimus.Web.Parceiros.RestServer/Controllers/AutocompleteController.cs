using Newtonsoft.Json;
using Optimus.Web.Parceiros.RestServer.Util;
using Oracle.DataAccess.Client;
using RestSharp.Extensions.MonoHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Optimus.Web.Parceiros.RestServer.Controllers
{
    [EnableCors("*", "*", "GET, POST")]
    [RoutePrefix("autocomplete")]
    public class AutocompleteController : ApiController
    {
        [AcceptVerbs("GET")]
        [Route("cliente")]
        public HttpResponseMessage retprod()
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();

            List<prodteste> ret = new List<prodteste>();

            string q = "SELECT TO_CHAR(CDENTIDADE) AS CDENTIDADE,CNPJ_CPF_FORMATADO,CNPJ_CPF,RAZAO_NOME FROM VW_ENTIAFILIADO WHERE 1=1";

            IEnumerable<KeyValuePair<string, string>> queryString = Request.GetQueryNameValuePairs();

            string filtro = queryString.Where(nv => nv.Key == "$filter").Select(nv => nv.Value).FirstOrDefault();

            string top = queryString.Where(nv => nv.Key == "$top").Select(nv => nv.Value).FirstOrDefault();

            filtro = HttpUtility.UrlDecode(filtro);
            string campo = "";
            //?$top=10&$filter=substringof(%27igrej%27,tolower(d))&$orderby=d
            filtro = filtro.Substring(13, filtro.Length - 13);
            campo = filtro.Replace(")","");
            filtro = filtro.Substring(0, filtro.IndexOf("'"));
            campo = campo.Substring(campo.IndexOf("(") + 1, (campo.Length - campo.IndexOf("(") - 1));

            string where = "";

            if (!string.IsNullOrEmpty(filtro))
            {
                switch (campo)
                {
                    case "CDENTIDADE":
                        {
                            where = string.Format(" AND TO_CHAR(CDENTIDADE) LIKE '{0}%'", filtro);
                            break;
                        }
                    case "CNPJ_CPF":
                        {
                            where = string.Format(" AND CNPJ_CPF LIKE '{0}%'", filtro);
                            break;
                        }
                    case "RAZAO_NOME":
                        {
                            where = string.Format(" AND RAZAO_NOME LIKE UPPER('{0}%')", filtro);
                            break;
                        }
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(top))
                {
                    where += " AND ROWNUM <=" + top;
                }

                q = q + where;


                using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(cf.schema, cf.datasource)))
                {
                    OracleCommand command = new OracleCommand(q, connection);
                    connection.Open();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        prodteste cli = null;
                        while (reader.Read())
                        {
                            cli = new prodteste();
                            cli.CDENTIDADE = reader["CDENTIDADE"].ToString();
                            cli.CNPJ_CPF = reader["CNPJ_CPF"].ToString();
                            cli.CNPJ_CPF_FORMATADO = reader["CNPJ_CPF_FORMATADO"].ToString();
                            cli.RAZAO_NOME = reader["RAZAO_NOME"].ToString();

                            ret.Add(cli);
                        }
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

        public class prodteste
        {
            public string CDENTIDADE { get; set; }
            public string CNPJ_CPF { get; set; }
            public string CNPJ_CPF_FORMATADO { get; set; }
            public string RAZAO_NOME { get; set; }
        }
    }
}
