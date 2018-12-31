using Newtonsoft.Json;
using Optimus.Web.Parceiros.RestServer.Database;
using Optimus.Web.Parceiros.RestServer.Extensions;
using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Util;
using Oracle.DataAccess.Client;
using RJS.Optimus.Biblioteca;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Optimus.Web.Parceiros.RestServer.Controllers
{
    [EnableCors("*", "*", "GET, POST")]
    [RoutePrefix("upload")]
    public class UploadController : ApiController
    {

        [AcceptVerbs("POST")]
        [Route("send")]
        //public async Task<IHttpActionResult> Add()

        public async Task<HttpResponseMessage> Add()
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            double ret = 0;


            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = ConfigurationManager.AppSettings.Get("PATHARQUIVOS") + "\\UPLOAD";

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var provider = new CustomMultipartFormDataStreamProvider(root);

            var result = await Request.Content.ReadAsMultipartAsync(provider);

            int id = 0;

            var model = result.FormData["model"];

            UploadParceiroADO upado = new UploadParceiroADO();
            UploadParceiro up = JsonConvert.DeserializeObject<UploadParceiro>(model);

            up.cdentifilial = Convert.ToInt32(cf.cdentifilial);

            id = upado.InsertUpload(up, cf.datasource, cf.schema, cf.cdentifilial);


            FileInfo fi = null;
            string extensao = string.Empty;

            foreach (var file in result.FileData)
            {
                fi = new FileInfo(file.LocalFileName);

                System.IO.File.Move(file.LocalFileName, root + "\\" + id.ToString() + fi.Extension);
                up.txpath = id.ToString() + fi.Extension;
                up.CdUploadParceiro = id;
                upado.UpdatePath(up, cf.datasource, cf.schema);
            }


            return Request.CreateResponse(HttpStatusCode.OK, "OK");

        }

        [AcceptVerbs("POST")]
        [Route("consulta/{cdpai}")]
        public HttpResponseMessage GetNfeByForn(HttpRequestMessage request, [FromBody] UploadParceiroPesquisa pesq, string cdpai)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<UploadParceiro> ret = new List<UploadParceiro>();
            string query = string.Empty;

            try
            {
                ret = Database.UploadParceiroADO.ConsultarNfe(pesq, cdpai, cf.cdentifilial, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "GetPedidosByForn: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
    }
}
