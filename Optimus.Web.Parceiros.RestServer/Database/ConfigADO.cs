using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Database
{
    public class ConfigADO
    {
        public static ConfigAppobj RetornaConfig(string packagename)
        {
            ConfigAppobj ret = new ConfigAppobj();

            string query = string.Format("SELECT * FROM CONFIGAPP WHERE TXPACKAGENAME = '{0}'", packagename);

            DataSet dts = OracleHelper.ExecQuery(query, null, null, ConfigurationManager.AppSettings.Get("conintegracao"));
            string datasource = string.Empty;
            string schema = string.Empty;

            if (dts.Tables.Count > 0)
            {
                foreach (DataRow dr in dts.Tables[0].Rows)
                {
                    ret.optcodapp = dr["CDAPP"].ToString();
                    ret.optcssfundotopo = dr["txcssfundotopo"].ToString();
                    ret.optcsstopo = dr["txcsstopo"].ToString();
                    ret.optdadosempresa = dr["stmostradadosempresa"].ToString();
                    ret.optemailcontato = dr["txcontaemail"].ToString();
                    ret.optfilial = dr["cdentifilial"].ToString();
                    ret.optpkgname = dr["txpackagename"].ToString();
                    ret.optsenderidandroid = dr["txsenderidandroid"].ToString();
                    ret.optsenderidios = dr["txsenderidios"].ToString();
                    if (!string.IsNullOrEmpty(dr["txtelefones"].ToString()))
                    {
                        if (dr["txtelefones"].ToString().Contains('|'))
                        {
                            string[] tels = dr["txtelefones"].ToString().Split('|');
                            foreach (var item in tels)
                            {
                                ret.opttelefones.Add(new telefone(item));
                            }
                        }
                        else
                        {
                            ret.opttelefones.Add(new telefone(dr["txtelefones"].ToString()));
                        }

                    }
                    ret.opttoken = dr["txtoken"].ToString();
                    ret.opttxhoraatendimento = dr["txhoraatendimento"].ToString();
                    ret.opturllogo = dr["txurllogo"].ToString();
                    if (ret.optdadosempresa == "1")
                    {
                        datasource = dr["txdatasource"].ToString();
                        schema = dr["txesquema"].ToString();
                    }
                }

                if (ret.optdadosempresa == "1")
                {
                    //buscar a filial
                    string q = string.Format("select cnpj,nmrazaosocial,cdcep,txcidade,txbairro,txlogradouro,txnumero,txcomplemento,cduf from vw_filial where cdentifilial = {0}", ret.optfilial);
                    DataSet dtsf = OracleHelper.ExecQuery(q.ToString(), null, "", OracleHelper.BancoOracle.optimus, schema, datasource);
                    if (dtsf.Tables.Count > 0)
                    {
                        if (dtsf.Tables[0].Rows.Count > 0 )
                        {
                            ret.filial.bairro = dtsf.Tables[0].Rows[0]["txbairro"].ToString().ToUpper();
                            ret.filial.cep = Convert.ToUInt64(dtsf.Tables[0].Rows[0]["cdcep"].ToString()).ToString(@"00000\-000");
                            ret.filial.cnpj = Convert.ToUInt64(dtsf.Tables[0].Rows[0]["cnpj"].ToString()).ToString(@"00\.000\.000\/0000\-00");
                            ret.filial.complemento = dtsf.Tables[0].Rows[0]["txcomplemento"].ToString().ToUpper();
                            ret.filial.logradouro = dtsf.Tables[0].Rows[0]["txlogradouro"].ToString().ToUpper();
                            ret.filial.NmRazao = dtsf.Tables[0].Rows[0]["nmrazaosocial"].ToString().ToUpper();
                            ret.filial.numero = dtsf.Tables[0].Rows[0]["txnumero"].ToString();
                            ret.filial.uf = dtsf.Tables[0].Rows[0]["cduf"].ToString().ToUpper();
                            ret.filial.cidade = dtsf.Tables[0].Rows[0]["txcidade"].ToString().ToUpper();
                        }
                    }

                }
            }


            return ret;
        }


       
    }
}