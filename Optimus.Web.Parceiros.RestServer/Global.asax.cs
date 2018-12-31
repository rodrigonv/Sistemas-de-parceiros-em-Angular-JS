using Newtonsoft.Json;
using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Optimus.Web.Parceiros.RestServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new AuthenticationHandler());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthorizeRequest()
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }
    }


    public class AuthenticationHandler : DelegatingHandler
    {
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //request.Headers.Add("Content-Type", "application/json");
            //string aaa = request.Headers.GetValues("Content-Type").FirstOrDefault();
            if (request.Headers.Contains("opttoken"))
            {
                if (request.Headers.GetValues("opttoken").FirstOrDefault() != "9999")
                {
                    try
                    {
                        string token = Util.Cripto.Decrypt(request.Headers.GetValues("opttoken").FirstOrDefault(), ConfigurationManager.AppSettings.Get("CHAVE"));
                        string[] val = token.Split('|');
                        Util.LogUtil.GravaLog(this, "token decri: " + token, "SEMCNPJ", Log.TipoLog.info);
                        HttpContext.Current.Session["cdentifilial"] = val[0];//filial
                        HttpContext.Current.Session["datasource"] = val[1];//datasource
                        HttpContext.Current.Session["schema"] = val[2];//schema
                        //HttpContext.Current.Session["cdentifilial"] = "2";//val[0];//filial
                        //HttpContext.Current.Session["datasource"] = "200.98.64.201";//val[1];//datasource
                        //HttpContext.Current.Session["schema"] = "demo";//val[2];//schema
                        HttpContext.Current.Session["codapp"] = val[3];//codapp
                        HttpContext.Current.Session["cnpj"] = val[4];//cnpj
                        HttpContext.Current.Session["token"] = request.Headers.GetValues("opttoken").FirstOrDefault();//cnpj
                    }
                    catch
                    {
                        Util.LogUtil.GravaLog(this, "token invalido " + request.Headers.GetValues("opttoken").FirstOrDefault(), "SEMCNPJ", Log.TipoLog.erro);
                        return SendError("token invalido.", HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    Util.LogUtil.GravaLog(this, "token 9999 " + request.Headers.GetValues("opttoken").FirstOrDefault(), "SEMCNPJ", Log.TipoLog.info);
                }
            }
            else
            {
                string path = request.RequestUri.AbsolutePath;


                //if (!path.Contains("todos"))
                //{
                if (!path.Contains("config") || !path.Contains("teste"))
                {
                    if (!path.Contains("notification"))
                    {
                        Util.LogUtil.GravaLog(this, "token nao informado ", "SEMCNPJ", Log.TipoLog.erro);

                        return SendError("token nao informado.", HttpStatusCode.Forbidden);
                    }

                }
                //}


            }

            return base.SendAsync(request, cancellationToken);
        }
        private Task<HttpResponseMessage> SendError(string error, HttpStatusCode code)
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent(error);
            response.StatusCode = code;

            return Task<HttpResponseMessage>.Factory.StartNew(() => response);
        }
    }

}
