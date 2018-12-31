using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Util;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Database
{
    public class UploadParceiroADO
    {
        public int InsertUpload(UploadParceiro up, string datasource, string schema, string cdentifilial)
        {
            int ret = 0;

            string query = string.Format("INSERT INTO uploadparceiro (TXINFO,TXPATH,CDENTIFILIAL,CDFORNECEDOR) VALUES('{0}','{1}',{2},{3})",
                up.txinfo, up.txpath, cdentifilial, up.Cdfornecedor);
            
            Util.OracleHelper.ExecProcedureNonQuery(query, null, System.Data.CommandType.Text, datasource, schema);

            string querycur = "select uploadparceiro_seq.currval as id from dual";

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(querycur, connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret = Convert.ToInt32(reader["id"].ToString());
                    }
                }
            }

            return ret;

        }

        public int DeleteArquivo(UploadParceiro up, string datasource, string schema)
        {
            int ret = 0;
            var root = ConfigurationManager.AppSettings.Get("PATHARQUIVOS") + "\\UPLOAD"; 

            UploadParceiro upret = this.GetUploadById(up, datasource, schema);

            if (System.IO.File.Exists(root + up.txpath))
            {
                System.IO.File.Delete(root + up.txpath);
            }


            string queryfuncupdate = string.Format("delete uploadparceiro where CdUploadParceiro = " + up.CdUploadParceiro);
            ret = Util.OracleHelper.ExecProcedureNonQuery(queryfuncupdate, null, System.Data.CommandType.Text, datasource, schema);

            return ret;
        }

        public int UpdatePath(UploadParceiro up, string datasource, string schema)
        {
            int ret = 0;

            string queryfuncupdate = string.Format("update uploadparceiro set txpath='{0}' where CdUploadParceiro = {1}",up.txpath, up.CdUploadParceiro);
            ret = Util.OracleHelper.ExecProcedureNonQuery(queryfuncupdate, null, System.Data.CommandType.Text, datasource, schema);

            return ret;
        }



        public static List<UploadParceiro> ConsultarNfe(UploadParceiroPesquisa upp,string cdpai,string cdentifilial, string datasource, string schema)
        {
            List<UploadParceiro> lstret = new List<UploadParceiro>();
            UploadParceiro uploadret = new UploadParceiro();
            StringBuilder query = new StringBuilder();
            query.Append("select cduploadparceiro,u.cdfornecedor,(select nmentidade from entidade where cdentidade = u.cdfornecedor) as nmfornecedor,txinfo,cdentifilial, ");
            query.Append(" to_char(dtinclusao,'DD/MM/YYYY') as dtinclusao,txpath from uploadparceiro u where stexcluido = 0 ");
            if (!string.IsNullOrEmpty(upp.dtinicio) && !string.IsNullOrEmpty(upp.dtfim))
            {
                query.AppendLine(string.Format(" and trunc(dtinclusao) between to_date('{0}','DD/MM/YYYY') and to_date('{1}','DD/MM/YYYY')", upp.dtinicio, upp.dtfim));
            }
            query.AppendLine(string.Format("and cdfornecedor = {0}",cdpai));
            query.AppendLine(string.Format("and cdentifilial = {0}", cdentifilial));
            query.AppendLine(" order by dtinclusao desc");
            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(query.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        uploadret = new UploadParceiro();
                        uploadret.CdUploadParceiro = Convert.ToInt32(reader["cduploadparceiro"].ToString());
                        uploadret.dtinclusao = reader["dtinclusao"].ToString();
                        uploadret.nmfornecedor = reader["nmfornecedor"].ToString();
                        uploadret.txpath = "/UPLOAD/" + reader["txpath"].ToString();
                        uploadret.txinfo = reader["txinfo"].ToString();
                        uploadret.Cdfornecedor = Convert.ToInt32(reader["Cdfornecedor"].ToString());
                        uploadret.cdentifilial = Convert.ToInt32(reader["cdentifilial"].ToString());
                        lstret.Add(uploadret);
                    }
                }
            }
            return lstret;
        }
        public UploadParceiro GetUploadById(UploadParceiro up, string datasource, string schema)
        {
            UploadParceiro uploadret = new UploadParceiro();

            StringBuilder query = new StringBuilder();
            query.Append("select cduploadparceiro,u.cdfornecedor,(select nmentidade from entidade where cdentidade = u.cdfornecedor) as nmfornecedor,txinfo,cdentifilial, ");
            query.Append(string.Format(" to_char(dtinclusao,'DD/MM/YYYY') as dtinclusao,txpath from uploadparceiro u where cduploadparceiro ={0} and stexcluido = 0", up.CdUploadParceiro));

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(query.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        uploadret.CdUploadParceiro = Convert.ToInt32(reader["cduploadparceiro"].ToString());
                        uploadret.dtinclusao = reader["dtinclusao"].ToString();
                        uploadret.nmfornecedor = reader["nmfornecedor"].ToString();
                        uploadret.txpath = reader["txpath"].ToString();
                        uploadret.txinfo = reader["txinfo"].ToString();
                        uploadret.Cdfornecedor = Convert.ToInt32(reader["Cdfornecedor"].ToString());
                        uploadret.cdentifilial = Convert.ToInt32(reader["cdentifilial"].ToString());
                    }
                }
            }
            return uploadret;
        }
    }
}