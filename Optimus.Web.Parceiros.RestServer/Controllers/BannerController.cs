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
    [RoutePrefix("banner")]
    public class BannerController : ApiController
    {

        [AcceptVerbs("POST")]
        [Route("insert")]
        public HttpResponseMessage BannerInsert(HttpRequestMessage request, [FromBody] Banner banner)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            double ret = 0;
            try
            {
                BannerADO bado = new BannerADO();
                ret = bado.CadastrarBanner(banner, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "BannerInsert: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("POST")]
        [Route("delete")]
        public HttpResponseMessage BannerDelete(HttpRequestMessage request, [FromBody] Banner banner)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            double ret = 0;
            try
            {
                BannerADO bado = new BannerADO();
                ret = bado.DeleteBanner(banner.cdbanner, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "BannerInsert: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
        [AcceptVerbs("POST")]
        [Route("pesquisa")]
        public HttpResponseMessage BannerPesquisa(HttpRequestMessage request, [FromBody] Banner banner)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();

            List<Banner> ret = new List<Banner>();
            try
            {
                BannerADO bado = new BannerADO();
                ret = bado.ConsultarBanner(banner, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "BannerPesquisa: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
        [AcceptVerbs("POST")]
        [Route("byid")]
        public HttpResponseMessage BannerPesquisabyid(HttpRequestMessage request, [FromBody] Banner banner)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();

            Banner ret = new Banner();
            try
            {
                BannerADO bado = new BannerADO();
                ret = bado.GetBannerById(banner, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "BannerPesquisabyid: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("GET")]
        [Route("ativos")]
        public HttpResponseMessage BannersAtivos()
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();

            List<Banner> ret = new List<Banner>();
            try
            {
                BannerADO bado = new BannerADO();
                ret = bado.RetornaBannersAtivos(cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "BannersAtivos: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
    }
}
