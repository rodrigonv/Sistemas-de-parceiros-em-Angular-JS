using Optimus.Web.Parceiros.RestServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Database
{
    public class NotificacaoADO
    {
        public static int UpdateNotificacao(string idnotificacao, string datasource, string schema)
        {
            string query = string.Format("UPDATE NOTIFICACOESRETORNO SET STCLICADA = 1 WHERE IDNOTIFICACAO ='{0}'", idnotificacao);
            return OracleHelper.ExecProcedureNonQuery(query, null, System.Data.CommandType.Text, datasource, schema);
        }
    }
}