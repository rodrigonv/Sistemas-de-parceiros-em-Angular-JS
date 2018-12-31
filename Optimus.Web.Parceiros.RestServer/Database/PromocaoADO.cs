using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Database
{
    public class PromocaoADO
    {
        public static List<Promocao> GetPromocao(string cdentifilial, string datasource, string schema)
        {
            Promocao p = new Promocao();
            List<Promocao> lstp = new List<Promocao>();

            string q = string.Format("SELECT CDPROMOCAO,TXURLIMAGEM,TXURLREDIRECT,TXDETALHES,NMPROMOCAO FROM VW_PROMOCAOVALIDA WHERE CDENTIFILIAL={0} AND (CDCANAL=10 OR CDCANAL=0) ORDER BY DTINICIO DESC", cdentifilial);

            DataSet dts = OracleHelper.ExecQuery(q.ToString(), null, "", OracleHelper.BancoOracle.optimus, schema, datasource);

            if (dts.Tables.Count > 0)
            {
                for (int i = 0; i < dts.Tables[0].Rows.Count; i++)
                {
                    p = new Promocao();
                    p.cdpromocao = dts.Tables[0].Rows[i]["CDPROMOCAO"].ToString();
                    p.txurlimagem = dts.Tables[0].Rows[i]["TXURLIMAGEM"].ToString();
                    p.txurlredirect = dts.Tables[0].Rows[i]["TXURLREDIRECT"].ToString();
                    p.nmpromocao = dts.Tables[0].Rows[i]["NMPROMOCAO"].ToString();
                    p.txdetalhes = dts.Tables[0].Rows[i]["TXDETALHES"].ToString();
                    lstp.Add(p);
                }
            }
            return lstp;
        }

        public static Promocao GetPromocaoByCdpromocao(string cdentifilial, string cdpromocao, string datasource, string schema)
        {
            Promocao p = new Promocao();
            List<Promocao> lstp = new List<Promocao>();

            string q = string.Format("SELECT CDPROMOCAO,TXURLIMAGEM,TXURLREDIRECT,TXDETALHES,NMPROMOCAO FROM VW_PROMOCAOVALIDA WHERE CDENTIFILIAL={0} AND (CDCANAL=10 OR CDCANAL=0) AND CDPROMOCAO={1} ORDER BY DTINICIO DESC", cdentifilial, cdpromocao);

            DataSet dts = OracleHelper.ExecQuery(q.ToString(), null, "", OracleHelper.BancoOracle.optimus, schema, datasource);

            if (dts.Tables.Count > 0)
            {
                for (int i = 0; i < dts.Tables[0].Rows.Count; i++)
                {
                    p = new Promocao();
                    p.cdpromocao = dts.Tables[0].Rows[i]["CDPROMOCAO"].ToString();
                    p.txurlimagem = dts.Tables[0].Rows[i]["TXURLIMAGEM"].ToString();
                    p.txurlredirect = dts.Tables[0].Rows[i]["TXURLREDIRECT"].ToString();
                    p.nmpromocao = dts.Tables[0].Rows[i]["NMPROMOCAO"].ToString();
                    p.txdetalhes = dts.Tables[0].Rows[i]["TXDETALHES"].ToString();
                    lstp.Add(p);
                }
            }
            return lstp.FirstOrDefault();
        }



    }

}