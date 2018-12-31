using Optimus.Web.Parceiros.RestServer.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Database
{
    public class ParceiroADO
    {
        public static List<ParceiroComissao> RetornaComissaoFuncAggByParam(ParceiroSearch ps, string datasource, string schema)
        {
            List<ParceiroComissao> lstResult = new List<ParceiroComissao>();

            StringBuilder querycli = new StringBuilder();

            querycli.AppendLine(string.Format(" select '{0}'as dtinicio,'{1}' as dtfim, sum(comissao) as comissao, sum(vrtotal) as vrtotal,func  from vw_parc_com_agg_total ", ps.dtinicio, ps.dtfim));
            querycli.AppendLine(" where 1=1 ");

            if (!string.IsNullOrEmpty(ps.dtinicio) && !string.IsNullOrEmpty(ps.dtfim))
            {
                querycli.AppendLine(string.Format("and to_date(dtpedido,'DD/MM/YYYY') between to_date('{0}','DD/MM/YYYY') and to_date('{1}','DD/MM/YYYY')", ps.dtinicio, ps.dtfim));
            }

            querycli.AppendLine(string.Format(" and cdentidadepai = {0} ", ps.cdentidadepai));
            

            //if (ps.cdentidadepai > 0)
            //{
            //    querycli.AppendLine(string.Format(" and cdentidadepai = {0}", ps.cdentidadepai));
            //}

            if (ps.cdfunc > 0)
            {
                querycli.AppendLine(string.Format(" and cdfunc = {0}", ps.cdfunc));
            }


            querycli.AppendLine(" group by func ");
            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(querycli.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    ParceiroComissao psc = null;
                    while (reader.Read())
                    {
                        psc = new ParceiroComissao();
                        //psc.cdentidadepai = Convert.ToInt32(reader["cdentidadepai"].ToString());
                        //psc.cdfunc = reader["cdfunc"].ToString();
                        //psc.cdstatuspedido = Convert.ToInt32(reader["cdstatuspedido"].ToString());
                        psc.comissao = Convert.ToDouble(reader["comissao"].ToString());
                        psc.vrtotal = Convert.ToDouble(reader["vrtotal"].ToString());
                        psc.func = reader["func"].ToString();
                        //psc.dtpedido = reader["dtpedido"].ToString();
                        lstResult.Add(psc);
                    }
                }
            }
            return lstResult.OrderBy(a => a.dtpedido).ToList();
        }
    }
}