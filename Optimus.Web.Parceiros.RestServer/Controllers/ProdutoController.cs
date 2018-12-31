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
    [RoutePrefix("produto")]
    public class ProdutoController : ApiController
    {
        [AcceptVerbs("POST")]
        [Route("")]
        public HttpResponseMessage ProdutoInsert(HttpRequestMessage request, [FromBody] produtosearch prod)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<Produto> ret = new List<Produto>();
            try
            {
                ret = Database.ProdutoADO.RetornaProdutoPesquisa(prod, cf.datasource, cf.schema, cf.cdentifilial);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "Produto search: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("POST")]
        [Route("insert")]
        public HttpResponseMessage ProdutoParceiroInsert(HttpRequestMessage request, [FromBody] Produto prod)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<Produto> ret = new List<Produto>();
            try
            {
                ret = Database.ProdutoADO.CadastrarProdParceiro(prod.cdproduto, cf.cdentifilial, prod.codforn, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "ProdutoParceiroInsert: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
        [AcceptVerbs("POST")]
        [Route("insertnolist")]
        public HttpResponseMessage ProdutoParceiroInsertnolist(HttpRequestMessage request, [FromBody] Produto prod)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {
                Database.ProdutoADO.CadastrarProdParceironolist(prod.cdproduto, cf.cdentifilial, prod.codforn, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "ProdutoParceiroInsertnolist: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "ok");
        }
        [AcceptVerbs("POST")]
        [Route("search")]
        public HttpResponseMessage RetornaProdutoPesquisaParceiroRest(HttpRequestMessage request, [FromBody] Produto prod)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<Produto> ret = new List<Produto>();
            try
            {
                ret = Database.ProdutoADO.RetornaProdutoPesquisaParceiro(cf.datasource, cf.schema, prod.codforn);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaProdutoPesquisaParceiroRest: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("POST")]
        [Route("searchprod")]
        public HttpResponseMessage RetornaProdutoPesquisaParceiroRestprod(HttpRequestMessage request, [FromBody] Produto prod)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<Produto> ret = new List<Produto>();
            try
            {
                ret = Database.ProdutoADO.RetornaProdutoPesquisaParceiro(cf.datasource, cf.schema, prod.codforn);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaProdutoPesquisaParceiroRestprod: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
        [AcceptVerbs("POST")]
        [Route("searchprodsel")]
        public HttpResponseMessage RetornaProdutoPesquisaParceiroRestprod(HttpRequestMessage request, [FromBody] produtosearch prod)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<Produto> ret = new List<Produto>();
            try
            {
                ret = Database.ProdutoADO.RetornaProdutoPesquisaParceiroSel(cf.datasource, cf.schema, prod);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaProdutoPesquisaParceiroRestprod: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
        [AcceptVerbs("POST")]
        [Route("searchnofilial")]
        public HttpResponseMessage ProdutoPesquisaSemfilial(HttpRequestMessage request, [FromBody] produtosearch prod)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<Produto> ret = new List<Produto>();
            try
            {
                ret = Database.ProdutoADO.RetornaProdutoPesquisaSemFilial(prod, cf.datasource, cf.schema, cf.cdentifilial);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "ProdutoPesquisaSemfilial: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }



        [AcceptVerbs("POST")]
        [Route("delete")]
        public HttpResponseMessage DeleteParceiroRestprod(HttpRequestMessage request, [FromBody] Produto prod)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {
                Database.ProdutoADO.ExcluirProdParceiro(prod.cdprodparceiro, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "DeleteParceiroRestprod: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "ok");
        }
    }
}
