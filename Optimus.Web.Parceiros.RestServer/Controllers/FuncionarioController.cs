using Newtonsoft.Json;
using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Util;
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
    [RoutePrefix("funcionario")]
    public class FuncionarioController : ApiController
    {
        [AcceptVerbs("POST")]
        [Route("")]
        public HttpResponseMessage cadastrarFuncionario(HttpRequestMessage request, [FromBody] UsuarioPost user)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            string ret = string.Empty;
            try
            {
                Database.UsuarioADO.CadastrarColaborador(user.cdfuncionario.ToString(),user.nomeuser,user.usuario,user.senha,user.cdforn, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "cadastrarFuncionario:" + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "ok");

        }

        [AcceptVerbs("POST")]
        [Route("login")]
        public HttpResponseMessage Login(HttpRequestMessage request, [FromBody] UsuarioPost user)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            UsuarioRetorno ret = new UsuarioRetorno();
            try
            {
                ret = Database.UsuarioADO.Login(user.usuario, user.senha, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "Login: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);

        }

        [AcceptVerbs("POST")]
        [Route("pesquisa")]
        public HttpResponseMessage PesquisaColaborador(HttpRequestMessage request, [FromBody] UsuarioPost user)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<UsuarioRetorno> ret = new List<UsuarioRetorno>();
            try
            {
                ret = Database.UsuarioADO.RetornaColaborador(user.nomeuser, user.usuario, user.cdforn, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "PesquisaColaborador: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);

        }

        [AcceptVerbs("GET")]
        [Route("pesquisa/{cdcolaborador}")]
        public HttpResponseMessage PesquisaColaboradorbyid(string cdcolaborador)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            UsuarioRetorno ret = new UsuarioRetorno();
            try
            {
                ret = Database.UsuarioADO.RetornaColaboradorByid(cdcolaborador, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "PesquisaColaborador: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);

        }

        [AcceptVerbs("GET")]
        [Route("pesquisanome/{nome}")]
        public HttpResponseMessage PesquisaColaboradorbynome(string nome)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            bool ret = false;
            try
            {
                ret = Database.UsuarioADO.TemColaboradorByNome(nome, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "PesquisaColaborador: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);

        }

        [AcceptVerbs("GET")]
        [Route("excluir/{cdfuncionario}")]
        public HttpResponseMessage Excluirfunc(string cdfuncionario)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            bool ret = false;
            try
            {
                ret = Database.UsuarioADO.ExcluirFuncionario(cdfuncionario, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "PesquisaColaborador: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);

        }

        [AcceptVerbs("GET")]
        [Route("parceiros")]
        public HttpResponseMessage RetornaParceiros()
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<Parceiro> ret = new List<Parceiro>();
            try
            {
                ret = Database.UsuarioADO.RetornaParceiros(cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaParceiros: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);

        }


        //RetornaColaboradoresByIdPai


        [AcceptVerbs("GET")]
        [Route("colaboradorparceiro/{cdforn}")]
        public HttpResponseMessage Retornacolaboradorparceiro(string cdforn)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            List<UsuarioRetorno> ret = new List<UsuarioRetorno>();
            try
            {
                ret = Database.UsuarioADO.RetornaColaboradoresByIdPai(cdforn, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "Retornacolaboradorparceiro: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);

        }

    }

}
