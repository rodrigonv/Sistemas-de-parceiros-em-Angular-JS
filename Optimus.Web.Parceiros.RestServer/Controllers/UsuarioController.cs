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
    [RoutePrefix("usuario")]
    public class UsuarioController : ApiController
    {
        [AcceptVerbs("POST")]
        //[Route("insert/{email}/{idcel}/{cdentifilial}/{so}/{cdapp}")]
        [Route("insert")]
        [System.Web.Mvc.ValidateInput(false)]

        //public HttpResponseMessage cadastrar(Usuario user)
        public HttpResponseMessage cadastrar(HttpRequestMessage request, [FromBody] Usuario user)
        {
            RetOk retok = new RetOk();

            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {
                string aa = JsonConvert.SerializeObject(user);
                //if (!string.IsNullOrEmpty(user.email))
                //{
                //Usuario userexiste = Database.UsuarioADO.ChecaEmail(user.email, user.cdapp, cf.cdentifilial, cf.datasource, cf.schema);
                //if (userexiste != null)
                //{
                //    retok.Codigo = "199";
                //    retok.Mensagem = "NOK";
                //}
                //else
                //{
                //    Util.LogUtil.GravaLog(this, "User post: " + aa, cf.Cnpj, Log.TipoLog.info);
                //    int retquery = Database.UsuarioADO.InsertUser(user, cf.datasource, cf.schema);

                //    retok.Codigo = "100";
                //    retok.Mensagem = "OK";
                //    //}
                //}
                //else
                //{
                Util.LogUtil.GravaLog(this, "User post: " + aa, cf.Cnpj, Log.TipoLog.info);

                string pass = string.Empty;

                bool enviarsenha = false;

                if (!string.IsNullOrEmpty(user.email) && user.senha == "CAD123VINICLARK4321")
                {
                    pass = Util.Cripto.CreatePassword(6);

                    user.senha = pass;

                    enviarsenha = true;
                }


                int retquery = Database.UsuarioADO.InsertUser(user, cf.datasource, cf.schema);

                if (enviarsenha)
                {

                    string query = string.Format("SELECT txsmtp,nrportasmtp,txcontaemail,txsenhaemail,stssl,txtemplateemail FROM configapp where cdapp ={0} and cdentifilial = {1}", cf.codapp, cf.cdentifilial);

                    DataSet dts = OracleHelper.ExecQuery(query, null, null, ConfigurationManager.AppSettings.Get("conintegracao"));
                    string smtp = string.Empty;
                    string porta = string.Empty;
                    string usuario = string.Empty;
                    string senha = string.Empty;
                    bool EnableSSL = false;
                    string templatehtml = string.Empty;
                    StringBuilder txbody = new StringBuilder();
                    if (dts.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dts.Tables[0].Rows)
                        {
                            smtp = dr["txsmtp"].ToString();
                            porta = dr["nrportasmtp"].ToString();
                            usuario = dr["txcontaemail"].ToString();
                            senha = dr["txsenhaemail"].ToString();
                            EnableSSL = dr["stssl"].ToString() == "1";
                            templatehtml = dr["txtemplateemail"].ToString();
                        }
                    }
                    RJSOptimusEmail emailsend = new RJSOptimusEmail(smtp, Convert.ToInt32(porta), usuario, senha, EnableSSL);

                    emailsend.Subject = "Cadastro";
                    emailsend.To = user.email;
                    emailsend.From = usuario;

                    if (string.IsNullOrEmpty(templatehtml))
                    {
                        txbody.AppendLine("Segue o seu usuário e senha de acesso.");
                        txbody.AppendLine(string.Format("Usuário: {0}", user.email));
                        txbody.AppendLine(string.Format("Senha:{0}", pass));
                    }
                    else
                    {//todo dar replace
                        txbody.Append(templatehtml);
                    }
                    emailsend.Body = txbody.ToString();
                    emailsend.Enviar();

                }


                retok.Codigo = "100";
                retok.Mensagem = "OK";
                //}

            }
            catch (Exception e)
            {
                retok.Codigo = "200";
                retok.Mensagem = "NOK";
                Util.LogUtil.GravaLog(this, "User post: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, retok);
            }


            string json = JsonConvert.SerializeObject(retok);
            var res = Request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return res;


        }



        [AcceptVerbs("POST")]
        [Route("login")]
        public HttpResponseMessage logar(HttpRequestMessage request, [FromBody] Usuario user)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            try
            {

                user = Database.UsuarioADO.Login(user, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "Login post: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [AcceptVerbs("GET")]
        [Route("check/{email}/")]
        public HttpResponseMessage checkEmail(string email)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            Usuario user = new Usuario();
            try
            {
                user = Database.UsuarioADO.ChecaEmail(email, cf.codapp, cf.cdentifilial, cf.datasource, cf.schema);
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "checkEmail get: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [AcceptVerbs("GET")]
        [Route("get/{email}/")]
        public HttpResponseMessage getuserByEmail(string email)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            Usuario user = new Usuario();
            RetOk retok = new RetOk();
            try
            {
                user = Database.UsuarioADO.RetornaUsuarioByEmail(email, cf.codapp, cf.cdentifilial, cf.datasource, cf.schema);

                if (string.IsNullOrEmpty(user.email))
                {
                    retok.Codigo = "200";
                    retok.Mensagem = "NOK";

                }
                else
                {
                    retok.Codigo = "100";
                    retok.Mensagem = "OK";
                    retok.dados = user;
                }

            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "getuserByEmail: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }

            string json = JsonConvert.SerializeObject(retok);
            var res = Request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return res;
            //return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [AcceptVerbs("GET")]
        [Route("esquecisenha/{email}/")]
        public HttpResponseMessage EsqueciSenhaByEmail(string email)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            Usuario user = new Usuario();
            RetOk retok = new RetOk();
            try
            {
                user = Database.UsuarioADO.RetornaUsuarioByEmail(email, cf.codapp, cf.cdentifilial, cf.datasource, cf.schema);

                if (string.IsNullOrEmpty(user.email))
                {
                    retok.Codigo = "200";
                    retok.Mensagem = "NOK";

                }
                else
                {
                    string query = string.Format("SELECT txsmtp,nrportasmtp,txcontaemail,txsenhaemail,stssl,txtemplateemail FROM configapp where cdapp ={0} and cdentifilial = {1}", cf.codapp, cf.cdentifilial);

                    DataSet dts = OracleHelper.ExecQuery(query, null, null, ConfigurationManager.AppSettings.Get("conintegracao"));
                    string smtp = string.Empty;
                    string porta = string.Empty;
                    string usuario = string.Empty;
                    string senha = string.Empty;
                    bool EnableSSL = false;
                    string templatehtml = string.Empty;
                    StringBuilder txbody = new StringBuilder();
                    if (dts.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dts.Tables[0].Rows)
                        {
                            smtp = dr["txsmtp"].ToString();
                            porta = dr["nrportasmtp"].ToString();
                            usuario = dr["txcontaemail"].ToString();
                            senha = dr["txsenhaemail"].ToString();
                            EnableSSL = dr["stssl"].ToString() == "1";
                            templatehtml = dr["txtemplateemail"].ToString();
                        }
                    }
                    RJSOptimusEmail emailsend = new RJSOptimusEmail(smtp, Convert.ToInt32(porta), usuario, senha, EnableSSL);

                    emailsend.Subject = "Recuperação de senha";
                    emailsend.To = user.email;
                    emailsend.From = usuario;

                    if (string.IsNullOrEmpty(templatehtml))
                    {
                        txbody.AppendLine("Você solicitou a sua senha.");
                        txbody.AppendLine(string.Format("Usuário: {0}", email));
                        txbody.AppendLine(string.Format("Senha:{0}", user.senha));
                    }
                    else
                    {//todo dar replace
                        txbody.Append(templatehtml);
                    }
                    emailsend.Body = txbody.ToString();
                    emailsend.Enviar();

                    retok.Codigo = "100";
                    retok.Mensagem = "OK";
                    retok.dados = null;
                }

            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "getuserByEmail: " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }

            string json = JsonConvert.SerializeObject(retok);
            var res = Request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return res;
            //return Request.CreateResponse(HttpStatusCode.OK, user);
        }
    }

    public class RetOk
    {
        public string Codigo { get; set; }
        public string Mensagem { get; set; }

        public object dados { get; set; }
    }
}
