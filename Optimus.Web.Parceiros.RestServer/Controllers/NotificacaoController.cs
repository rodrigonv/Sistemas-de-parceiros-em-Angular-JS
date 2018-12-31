using Newtonsoft.Json;
using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Optimus.Web.Parceiros.RestServer.Controllers
{
    [EnableCors("*", "*", "POST")]
    [RoutePrefix("notificacao")]
    public class NotificacaoController : ApiController
    {
        //
        [AcceptVerbs("POST")]
        [Route("update")]
        [System.Web.Mvc.ValidateInput(false)]

        //public HttpResponseMessage cadastrar(Usuario user)
        public HttpResponseMessage updateNotificacao(HttpRequestMessage request, [FromBody] Notificacao notificacao)
        {
            RetOk retok = new RetOk();
            Util.LogUtil.GravaLog(this, "updateNotificacao: entrou " + JsonConvert.SerializeObject(notificacao), "9999", Log.TipoLog.info);

            ConfigAppMembers cf = new ConfigAppMembers();
            cf.Cnpj = "9999";
            try
            {
                try
                {
                    cf = Util.ConfigApp.getConfig();
                }
                catch (Exception exxx)
                {
                    Util.LogUtil.GravaLog(this, "updateNotificacao: erro cf " + exxx.ToString(), "9999", Log.TipoLog.erro);
                }


                Util.LogUtil.GravaLog(this, "updateNotificacao: " + JsonConvert.SerializeObject(notificacao) + "d:" + cf.datasource + "s:" + cf.schema, cf.Cnpj, Log.TipoLog.info);

                int retquery = Database.NotificacaoADO.UpdateNotificacao(notificacao.idNotificacao, cf.datasource, cf.schema);

                retok.Codigo = "100";
                retok.Mensagem = "OK";
            }
            catch (Exception e)
            {
                retok.Codigo = "200";
                retok.Mensagem = "NOK";
                Util.LogUtil.GravaLog(this, "updateNotificacao: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.BadRequest, retok);
            }


            string json = JsonConvert.SerializeObject(retok);
            var res = Request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return res;


        }

    }
}
