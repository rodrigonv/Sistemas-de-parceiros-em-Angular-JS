using Optimus.Web.Parceiros.RestServer.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Database
{
    public class BasicoADO
    {
        /*
              select cdtipotelefone,nmtipotelefone from tipotelefone where stexcluido = 0
            select cdtipoemail,nmtipoemail from tipoemail where stexcluido = 0
            select cdtipocontato,nmtipocontato from tipocontato where stexcluido=0
            select cdtipoendereco,nmtipoendereco from tipoendereco where stexcluido=0
         */

        public static List<Basico> RetornaTipoTelefone(string datasource, string schema)
        {
            List<Basico> lstResult = new List<Basico>();

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand("select cdtipotelefone,nmtipotelefone from tipotelefone where stexcluido = 0 ", connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    Basico bas = null;
                    while (reader.Read())
                    {
                        bas = new Basico();
                        bas.nome = reader["nmtipotelefone"].ToString();
                        bas.valor = reader["cdtipotelefone"].ToString();
                        lstResult.Add(bas);
                    }
                }
            }
            return lstResult.OrderBy(a => a.nome).ToList();
        }
        public static List<Basico> RetornaTipoEmail(string datasource, string schema)
        {
            List<Basico> lstResult = new List<Basico>();

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand("select cdtipoemail,nmtipoemail from tipoemail where stexcluido = 0 ", connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    Basico bas = null;
                    while (reader.Read())
                    {
                        bas = new Basico();
                        bas.nome = reader["nmtipoemail"].ToString();
                        bas.valor = reader["cdtipoemail"].ToString();
                        lstResult.Add(bas);
                    }
                }
            }
            return lstResult.OrderBy(a => a.nome).ToList();
        }

        public static List<Basico> RetornaTipoContato(string datasource, string schema)
        {
            List<Basico> lstResult = new List<Basico>();

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand("select cdtipocontato,nmtipocontato from tipocontato where stexcluido = 0 ", connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    Basico bas = null;
                    while (reader.Read())
                    {
                        bas = new Basico();
                        bas.nome = reader["nmtipocontato"].ToString();
                        bas.valor = reader["cdtipocontato"].ToString();
                        lstResult.Add(bas);
                    }
                }
            }
            return lstResult.OrderBy(a => a.nome).ToList();
        }

        public static List<Basico> RetornaTipoEndereco(string datasource, string schema)
        {
            List<Basico> lstResult = new List<Basico>();

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand("select cdtipoendereco,nmtipoendereco from tipoendereco where stexcluido = 0 ", connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    Basico bas = null;
                    while (reader.Read())
                    {
                        bas = new Basico();
                        bas.nome = reader["nmtipoendereco"].ToString();
                        bas.valor = reader["cdtipoendereco"].ToString();
                        lstResult.Add(bas);
                    }
                }
            }
            return lstResult.OrderBy(a => a.nome).ToList();
        }


        public static List<Basico> RetornaStatusPedido(string datasource, string schema)
        {
            List<Basico> lstResult = new List<Basico>();

            string query = "select CDSTATUSPEDIDO, TO_CHAR(CDSTATUSPEDIDO) || ' - ' || NMSTATUS AS NMSTATUSDSC from statuspedido WHERE CDSTATUSPEDIDO != 13 order by CDSTATUSPEDIDO";



            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(query, connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    Basico statusped = null;
                    while (reader.Read())
                    {
                        statusped = new Basico();
                        statusped.valor = reader["CDSTATUSPEDIDO"].ToString();
                        statusped.nome = reader["NMSTATUSDSC"].ToString();

                        lstResult.Add(statusped);
                    }
                }
            }
            return lstResult;
        }
    }
}