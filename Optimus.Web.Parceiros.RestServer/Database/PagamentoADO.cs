using Optimus.Web.Parceiros.RestServer.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Database
{
    public class PagamentoADO
    {
        public static List<pedidopagamento> GravaPagamento(string codped, string forma, string valorpago, string recebidodinheiro, string autorizado,
      string parcelas, string cancelar, string codpag, string operador, string nrtitulo, string nrautorizacao, string payerid, string retpagtotrans, string statuspagto, string bandeira, string datasource, string schema, string cdformapagamento)
        {
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");

            OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource));
            OracleParameter pm = null;
            try
            {
                con.Open();

                OracleCommand cm = new OracleCommand("PK_FATURAMENTO.SP_PAGAMENTO", con);
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.Parameters.Clear();

                pm = cm.Parameters.Add("P_NRPEDIDO", OracleDbType.Double);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = Convert.ToDouble(codped);

                pm = cm.Parameters.Add("P_FORMA", OracleDbType.Double);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = Convert.ToDouble(forma);

                pm = cm.Parameters.Add("P_VALORPAGO", OracleDbType.Double);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = Convert.ToDouble(valorpago.Replace(',', '.'), cultureInfo);

                pm = cm.Parameters.Add("P_RECEBIDODINHEIRO", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                if (recebidodinheiro == "")
                    pm.Value = DBNull.Value;
                else
                    pm.Value = recebidodinheiro;

                pm = cm.Parameters.Add("P_AUTORIZADO", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = autorizado;

                pm = cm.Parameters.Add("P_PARCELAS", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = parcelas;

                pm = cm.Parameters.Add("P_CANCELAR", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = cancelar;

                pm = cm.Parameters.Add("P_CODPAG", OracleDbType.Double);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = Convert.ToDouble(codpag);

                pm = cm.Parameters.Add("P_OPERADOR", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = operador;

                pm = cm.Parameters.Add("P_NRTITULO", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = "A";
                //A titulo principal - FRETE valor que nao entra no faturamento
                pm = cm.Parameters.Add("P_PEFPRECIBOLOJA", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = pm.Value = DBNull.Value;
                pm = cm.Parameters.Add("P_PEFPRECIBOCLIENTE", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = retpagtotrans;

                pm = cm.Parameters.Add("P_PEFPNRAUTORIZACAO", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = nrautorizacao;
                pm = cm.Parameters.Add("P_ESITEFUSN", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = payerid;
                pm = cm.Parameters.Add("P_SITEFUSN", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = bandeira;
                pm = cm.Parameters.Add("P_DTPAGCARTAOESITEF", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = DBNull.Value;
                pm = cm.Parameters.Add("P_HOSTUSN", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = statuspagto;
                pm = cm.Parameters.Add("P_MESSAGEESITEF", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = DBNull.Value;
                pm = cm.Parameters.Add("P_NITESITEF", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = DBNull.Value;
                pm = cm.Parameters.Add("P_TRANSACTIONSTATUSESITEF", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = DBNull.Value;
                pm = cm.Parameters.Add("P_AUTHORIZERIDESITEF", OracleDbType.Varchar2);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = DBNull.Value;

                pm = cm.Parameters.Add("P_CDFORMAPAGAMENTO", OracleDbType.Double);
                pm.Direction = System.Data.ParameterDirection.Input;
                pm.Value = Convert.ToDouble(cdformapagamento);
                

                cm.ExecuteNonQuery();
                List<pedidopagamento> lstPegpag = new List<pedidopagamento>();

                lstPegpag = BuscaPagamentoTitulo(codped,datasource, schema);

                return lstPegpag;

            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                con.Close();
            }

        }

        public static List<pedidopagamento> BuscaPagamentoTitulo(string codped, string datasource, string schema)
        {
            OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource));

            try
            {


                con.Open();
                String sql = "";
                sql = "select cdformapag,formapag,codped,dataemissao,to_char(vrpago, '999G999G999G999D99') as vrpago,vrtroco,statusped,pago,alteravel,parcelas,datapag,cdpag,cdstatus,nrtitulo from vw_tele_pedido_pagamento where cdstatus <> 17 and codped = " + codped + " order by cdpag desc";
                OracleCommand cm1 = new OracleCommand(sql, con);
                OracleDataReader dr = cm1.ExecuteReader();
                List<pedidopagamento> lista = new List<pedidopagamento>();
                while (dr.Read())
                {
                    lista.Add(new pedidopagamento(dr["codped"].ToString(),
                        dr["vrpago"].ToString(), dr["formapag"].ToString(),
                        dr["dataemissao"].ToString(), dr["datapag"].ToString(),
                        dr["vrtroco"].ToString(), dr["pago"].ToString(),
                        dr["statusped"].ToString(), dr["alteravel"].ToString(),
                        dr["parcelas"].ToString(), dr["cdformapag"].ToString(),
                        dr["cdpag"].ToString(), dr["nrtitulo"].ToString()
                        ));
                }
                con.Close();
                return lista;

                // return cm.ExecuteScalar().ToString();
                // DataTable dt = new DataTable();
                // dt.Load(cm.ExecuteReader());

            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                con.Close();
            }

        }

        public static List<pedidopagamento> BuscaPagamentoTituloByCodpag(string codpag, string datasource, string schema)
        {
            OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource));

            try
            {


                con.Open();
                String sql = "";
                sql = "select cdformapag,formapag,codped,dataemissao,to_char(vrpago, '999G999G999G999D99') as vrpago,vrtroco,statusped,pago,alteravel,parcelas,datapag,cdpag,cdstatus,nrtitulo from vw_tele_pedido_pagamento where cdstatus <> 17 and cdpag = " + codpag + " order by cdpag desc";
                OracleCommand cm1 = new OracleCommand(sql, con);
                OracleDataReader dr = cm1.ExecuteReader();
                List<pedidopagamento> lista = new List<pedidopagamento>();
                while (dr.Read())
                {
                    lista.Add(new pedidopagamento(dr["codped"].ToString(),
                        dr["vrpago"].ToString(), dr["formapag"].ToString(),
                        dr["dataemissao"].ToString(), dr["datapag"].ToString(),
                        dr["vrtroco"].ToString(), dr["pago"].ToString(),
                        dr["statusped"].ToString(), dr["alteravel"].ToString(),
                        dr["parcelas"].ToString(), dr["cdformapag"].ToString(),
                        dr["cdpag"].ToString(), dr["nrtitulo"].ToString()
                        ));
                }
                con.Close();
                return lista;

                // return cm.ExecuteScalar().ToString();
                // DataTable dt = new DataTable();
                // dt.Load(cm.ExecuteReader());

            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                con.Close();
            }

        }

        public static List<pedidopagamento> BuscaPagamentoTituloByCodpagRest(string codpag, string datasource, string schema)
        {
            OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource));

            try
            {


                con.Open();
                String sql = "";
                sql = "select cdformapag,formapag,codped,dataemissao,to_char(vrpago, '999G999G999G999D99') as vrpago,vrtroco,statusped,pago,alteravel,parcelas,datapag,cdpag,cdstatus,nrtitulo from vw_tele_pedido_pagamento where cdstatus <> 17 and cdpag = " + codpag + " order by cdpag desc";
                OracleCommand cm1 = new OracleCommand(sql, con);
                OracleDataReader dr = cm1.ExecuteReader();
                List<pedidopagamento> lista = new List<pedidopagamento>();
                while (dr.Read())
                {
                    lista.Add(new pedidopagamento(dr["codped"].ToString(),
                        dr["vrpago"].ToString(), dr["formapag"].ToString(),
                        dr["dataemissao"].ToString(), dr["datapag"].ToString(),
                        dr["vrtroco"].ToString(), dr["pago"].ToString(),
                        dr["statusped"].ToString(), dr["alteravel"].ToString(),
                        dr["parcelas"].ToString(), dr["cdformapag"].ToString(),
                        dr["cdpag"].ToString(), dr["nrtitulo"].ToString()
                        ));
                }
                con.Close();
                return lista;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }

        }


    }
}