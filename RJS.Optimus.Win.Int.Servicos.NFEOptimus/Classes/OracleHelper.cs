using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using Oracle.DataAccess.Client;
using Oracle.DataAccess;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class OracleHelper
    {
        private static string STRCONN { get; set; }

        public OracleHelper() { }

        public OracleHelper(string stconn)
        {
            STRCONN = stconn;
        }

        private static String StrConn(BancoOracle bd)
        {
            string Conn = "";
            try
            {
                if (bd == BancoOracle.optimus)
                {
                    Conn = string.IsNullOrEmpty(STRCONN) ? ConfigurationManager.AppSettings.Get("CONNECTION_ORACLE") : STRCONN;
                }
                else
                {
                    Conn = ConfigurationManager.AppSettings.Get("CONNECTION_ORACLE_DOCe");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Conn;
        }

        private static OracleConnection GetCon(BancoOracle bd)
        {
            OracleConnection conexao = new OracleConnection(StrConn(bd));
            try
            {
                conexao.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return conexao;
        }

        private static OracleConnection GetCon(string datasouce, string schema)
        {
            OracleConnection conexao = new OracleConnection(MontaConnection(datasouce, schema));
            try
            {
                conexao.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return conexao;
        }

        private static string MontaConnection(string datasouce, string schema)
        {
            return string.Format("Data Source={0}; User Id={1}; Password={2}4321;", datasouce, schema, schema);
        }

        public enum BancoOracle
        {
            optimus,
            log
        }
        //public static DataSet ExecProcedure(String sql, List<OracleParameter> lparam, string lNomeCursor, BancoOracle bd)
        //{
        //    DataSet dtResultado = new DataSet();
        //    OracleConnection con = null;
        //    OracleCommand cm = null;
        //    OracleDataAdapter odp = null;
        //    try
        //    {
        //        con = GetCon(bd);
        //        cm = con.CreateCommand();
        //        cm.CommandText = sql;
        //        cm.CommandType = CommandType.StoredProcedure;
        //        odp = new OracleDataAdapter(cm);

        //        //Caso retorno tenha cursor
        //        if (!string.IsNullOrEmpty(lNomeCursor))
        //            cm.Parameters.Add(lNomeCursor, OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        //        //Adiciona os parametros
        //        if (lparam != null)
        //        {
        //            foreach (OracleParameter par in lparam)
        //                cm.Parameters.Add(par);
        //        }

        //        odp.Fill(dtResultado, "SqlTable");
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        cm.Dispose();
        //        odp.Dispose();
        //        con.Close();
        //    }
        //    return dtResultado;
        //}
        public static DataSet ExecProcedure(String sql, List<OracleParameter> lparam, string lNomeCursor, BancoOracle bd, string datasource, string schema)
        {
            DataSet dtResultado = new DataSet();
            OracleConnection con = null;
            OracleCommand cm = null;
            OracleDataAdapter odp = null;
            try
            {
                con = GetCon(datasource, schema);
                cm = con.CreateCommand();
                cm.CommandText = sql;
                cm.CommandType = CommandType.StoredProcedure;
                odp = new OracleDataAdapter(cm);

                //Caso retorno tenha cursor
                if (!string.IsNullOrEmpty(lNomeCursor))
                    cm.Parameters.Add(lNomeCursor, OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                //Adiciona os parametros
                if (lparam != null)
                {
                    foreach (OracleParameter par in lparam)
                        cm.Parameters.Add(par);
                }

                odp.Fill(dtResultado, "SqlTable");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cm.Dispose();
                odp.Dispose();
                con.Close();
            }
            return dtResultado;
        }

        public static DataSet ExecSql(String sql, BancoOracle bd, string datasource, string schema)
        {
            DataSet dtResultado = new DataSet();
            OracleConnection con = null;
            OracleCommand cm = null;
            OracleDataAdapter odp = null;
            try
            {
                con = GetCon(datasource, schema);
                cm = con.CreateCommand();
                cm.CommandText = sql;
                cm.CommandType = CommandType.Text;
                odp = new OracleDataAdapter(cm);

                //Caso retorno tenha cursor
                //if (!string.IsNullOrEmpty(lNomeCursor))
                //    cm.Parameters.Add(lNomeCursor, OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                ////Adiciona os parametros
                //if (lparam != null)
                //{
                //    foreach (OracleParameter par in lparam)
                //        cm.Parameters.Add(par);
                //}

                odp.Fill(dtResultado, "SqlTable");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cm.Dispose();
                odp.Dispose();
                con.Close();
            }
            return dtResultado;
        }
        public static int ExecProcedureNonQuery(String sql, List<OracleParameter> lparam, CommandType cmdType, string datasource, string schema)
        {
            OracleConnection con = null;
            OracleCommand cm = null;
            int retorno = 0;
            try
            {
                con = GetCon(datasource, schema);
                cm = con.CreateCommand();
                cm.CommandText = sql;
                cm.CommandType = cmdType;

                //Adiciona os parametros
                if (lparam != null)
                {
                    foreach (OracleParameter par in lparam)
                        cm.Parameters.Add(par);
                }

                retorno = cm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cm.Dispose();
                con.Close();
            }
            return retorno;
        }

        //public static int ExecProcedureNonQuery(String sql, List<OracleParameter> lparam, CommandType cmdType, BancoOracle bd, string datasource, string schema)
        //{
        //    OracleConnection con = null;
        //    OracleCommand cm = null;
        //    int retorno = 0;
        //    try
        //    {
        //        con = GetCon(datasource, schema);
        //        cm = con.CreateCommand();
        //        cm.CommandText = sql;
        //        cm.CommandType = cmdType;

        //        //Adiciona os parametros
        //        if (lparam != null)
        //        {
        //            foreach (OracleParameter par in lparam)
        //                cm.Parameters.Add(par);
        //        }

        //        retorno = cm.ExecuteNonQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        cm.Dispose();
        //        con.Close();
        //    }
        //    return retorno;
        //}


        //public static int ExecProcedureNonQuery(String sql, List<OracleParameter> lparam, CommandType cmdType, BancoOracle bd, string parReturn)
        //{
        //    OracleConnection con = null;
        //    OracleCommand cm = null;
        //    int retorno = 0;
        //    try
        //    {
        //        con = GetCon(bd);
        //        cm = con.CreateCommand();
        //        cm.CommandText = sql;
        //        cm.CommandType = cmdType;

        //        //Adiciona os parametros
        //        if (lparam != null)
        //        {
        //            foreach (OracleParameter par in lparam)
        //                cm.Parameters.Add(par);
        //        }

        //        cm.ExecuteNonQuery();

        //        retorno = Convert.ToInt32(cm.Parameters[parReturn].Value.ToString());
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        cm.Dispose();
        //        con.Close();
        //    }
        //    return retorno;
        //}

        public static int ExecProcedureNonQuery(String sql, List<OracleParameter> lparam, CommandType cmdType, BancoOracle bd, string parReturn, string datasource, string schema)
        {
            OracleConnection con = null;
            OracleCommand cm = null;
            int retorno = 0;
            try
            {
                con = GetCon(datasource, schema);
                cm = con.CreateCommand();
                cm.CommandText = sql;
                cm.CommandType = cmdType;

                //Adiciona os parametros
                if (lparam != null)
                {
                    foreach (OracleParameter par in lparam)
                        cm.Parameters.Add(par);
                }

                cm.ExecuteNonQuery();

                retorno = Convert.ToInt32(cm.Parameters[parReturn].Value.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cm.Dispose();
                con.Close();
            }
            return retorno;
        }

        //public static int ExecProcedureNonQuery(String sql, List<OracleParameter> lparam, BancoOracle bd)
        //{
        //    OracleConnection con = null;
        //    OracleCommand cm = null;
        //    int retorno = 0;
        //    try
        //    {
        //        con = GetCon(bd);
        //        cm = con.CreateCommand();
        //        cm.CommandText = sql;

        //        //Adiciona os parametros
        //        if (lparam != null)
        //        {
        //            foreach (OracleParameter par in lparam)
        //                cm.Parameters.Add(par);
        //        }

        //        retorno = cm.ExecuteNonQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        cm.Dispose();
        //        con.Close();
        //    }
        //    return retorno;
        //}

        //public static int ExecProcedureScalar(String sql, List<OracleParameter> lparam, CommandType cmdType, BancoOracle bd)
        //{
        //    OracleConnection con = null;
        //    OracleCommand cm = null;
        //    int retorno = 0;
        //    try
        //    {
        //        con = GetCon(bd);
        //        cm = con.CreateCommand();
        //        cm.CommandText = sql;
        //        cm.CommandType = cmdType;

        //        //Adiciona os parametros
        //        if (lparam != null)
        //        {
        //            foreach (OracleParameter par in lparam)
        //                cm.Parameters.Add(par);
        //        }

        //        retorno = Convert.ToInt32(cm.ExecuteScalar());
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        cm.Dispose();
        //        con.Close();
        //    }
        //    return retorno;
        //}

        //public static int ExecProcedureScalarTrans(String sql, List<OracleParameter> lparam, CommandType cmdType, BancoOracle bd)
        //{
        //    OracleConnection con = null;
        //    OracleCommand cm = null;
        //    OracleTransaction dbTransaction = null;
        //    int retorno = 0;
        //    try
        //    {
        //        con = GetCon(bd);
        //        dbTransaction = con.BeginTransaction(IsolationLevel.ReadCommitted);
        //        cm = con.CreateCommand();
        //        cm.Transaction = dbTransaction;
        //        cm.CommandText = sql;
        //        cm.CommandType = cmdType;

        //        //Adiciona os parametros
        //        if (lparam != null)
        //        {
        //            foreach (OracleParameter par in lparam)
        //                cm.Parameters.Add(par);
        //        }

        //        retorno = Convert.ToInt32(cm.ExecuteScalar());
        //        dbTransaction.Commit();
        //    }
        //    catch (Exception e)
        //    {
        //        dbTransaction.Rollback();
        //        throw e;
        //    }
        //    finally
        //    {
        //        cm.Dispose();
        //        con.Close();
        //    }
        //    return retorno;
        //}

        public static int ExecProcedureScalarTrans(String sql, List<OracleParameter> lparam, CommandType cmdType, BancoOracle bd, string datasource, string schema)
        {
            OracleConnection con = null;
            OracleCommand cm = null;
            OracleTransaction dbTransaction = null;
            int retorno = 0;
            try
            {
                con = GetCon(datasource, schema);
                dbTransaction = con.BeginTransaction(IsolationLevel.ReadCommitted);
                cm = con.CreateCommand();
                cm.Transaction = dbTransaction;
                cm.CommandText = sql;
                cm.CommandType = cmdType;

                //Adiciona os parametros
                if (lparam != null)
                {
                    foreach (OracleParameter par in lparam)
                        cm.Parameters.Add(par);
                }

                retorno = Convert.ToInt32(cm.ExecuteScalar());
                dbTransaction.Commit();
            }
            catch (Exception e)
            {
                dbTransaction.Rollback();
                throw e;
            }
            finally
            {
                cm.Dispose();
                con.Close();
            }
            return retorno;
        }

    }
}
