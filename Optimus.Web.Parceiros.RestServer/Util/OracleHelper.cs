using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Util
{
    public class OracleHelper
    {
        private static String StrConn(BancoOracle bd)
        {
            string Conn = "";
            try
            {

                Conn = bd == BancoOracle.optimus ? ConfigurationManager.AppSettings.Get("Ora") : ConfigurationManager.AppSettings.Get("CONNECTION_ORACLE_LOG");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Conn;
        }
        public static String StrConn(string NomeBd, string DataSource)
        {
            string Conn = "";
            try
            {

                //Conn = string.Format("Data Source=cloudrjs69; User Id={0}; Password={1}4321;", NomeBd, NomeBd);
                Conn = string.Format("Data Source={0}; User Id={1}; Password={2}4321;", DataSource, NomeBd, NomeBd);
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
        private static OracleConnection GetCon(string NomeBd, string DataSource)
        {
            OracleConnection conexao = new OracleConnection(StrConn(NomeBd, DataSource));
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

        private static OracleConnection GetCon(string strconn)
        {
            OracleConnection conexao = new OracleConnection(strconn);
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

        public enum BancoOracle
        {
            optimus,
            log
        }
        public static DataSet ExecProcedure(String sql, List<OracleParameter> lparam, string lNomeCursor, BancoOracle bd)
        {
            DataSet dtResultado = new DataSet();
            OracleConnection con = null;
            OracleCommand cm = null;
            OracleDataAdapter odp = null;
            try
            {
                con = GetCon(bd);
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

        public static DataSet ExecProcedure(String sql, List<OracleParameter> lparam, List<string> lNomeCursores, BancoOracle bd)
        {
            DataSet dtResultado = new DataSet();
            OracleConnection con = null;
            OracleCommand cm = null;
            OracleDataAdapter odp = null;
            Random rand = new Random();
            try
            {
                con = GetCon(bd);
                cm = con.CreateCommand();
                cm.CommandText = sql;
                cm.CommandType = CommandType.StoredProcedure;
                odp = new OracleDataAdapter(cm);

                //Caso retorno tenha cursor
                foreach (string item in lNomeCursores)
                {
                    cm.Parameters.Add(item, OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    odp.TableMappings.Add("Table" + rand.Next(1, 100), item);
                }

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
        public static DataSet ExecProcedure(String sql, List<OracleParameter> lparam, List<string> lNomeCursores, string strconn)
        {
            DataSet dtResultado = new DataSet();
            OracleConnection con = null;
            OracleCommand cm = null;
            OracleDataAdapter odp = null;
            Random rand = new Random();
            try
            {
                con = new OracleConnection(strconn);
                cm = con.CreateCommand();
                cm.CommandText = sql;
                cm.CommandType = CommandType.StoredProcedure;
                odp = new OracleDataAdapter(cm);

                //Caso retorno tenha cursor
                foreach (string item in lNomeCursores)
                {
                    cm.Parameters.Add(item, OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    odp.TableMappings.Add("Table" + rand.Next(1, 100), item);
                }

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
        public static int ExecProcedureNonQuery(String sql, List<OracleParameter> lparam, CommandType cmdType, BancoOracle bd)
        {
            OracleConnection con = null;
            OracleCommand cm = null;
            int retorno = 0;
            try
            {
                con = GetCon(bd);
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

        public static int ExecProcedureNonQuery(String sql, List<OracleParameter> lparam, CommandType cmdType, BancoOracle bd, string parReturn)
        {
            OracleConnection con = null;
            OracleCommand cm = null;
            int retorno = 0;
            try
            {
                con = GetCon(bd);
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
        public static int ExecProcedureNonQuery(String sql, List<OracleParameter> lparam, CommandType cmdType, string strconn)
        {
            OracleConnection con = null;
            OracleCommand cm = null;
            int retorno = 0;
            try
            {
                con = new OracleConnection(strconn);
                cm = con.CreateCommand();
                cm.CommandText = sql;
                cm.CommandType = cmdType;

                //Adiciona os parametros
                if (lparam != null)
                {
                    foreach (OracleParameter par in lparam)
                        cm.Parameters.Add(par);
                }
                con.Open();
                retorno = cm.ExecuteNonQuery();

                //retorno = Convert.ToInt32(cm.Parameters[parReturn].Value.ToString());
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
        public static int ExecProcedureNonQuery(String sql, List<OracleParameter> lparam, BancoOracle bd)
        {
            OracleConnection con = null;
            OracleCommand cm = null;
            int retorno = 0;
            try
            {
                con = GetCon(bd);
                cm = con.CreateCommand();
                cm.CommandText = sql;

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

        public static int ExecProcedureScalar(String sql, List<OracleParameter> lparam, CommandType cmdType, BancoOracle bd)
        {
            OracleConnection con = null;
            OracleCommand cm = null;
            int retorno = 0;
            try
            {
                con = GetCon(bd);
                cm = con.CreateCommand();
                cm.CommandText = sql;
                cm.CommandType = cmdType;

                //Adiciona os parametros
                if (lparam != null)
                {
                    foreach (OracleParameter par in lparam)
                        cm.Parameters.Add(par);
                }

                retorno = Convert.ToInt32(cm.ExecuteScalar());
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

        public static int ExecProcedureScalar4nfe(String sql, string strconn)
        {
            OracleConnection con = null;
            OracleCommand cm = null;
            int retorno = 0;
            try
            {
                con = new OracleConnection(strconn);
                con.Open();
                cm = con.CreateCommand();
                cm.CommandText = sql;
                cm.CommandType = CommandType.Text;

                retorno = Convert.ToInt32(cm.ExecuteScalar());
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

        public static DataSet ExecQuery(String sql, List<OracleParameter> lparam, string lNomeCursor, BancoOracle bd, string NomeBd, string DataSource)
        {
            DataSet dtResultado = new DataSet();
            OracleConnection con = null;
            OracleCommand cm = null;
            OracleDataAdapter odp = null;
            try
            {
                con = GetCon(NomeBd, DataSource);
                cm = con.CreateCommand();
                cm.CommandText = sql;
                cm.CommandType = CommandType.Text;
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

        public static DataSet ExecQuery(String sql, List<OracleParameter> lparam, BancoOracle bd)
        {
            DataSet dtResultado = new DataSet();
            OracleConnection con = null;
            OracleCommand cm = null;
            OracleDataAdapter odp = null;
            try
            {
                con = GetCon(bd);
                cm = con.CreateCommand();
                cm.CommandText = sql;
                cm.CommandType = CommandType.Text;
                odp = new OracleDataAdapter(cm);

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

        public static DataSet ExecQuery(String sql, List<OracleParameter> lparam, string lNomeCursor, string strconn)
        {
            DataSet dtResultado = new DataSet();
            OracleConnection con = null;
            OracleCommand cm = null;
            OracleDataAdapter odp = null;
            try
            {
                con = new OracleConnection(strconn);
                con.Open();
                cm = con.CreateCommand();
                cm.CommandText = sql;
                cm.CommandType = CommandType.Text;
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
                con = GetCon(schema, datasource);
                cm = con.CreateCommand();
                cm.CommandText = sql;
                cm.CommandType = CommandType.Text;
                odp = new OracleDataAdapter(cm);

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
                con = GetCon(schema, datasource);
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
            finally
            {
                cm.Dispose();
                con.Close();
            }
            return retorno;
        }
    }
}