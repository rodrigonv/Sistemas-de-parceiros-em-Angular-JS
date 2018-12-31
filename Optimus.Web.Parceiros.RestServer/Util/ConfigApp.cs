using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Util
{
    public class ConfigApp
    {
        public static ConfigAppMembers getConfig()
        {
            ConfigAppMembers cf = new ConfigAppMembers();

            cf.cdentifilial = HttpContext.Current.Session["cdentifilial"].ToString();//filial
            cf.datasource = HttpContext.Current.Session["datasource"].ToString(); ;//datasource
            cf.schema = HttpContext.Current.Session["schema"].ToString(); ;//schema
            cf.codapp = HttpContext.Current.Session["codapp"].ToString(); ;//codapp
            cf.Cnpj = HttpContext.Current.Session["cnpj"].ToString();
            cf.token = HttpContext.Current.Session["token"].ToString();
            return cf;
        }
    }

    public class ConfigAppMembers
    {
        public string cdentifilial { get; set; }
        public string datasource { get; set; }

        public string schema { get; set; }

        public string codapp { get; set; }

        public string Cnpj { get; set; }

        public string token { get; set; }
    }
}