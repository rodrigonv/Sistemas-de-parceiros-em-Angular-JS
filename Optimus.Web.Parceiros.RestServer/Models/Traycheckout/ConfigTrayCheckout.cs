using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models.Traycheckout
{
    public static class ConfigTrayCheckout
    {
        public static bool environmentProduction = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("TRAYCHECKOUTPRODUCAO"));
        //public static string tokenAccount = "cbc76d42e047a01";//sandbox
        public static string tokenAccount =ConfigurationManager.AppSettings.Get("TOKENTRAYCHECKOUT"); //"27348691e1ed0c1";

        public static string urlNotification = ConfigurationManager.AppSettings.Get("URLNOTIFICATIONTRAY").Replace("#","&");// "https://parceiros.multivisi.com.br/rest/traycheckout/notification?cp={0}&opttoken={1}"; // ex: http://localhost:59333/notification


        public static string UrlSplit()
        {
            //Url Production
            if (environmentProduction)
            {
                return "https://api.traycheckout.com.br/api/seller_splits/simulate_split";
            }
            else
            {
                //Url Sandbox (ambiente de testes)
                return "http://api.sandbox.checkout.tray.com.br/api/seller_splits/simulate_split";
            }
           
        }

        public static string UrlGetByToken()
        {
            //Url Production
            if (environmentProduction)
            {
                return "https://api.traycheckout.com.br/v2/transactions/get_by_token";

            }
            else
            {
                //Url Sandbox (ambiente de testes)
                return "https://api.sandbox.traycheckout.com.br/v2/transactions/get_by_token";
            }
          
                    
        }

        /*public static string UrlGetByTokenV1()
        {
            //Url Production
            if (environmentProduction)
            {
                return "https://api.traycheckout.com.br/v1/transactions/get_by_token";
            }
            //Url Sandbox (ambiente de testes)
            return "https://api.sandbox.checkout.tray.com.br/api/v1/transactions/get_by_token";
        }
        */

        public static string UrlCreateTransaction()
        {
            //Url Production
            if (environmentProduction)
            {
                return "https://api.traycheckout.com.br/v2/transactions/pay_complete";
            }
            else
            {
                //Url Sandbox (ambiente de testes)
                return "https://api.sandbox.traycheckout.com.br/v2/transactions/pay_complete";
            }
           
        }
        /// <summary>
        /// Ambiente de Testes	https://api.sandbox.traycheckout.com.br/v1/transactions/simulate_splitting
        ///Ambiente de Produção	https://api.traycheckout.com.br/v1/transactions/simulate_splitting
        /// </summary>
        /// <returns></returns>
        public static string UrlSimulateSplittingRest()
        {
            //Url Production
            if (environmentProduction)
            {
                return "https://api.traycheckout.com.br/v1/transactions/simulate_splitting";
            }
            else
            {
                //Url Sandbox (ambiente de testes)
                return "https://api.sandbox.traycheckout.com.br/v1/transactions/simulate_splitting";
            }
          
        }

        public static string UrlTransactionRest()
        {
            //Url Production
            if (environmentProduction)
            {
                return "https://api.traycheckout.com.br/v1/transactions/";
            }
            else
            {
                //Url Sandbox (ambiente de testes)
                return "https://api.sandbox.traycheckout.com.br/v1/transactions/";
            }
           
        }

    }
}