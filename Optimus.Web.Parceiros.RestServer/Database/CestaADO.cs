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
    public class CestaADO
    {
        public static List<cesta> GravaCesta(string codempresa, string codped, string cliente, string[] produ, string finaliza, string operador, string codend, string preco, string subsquant, string codforn, string datasource, string schema)
        {
            using (OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleParameter pm = null;
                con.Open();
                using (OracleCommand cm = new OracleCommand("PK_FATURAMENTO.SP_GRAVACESTA", con))
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    for (int i = 0; i < produ.Count(); i++)
                    {
                        cm.Parameters.Clear();


                        pm = cm.Parameters.Add("p_nrpedido", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        if (codped == "")
                            pm.Value = DBNull.Value;
                        else
                            pm.Value = codped;

                        pm = cm.Parameters.Add("p_codfilial", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = codempresa;

                        pm = cm.Parameters.Add("p_codcliente", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = cliente;

                        var dadosprod = produ[i].Split('|');
                        pm = cm.Parameters.Add("p_codprod", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = dadosprod[0];// produ[i].Substring(0, produ[i].IndexOf("|"));

                        pm = cm.Parameters.Add("p_quant", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = dadosprod[1];// produ[i].Substring(produ[i].IndexOf("|") + 1, produ[i].IndexOf("]") - produ[i].IndexOf("|") - 1);

                        pm = cm.Parameters.Add("p_nomeprod", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = dadosprod[2];// produ[i].Substring(produ[i].IndexOf("]") + 1, produ[i].Length - produ[i].IndexOf("]") - 1);

                        pm = cm.Parameters.Add("p_finaliza", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = finaliza;

                        pm = cm.Parameters.Add("p_operador", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = operador;

                        pm = cm.Parameters.Add("p_codend", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = codend;

                        pm = cm.Parameters.Add("p_nrpedretorno", OracleDbType.Double);
                        pm.Direction = System.Data.ParameterDirection.Output;

                        pm = cm.Parameters.Add("p_preco", OracleDbType.Double);
                        pm.Direction = System.Data.ParameterDirection.Input;

                        var fmt = new System.Globalization.NumberFormatInfo();


                        if (string.IsNullOrEmpty(preco))
                        {
                            var preconegative = "−1";
                            fmt.NegativeSign = "−";

                            var number = double.Parse(preconegative, fmt);
                            pm.Value = number;
                        }
                        else
                        {
                            if (preco.Contains(','))
                            {
                                preco = preco.Replace(',', '.');
                            }
                            fmt.CurrencyDecimalDigits = 3;
                            pm.Value = Convert.ToDouble(preco, fmt);
                        }



                        //pm = cm.Parameters.Add("p_subsquant", OracleDbType.Double);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = Convert.ToDouble(subsquant);



                        cm.ExecuteNonQuery();
                        codped = cm.Parameters["p_nrpedretorno"].Value.ToString();
                    }
                }
                double margem = 0;
                //using (OracleCommand cm2 = new OracleCommand("select nvl(pcmargemminima,0) pcmargemminima from funcionario where cdfuncionario=" + operador, con))
                //{
                //    using (OracleDataReader dr2 = cm2.ExecuteReader())
                //    {
                //        while (dr2.Read())
                //        {
                //            margem = Convert.ToDouble(dr2["pcmargemminima"].ToString());
                //        }
                //    }
                //}

                List<cesta> lista = new List<cesta>();

                lista = RetornaCesta(codped, codforn, datasource, schema);

                return lista;
            }
        }


        public static List<cesta> RetornaCesta(string codped, string codforn, string datasource, string schema)
        {
            using (OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                con.Open();

                double margem = 0;

                List<cesta> lista = new List<cesta>();


                StringBuilder query = new StringBuilder();

                /*
                 preconota = digitado
                 preco = tabela
                 percentualcomissao = da tabela do fornecedor
                 percentualcomissaocalc = calcular a comissao sobre o total de itens
                 */

                #region Calculando o percentual da tabela fornecedor e com valor maximo de produto em relacao ao total
                //string PARAM_PERCENTUAL_MAXIMO_VALOR_PRODUTO = ConfigurationManager.AppSettings.Get("PERC_MAXIMO_VALOR_PRODUTO");
                string PERC_COMISSAO_PRODUTO = ConfigurationManager.AppSettings.Get("PERC_COMISSAO_PRODUTO");
                //query.AppendLine("  select NVL(Round((t.qtfaturada * t.precovenda) - (t.qtfaturada * t.preconota),2),0) as vrdesconto2, ");
                //query.AppendLine("  TRIM(TO_CHAR(t.preconota,'L999G999G999G999D99','NLS_NUMERIC_CHARACTERS = '',. ''NLS_CURRENCY = '' '' ')) as preconotaview,");
                //query.AppendLine("  TRIM(TO_CHAR(vrcontabil,'L999G999G999G999D99','NLS_NUMERIC_CHARACTERS = '',. ''NLS_CURRENCY = '' '' ')) as vrcontabilview,");
                //query.AppendLine("  (select sum(qtfaturada) from tmp_cesta where codped  = " + codped + ") as qttotal,");
                //query.AppendLine("  (select sum(qtfaturada * preconota) from tmp_cesta where codped  = " + codped + ")  vrtotal,");
                //query.AppendLine("  (select sum(NVL(Round((qtfaturada * precovenda) - (qtfaturada * preconota),2),0)) from tmp_cesta where codped  = " + codped + ") as vrtotaldesc, ");
                //query.AppendLine("  to_char(t.datapedido,'DD/MM/YYYY') as datapedido2, ");
                //query.AppendLine("  NVL((select perccomiparceiro from fornecedor where cdentifornecedor = " + codforn + "),0) percicomissao,");
                //query.AppendLine("  NVL(((select  NVL(perccomiparceiro,0) from fornecedor where cdentifornecedor = " + codforn + ")  * (select sum(qtfaturada * preconota) from tmp_cesta where codped  = " + codped + ") / 100),0) as comissao, ");
                //query.AppendLine("  ROUND((t.precovenda * " + PARAM_PERCENTUAL_MAXIMO_VALOR_PRODUTO + " /100) + t.precovenda,2) as precomaximo, ");
                //query.AppendLine("  ROUND( t.precovenda - (t.precovenda * NVL((select perccomiparceiro from fornecedor where cdentifornecedor = " + codforn + "),0) /100),2) as precominimo,t.* ");
                //query.AppendLine("  from tmp_cesta t  ");
                //query.AppendLine("  where t.quantidade > 0 and t.codped = " + codped + " order by t.id desc ");
                #endregion

                string PARAM_PERCENTUAL_MAXIMO_VALOR_PRODUTO = ConfigurationManager.AppSettings.Get("PERC_MAXIMO_VALOR_PRODUTO");

                query.AppendLine("  select  ");
                query.AppendLine("  TRIM(TO_CHAR(t.preconota,'L999G999G999G999D99','NLS_NUMERIC_CHARACTERS = '',. ''NLS_CURRENCY = '' '' ')) as preconotaview, ");
                query.AppendLine("  TRIM(TO_CHAR(vrcontabil,'L999G999G999G999D99','NLS_NUMERIC_CHARACTERS = '',. ''NLS_CURRENCY = '' '' ')) as vrcontabilview, ");
                query.AppendLine("  (select sum(qtfaturada) from tmp_cesta where codped  = " + codped + ") as qttotal, ");
                query.AppendLine("  (select sum(qtfaturada * preconota) from tmp_cesta where codped  = " + codped + ") vrtotal, ");
                query.AppendLine("  to_char(t.datapedido,'DD/MM/YYYY') as datapedido2,  ");

                query.AppendLine("  ft_comissaoparceiro(t.precovenda*qtfaturada,t.preconota* qtfaturada ,7,1,1,1) as totalcomissao,  ");
                query.AppendLine("  ft_comissaoparceiro(t.precovenda*qtfaturada,t.preconota* qtfaturada ,7,1,1,0) as over, ");
                query.AppendLine("  ft_comissaoparceiro(t.precovenda*qtfaturada,t.preconota* qtfaturada ,7,1,0,1) as comissao80,");

                query.AppendLine("  t.*  ");
                query.AppendLine("  from tmp_cesta t   ");
                query.AppendLine("  where t.quantidade > 0 and t.codped = " + codped + " order by t.id desc  ");

                //query.AppendLine("  select ");
                //query.AppendLine("  TRIM(TO_CHAR(t.preconota,'L999G999G999G999D99','NLS_NUMERIC_CHARACTERS = '',. ''NLS_CURRENCY = '' '' ')) as preconotaview,");
                //query.AppendLine("  TRIM(TO_CHAR(vrcontabil,'L999G999G999G999D99','NLS_NUMERIC_CHARACTERS = '',. ''NLS_CURRENCY = '' '' ')) as vrcontabilview, ");
                //query.AppendLine("  (select sum(qtfaturada) from tmp_cesta where codped  = " + codped + ") as qttotal,");
                //query.AppendLine("  (select sum(qtfaturada * preconota) from tmp_cesta where codped  = " + codped + ") vrtotal,");
                //query.AppendLine("  to_char(t.datapedido,'DD/MM/YYYY') as datapedido2, ");
                //query.AppendLine("  ROUND(((t.preconota - t.precovenda) * 80 /100),2) as comissao80, ");
                //query.AppendLine("  sum(qtfaturada * preconota) as vrtotalprod, ");
                //query.AppendLine("  t.codped,t.codproduto,t.nomeprod,t.codcliente,t.preconota,t.vrdesconto,t.estoque,t.vrcontabil,t.quantidade,");
                //query.AppendLine("  t.quantfalta,t.qtfaturada,t.cod_pedgravado,t.codend,t.promocao,t.operador,t.codempresa,t.precovenda");
                //query.AppendLine("  from tmp_cesta t ");
                //query.AppendLine("  where t.quantidade > 0 and t.codped = " + codped + " ");
                //query.AppendLine("  group by t.preconota,vrcontabil ,t.datapedido,t.precovenda,t.codped,t.codproduto,t.nomeprod,t.codcliente,t.preconota,t.vrdesconto,t.estoque,t.vrcontabil,");
                //query.AppendLine("  t.quantidade,t.quantfalta,t.qtfaturada,t.cod_pedgravado,t.codend,t.promocao,t.operador,t.codempresa,t.precovenda,t.id ");
                //query.AppendLine("  order by t.id desc ");


                using (OracleCommand cm1 = new OracleCommand(query.ToString(), con))
                {
                    using (OracleDataReader dr = cm1.ExecuteReader())
                    {

                        //double qtdeTotal = 0;
                        //double VrTotal = 0;
                        //double VrTotalDesc = 0;

                        while (dr.Read())
                        {
                            string lob = "";
                            if (!string.IsNullOrEmpty(dr["perlob"].ToString()))
                            {
                                if (Convert.ToDouble(dr["perlob"].ToString()) >= margem)
                                    lob = dr["perlob"].ToString();
                                else
                                    lob = dr["perlob"].ToString() + "*";
                            }


                            //qtdeTotal += Convert.ToDouble(dr["qtfaturada"].ToString().Trim());
                            //VrTotal += Convert.ToDouble(dr["vrcontabil"].ToString().Trim());
                            //VrTotalDesc += RJS.Optimus.Biblioteca.RJSOptimusConverter.ToDouble(dr["vrdesconto2"].ToString().Trim(), 0);

                            lista.Add(
                                new cesta(dr["codped"].ToString(),
                                    dr["codproduto"].ToString(),
                                dr["nomeprod"].ToString(),
                                dr["codcliente"].ToString(),
                                dr["preconota"].ToString(),
                                dr["vrdesconto"].ToString(),
                                dr["estoque"].ToString(),
                                dr["vrcontabil"].ToString(),
                                dr["quantidade"].ToString(),
                                dr["quantfalta"].ToString(),
                                dr["qtfaturada"].ToString(),
                                dr["cod_pedgravado"].ToString(),
                                dr["codend"].ToString(),
                                dr["promocao"].ToString(),
                                lob, dr["operador"].ToString(),
                                 "0",
                                 dr["comissao80"].ToString(),
                                dr["codempresa"].ToString(),
                                dr["precovenda"].ToString(),
                                dr["datapedido2"].ToString(),
                                dr["preconotaview"].ToString(),
                                dr["vrcontabilview"].ToString(),
                                dr["qttotal"].ToString(),
                                dr["vrtotal"].ToString(),
                                "0",
                               "0",
                                "0", Convert.ToDouble(dr["over"].ToString()), Convert.ToDouble(dr["totalcomissao"].ToString())
                                ));
                        }

                        //if (lista.Count > 0)
                        //{
                        //    lista[0].QuantidadeTotalCesta = qtdeTotal.ToString();
                        //    lista[0].TotalCesta = VrTotal.ToString();
                        //    lista[0].TotalDescontoCesta = VrTotalDesc.ToString();
                        //}

                    }

                }
                return lista;
            }
        }


        public static cestatotais RetornaTotaisCesta(string codped, string datasource, string schema)
        {
            using (OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                con.Open();

                cestatotais cestatot = new cestatotais();
                StringBuilder query = new StringBuilder();

                query.AppendLine(" select codped,sum(vrcontabil) vrtotal, sum(quantidade) qtdetotal, sum(nvl(pw.nrpeso,0) * quantidade )/1000 as peso from tmp_cesta tmp, prodweb pw ");
                query.AppendLine(" where tmp.codproduto = pw.cdproduto and tmp.codped=" + codped);
                query.AppendLine(" group by codped");

                using (OracleCommand cm1 = new OracleCommand(query.ToString(), con))
                {
                    using (OracleDataReader dr = cm1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cestatot.idpedido = dr["codped"].ToString();
                            //cestatot.codproduto = dr["codproduto"].ToString();
                            cestatot.vrtotalpedido = Convert.ToDouble(dr["vrtotal"].ToString());
                            cestatot.totalitens = Convert.ToDouble(dr["qtdetotal"].ToString());
                            cestatot.pesototal = Convert.ToDouble(dr["peso"].ToString());
                        }
                    }

                }
                return cestatot;
            }
        }


        public static List<cesta> CriarCesta(string codped, string datasource, string schema)
        {
            string queryExiste = "select nvl(max(codped),0) as t  from tmp_cesta where cod_pedgravado =" + codped;

            StringBuilder query = new StringBuilder();

            query.AppendLine("    select pf.cadgcdempresa,pf.pefanrpedido,to_char(ip.prodcdproduto)||'|'|| ip.pefiqtfaturada||'|'||      ");
            query.AppendLine("    ( select max(nmproduto) from vw_pesqprodean where cdproduto = ip.prodcdproduto) as produ, 'S' as finaliza,  ");
            query.AppendLine("    pf.cagecdcadastro as cliente, ");
            query.AppendLine("    pf.pefacdenderecoentrega as cdendereco,");
            query.AppendLine("    pefivrpreconota,pf.pefacdoperadortmkt");
            query.AppendLine("    from PEDIDOFATURADO pf ,PEDIDOFATURADOITEM ip where");
            query.AppendLine("    pf.pefanrpedido = ip.pefanrpedido");
            query.AppendLine("    and pf.pefanrpedido = " + codped);

            List<cesta> lista = new List<cesta>();

            //try
            //{
            using (OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                con.Open();

                using (OracleCommand cm = new OracleCommand(queryExiste, con))
                {
                    cm.CommandType = System.Data.CommandType.Text;
                    int codpedcesta = Convert.ToInt32(cm.ExecuteScalar());
                    if (codpedcesta == 0)
                    {
                        using (OracleCommand cm2 = new OracleCommand(query.ToString(), con))
                        {
                            cm2.CommandType = System.Data.CommandType.Text;

                            using (OracleDataReader dr2 = cm2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    lista = GravaCesta(dr2["cadgcdempresa"].ToString(), dr2["pefanrpedido"].ToString(), dr2["cliente"].ToString(), new[] { dr2["produ"].ToString() },
                                        "S", dr2["pefacdoperadortmkt"].ToString(), dr2["cdendereco"].ToString(), dr2["pefivrpreconota"].ToString(), "0", dr2["pefacdoperadortmkt"].ToString(), datasource, schema);
                                }
                            }
                            //lista = RetornaCesta(codped, "", datasource, schema);
                        }
                    }
                    else
                    {
                        lista = RetornaCesta(codpedcesta.ToString(), "", datasource, schema);
                    }

                }
            }
            //}
            //catch (Exception ex)
            //{
            //    Util.Log.For(this, ConfigurationManager.AppSettings.Get("pathlog")).Error("CriarCesta ->" + ex.ToString());
            //    throw;
            //}
            return lista;
        }


       

    }
}