using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Util;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Database
{
    public class BannerADO
    {
        public int CadastrarBanner(Banner banner, string datasource, string schema)
        {
            int ret = 0;
            if (banner.cdbanner == 0)
            {

                string query = string.Format("INSERT INTO bannerparceiro (NMPROMOCAO,DTINICIO,DTFIM,TXURLIMAGEM,TXURLREDIRECT,CDENTIFILIAL,STATIVO) VALUES('{0}',TO_DATE('{1}', 'DD/MM/YYYY'),TO_DATE('{2}', 'DD/MM/YYYY'),'{3}','{4}',{5},{6})",
                    banner.nmbanner, banner.dtinicio, banner.dtfim, banner.txurlimagem, banner.txurlredirect, 1, banner.stativo ? "1" : "0");
                ret = OracleHelper.ExecProcedureNonQuery(query, null, System.Data.CommandType.Text, datasource, schema);

            }
            else
            {
                string queryfuncupdate = string.Format("update bannerparceiro set NMPROMOCAO = '{0}',DTINICIO =TO_DATE('{1}', 'DD/MM/YYYY'),DTFIM =TO_DATE('{2}', 'DD/MM/YYYY'),TXURLIMAGEM = '{3}',TXURLREDIRECT='{4}',STATIVO = {5} where cdbannerparceiro = {6}",
                    banner.nmbanner, banner.dtinicio, banner.dtfim, banner.txurlimagem, banner.txurlredirect, banner.stativo ? "1" : "0", banner.cdbanner);
                ret = Util.OracleHelper.ExecProcedureNonQuery(queryfuncupdate, null, System.Data.CommandType.Text, datasource, schema);

            }
            return ret;

        }

        public int DeleteBanner(int cdbanner, string datasource, string schema)
        {
            int ret = 0;

            string queryfuncupdate = string.Format("update bannerparceiro set STEXCLUIDO = 1 where cdbannerparceiro = " + cdbanner);
            ret = Util.OracleHelper.ExecProcedureNonQuery(queryfuncupdate, null, System.Data.CommandType.Text, datasource, schema);

            return ret;
        }

        public List<Banner> RetornaBannersAtivos(string datasource, string schema)
        {
            List<Banner> lstret = new List<Banner>();

            string query = "select cdbannerparceiro,txurlimagem,txurlredirect from bannerparceiro where trunc(sysdate) BETWEEN trunc(dtinicio) and trunc(dtfim) and stexcluido=0 and stativo=1";

            Banner banner = new Banner();
            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(query, connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        banner = new Banner();
                        banner.cdbanner = banner.cdbanner = Convert.ToInt32(reader["cdbannerparceiro"].ToString());
                        banner.txurlimagem = reader["txurlimagem"].ToString();
                        banner.txurlredirect = reader["txurlredirect"].ToString();
                        lstret.Add(banner);
                    }
                }
            }
            return lstret;
        }

        public List<Banner> ConsultarBanner(Banner banner, string datasource, string schema)
        {
            List<Banner> lstret = new List<Banner>();

            StringBuilder query = new StringBuilder();
            query.Append("select cdbannerparceiro,nmpromocao,to_char(dtinicio,'DD/MM/YYYY') as dtinicio,to_char(dtfim,'DD/MM/YYYY') as dtfim,stexcluido, dtcadastro,dtexclusao, txurlimagem,txurlredirect,cdentifilial, cdusuario,stativo ");
            query.Append(" from bannerparceiro where stexcluido =0 ");
            if (!string.IsNullOrEmpty(banner.dtinicio) && !string.IsNullOrEmpty(banner.dtfim))
            {
                query.AppendLine(string.Format("and trunc(dtinicio) between to_date('{0}','DD/MM/YYYY') and to_date('{1}','DD/MM/YYYY')", banner.dtinicio, banner.dtfim));
            }
            if (!string.IsNullOrEmpty(banner.nmbanner))
            {
                query.AppendFormat(" and upper(nmpromocao) like upper('%{0}%')", banner.nmbanner);
            }
            // trunc(sysdate) BETWEEN dtinicio and dtfim");

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(query.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        banner = new Banner();
                        banner.cdbanner = Convert.ToInt32(reader["cdbannerparceiro"].ToString());
                        banner.dtfim = reader["dtfim"].ToString();
                        banner.dtinicio = reader["dtinicio"].ToString();
                        banner.nmbanner = reader["nmpromocao"].ToString();
                        banner.stativo = reader["stativo"].ToString() == "1";
                        banner.txurlimagem = reader["txurlimagem"].ToString();
                        banner.txurlredirect = reader["txurlredirect"].ToString();
                        lstret.Add(banner);
                    }
                }
            }
            return lstret;
        }
        public Banner GetBannerById(Banner banner, string datasource, string schema)
        {
            Banner bannerret = new Banner();

            StringBuilder query = new StringBuilder();
            query.Append("select cdbannerparceiro,nmpromocao,to_char(dtinicio,'DD/MM/YYYY') as dtinicio,to_char(dtfim,'DD/MM/YYYY') as dtfim,stexcluido, dtcadastro,dtexclusao, txurlimagem,txurlredirect,cdentifilial, cdusuario,stativo ");
            query.Append(string.Format(" from bannerparceiro where cdbannerparceiro={0}", banner.cdbanner));

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(query.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bannerret.cdbanner = Convert.ToInt32(reader["cdbannerparceiro"].ToString());
                        bannerret.dtfim = reader["dtfim"].ToString();
                        bannerret.dtinicio = reader["dtinicio"].ToString();
                        bannerret.nmbanner = reader["nmpromocao"].ToString();
                        bannerret.stativo = reader["stativo"].ToString() == "1";
                        bannerret.txurlimagem = reader["txurlimagem"].ToString();
                        bannerret.txurlredirect = reader["txurlredirect"].ToString();
                    }
                }
            }
            return bannerret;
        }

    }
}