using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Util;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Database
{
    public class UsuarioADO
    {
        public static int InsertUser(Usuario user, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();

            lstParam.Add(new OracleParameter("P_EMAIL", OracleDbType.Varchar2, user.email, System.Data.ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_IDCEL", OracleDbType.Varchar2, user.idcel, System.Data.ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_CDENTIFILIAL", OracleDbType.Double, user.cdentifilial, System.Data.ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_SO", OracleDbType.Varchar2, string.IsNullOrEmpty(user.so) ? "9" : user.so, System.Data.ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_SENHA", OracleDbType.Varchar2, user.senha, System.Data.ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_CDAPP", OracleDbType.Varchar2, user.cdapp, System.Data.ParameterDirection.Input));

            return OracleHelper.ExecProcedureNonQuery("PK_APP.SP_USUARIO", lstParam, System.Data.CommandType.StoredProcedure, datasource, schema);
        }
        public static Usuario Login(Usuario puser, string datasource, string schema)
        {
            /*
             PROCEDURE SP_USUARIO_LOGAR
    (  P_EMAIL IN VARCHAR2,
      P_SENHA IN VARCHAR2,
      P_CDENTIFILIAL IN NUMBER,
      P_CDAPP IN NUMBER,
      P_CURSOR_USUARIO OUT SYS_REFCURSOR
);
             */
            Usuario user = new Usuario();

            List<string> Cursores = new List<string>();
            Cursores.Add("P_CURSOR_USUARIO");

            List<OracleParameter> lstParam = new List<OracleParameter>();
            DataSet dtsRetorno = null;

            //lstParam.Add(new OracleParameter("P_EMAIL", puser.email));
            //lstParam.Add(new OracleParameter("P_SENHA", puser.senha));
            //lstParam.Add(new OracleParameter("P_CDENTIFILIAL", puser.cdentifilial));
            //lstParam.Add(new OracleParameter("P_CDAPP",Convert.ToDouble(puser.cdapp)));

            lstParam.Add(new OracleParameter("P_EMAIL", OracleDbType.Varchar2, puser.email, System.Data.ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_SENHA", OracleDbType.Varchar2, puser.senha, System.Data.ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_CDENTIFILIAL", OracleDbType.Double, Convert.ToDouble(puser.cdentifilial), System.Data.ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_CDAPP", OracleDbType.Double, Convert.ToDouble(puser.cdapp), System.Data.ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_IDCEL", OracleDbType.Varchar2, puser.idcel, System.Data.ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_SO", OracleDbType.Double, Convert.ToDouble(puser.so), System.Data.ParameterDirection.Input));

            dtsRetorno = OracleHelper.ExecProcedure("PK_APP.SP_USUARIO_LOGAR", lstParam, Cursores, Util.OracleHelper.StrConn(schema, datasource));

            DataTable dt = dtsRetorno.Tables[0];

            if (dt.Rows.Count > 0)
            {
                user.idcel = dt.Rows[0]["TXIDCELULAR"].ToString();
                user.cdentidade = dt.Rows[0]["CDENTIDADE"].ToString();
                user.so = dt.Rows[0]["CDSO"].ToString();
                user.email = dt.Rows[0]["TXEMAIL"].ToString();
                user.cdentifilial = dt.Rows[0]["CDENTIFILIAL"].ToString();
                user.status = "100";
            }
            else
            {
                user.status = "99";
            }

            if (!string.IsNullOrEmpty(user.cdentidade))
            {
                user.cliente = RetornaClienteById(user.cdentidade.ToString(), datasource, schema);
            }

            return user;

        }

        public static Usuario ChecaEmail(string email, string cdapp, string cdentifilial, string datasource, string schema)
        {
            Usuario user = new Usuario();
            StringBuilder q = new StringBuilder();
            q.AppendLine("select ");
            q.AppendLine(" CDENTIDADE ,");
            q.AppendLine(" TXEMAIL ,");
            q.AppendLine(" TXIDCELULAR ,");
            q.AppendLine(" CDSO ,");
            q.AppendLine(" TXSENHA ,");
            q.AppendLine(" CDENTIFILIAL,");
            q.AppendLine(" CDAPP ");
            q.AppendLine(string.Format(" from USUARIOAPP where upper(TXEMAIL)=upper('{0}') and cdapp ={1} and cdentifilial ={2} ", email, cdapp, cdentifilial));

            DataSet dts = OracleHelper.ExecQuery(q.ToString(), null, "", OracleHelper.BancoOracle.optimus, schema, datasource);

            if (dts.Tables.Count > 0)
            {
                user.idcel = dts.Tables[0].Rows[0]["TXIDCELULAR"].ToString();
                user.cdentidade = dts.Tables[0].Rows[0]["CDENTIDADE"].ToString();
                //user.nome = dts.Tables[0].Rows[0]["TXIDCELULAR"].ToString();
                //user.senha= dts.Tables[0].Rows[0]["TXIDCELULAR"].ToString();
                user.so = dts.Tables[0].Rows[0]["CDSO"].ToString();
                //user.sobrenome = dts.Tables[0].Rows[0]["TXIDCELULAR"].ToString();
                user.email = dts.Tables[0].Rows[0]["TXEMAIL"].ToString();
                user.cdentifilial = dts.Tables[0].Rows[0]["CDENTIFILIAL"].ToString();
                //user.so = dts.Tables[0].Rows[0]["CDSO"].ToString();
            }

            //if (string.IsNullOrEmpty(user.cdentidade))
            //{
            //    user.cliente = RetornaClienteById(user.cdentidade.ToString(), datasource, schema);
            //}

            return user;

        }

        public static cliente RetornaClienteById(string cdentidade, string datasource, string schema)
        {
            List<cliente> lstResult = new List<cliente>();

            string queryCep = string.Format("select * from vw_entidadepdv where cdentidade = {0}", cdentidade);

            StringBuilder querytelefone = new StringBuilder();

            querytelefone.AppendLine(" select cdtipotelefone,nrddd,nrtelefone");
            querytelefone.AppendLine("             from conttelefone ct, contato c");
            querytelefone.AppendLine("                where c.cdcontato = ct.cdcontato");
            querytelefone.AppendLine("                and c.cdentidade = " + cdentidade);
            querytelefone.AppendLine("                and ct.stexcluido = 0");
            querytelefone.AppendLine("                and c.stexcluido = 0 ");

            using (OracleConnection connection = new OracleConnection(OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryCep, connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    cliente cli = null;
                    while (reader.Read())
                    {
                        cli = new cliente();
                        cli.cdentidade = Convert.ToInt32(reader["cdentidade"].ToString());
                        cli.tipopessoa = reader["tipopessoa"].ToString();
                        cli.nome = reader["razao_nome"].ToString();
                        // cli.Email = reader["txemail"].ToString();
                        //cli.Endereco = reader["txlogradouro"].ToString();
                        //cli.Complemento = reader["txcomplemento"].ToString();
                        //cli.Numero = reader["txnumero"].ToString();
                        //cli.Bairro = reader["txbairro"].ToString();
                        //cli.Cidade = reader["txcidade"].ToString();
                        //cli.Uf = reader["cduf"].ToString();
                        //cli.Cep = reader["cdcep"].ToString();
                        //cli.CPFCNPJ = reader["cnpj_cpf"].ToString();
                        //cli.cdendereco = Convert.ToInt32(reader["CDENDERECO"].ToString());
                        //cli.Enderecop1 = string.Format("{0}, {1}", cli.Endereco, cli.Numero);
                        //cli.Enderecop2 = string.Format("{0} - CEP: {1}", cli.Bairro, cli.Cep);
                        //cli.Enderecop3 = string.Format("{0} - {1}", cli.Cidade, cli.Uf);


                        //using (OracleCommand command2 = new OracleCommand(querytelefone.ToString(), connection))
                        //{
                        //    using (OracleDataReader reader2 = command2.ExecuteReader())
                        //    {
                        //        while (reader2.Read())
                        //        {
                        //            if (reader2["cdtipotelefone"].ToString() == "2")
                        //            {
                        //                cli.ddd = reader2["nrddd"].ToString();
                        //                cli.telefone = reader2["nrtelefone"].ToString();
                        //            }
                        //            else if (reader2["cdtipotelefone"].ToString() == "3")
                        //            {
                        //                cli.dddcel = reader2["nrddd"].ToString();
                        //                cli.telefonecel = reader2["nrtelefone"].ToString();
                        //            }
                        //        }
                        //    }
                        //}

                        //if (!string.IsNullOrEmpty(cli.ddd) && !string.IsNullOrEmpty(cli.telefone))
                        //{
                        //    cli.Enderecop4 = string.Format("({0}){1}", cli.ddd, cli.telefone);
                        //}
                        //if (!string.IsNullOrEmpty(cli.dddcel) && !string.IsNullOrEmpty(cli.telefonecel))
                        //{
                        //    cli.Enderecop5 = string.Format("({0}){1}", cli.dddcel, cli.telefonecel);
                        //}

                        lstResult.Add(cli);
                    }
                }
            }
            return lstResult.OrderByDescending(a => a.cdentidade).FirstOrDefault();
        }

        public static Usuario RetornaUsuarioByEmail(string email, string cdapp, string cdentifilial, string datasource, string schema)
        {
            Usuario user = new Usuario();
            string q = string.Format("select * from usuarioapp where upper(TXEMAIL)=upper('{0}') and cdapp ={1} and cdentifilial ={2} ", email, cdapp, cdentifilial);

            DataSet dts = OracleHelper.ExecQuery(q.ToString(), null, "", OracleHelper.BancoOracle.optimus, schema, datasource);

            if (dts.Tables[0].Rows.Count > 0)
            {
                user.idcel = dts.Tables[0].Rows[0]["TXIDCELULAR"].ToString();
                user.cdentidade = dts.Tables[0].Rows[0]["CDENTIDADE"].ToString();
                //user.nome = dts.Tables[0].Rows[0]["TXIDCELULAR"].ToString();
                user.senha = dts.Tables[0].Rows[0]["TXSENHA"].ToString();
                user.so = dts.Tables[0].Rows[0]["CDSO"].ToString();
                //user.sobrenome = dts.Tables[0].Rows[0]["TXIDCELULAR"].ToString();
                user.email = dts.Tables[0].Rows[0]["TXEMAIL"].ToString();
                user.cdentifilial = dts.Tables[0].Rows[0]["CDENTIFILIAL"].ToString();
                //user.so = dts.Tables[0].Rows[0]["CDSO"].ToString();
            }

            //if (string.IsNullOrEmpty(user.cdentidade))
            //{
            //    user.cliente = RetornaClienteById(user.cdentidade.ToString(), datasource, schema);
            //}

            return user;

        }

        public static void CadastrarColaborador(string cdfuncionario, string nomeuser, string usuario, string senha, string cdforn, string datasource, string schema)
        {
            if (string.IsNullOrEmpty(cdfuncionario) || cdfuncionario == "0")
            {
                string queryuser = string.Format("INSERT INTO USUARIO(CDUSUARIO,CDDOMINIO,STUSUARIO,STEXCLUIDO)VALUES('{0}',1,1,0)", nomeuser);
                string queryfunc = string.Format("INSERT INTO FUNCIONARIO(nmfuncionario,dsemail,cdusuariooptimus,cdentidadepai,dssenha,dtadmissao) (SELECT '{0}','{1}',cdusuariooptimus,{2},'{3}',sysdate FROM USUARIO WHERE CDUSUARIO = '{4}')", nomeuser, usuario, cdforn, senha, nomeuser);
                OracleHelper.ExecProcedureNonQuery(queryuser, null, System.Data.CommandType.Text, datasource, schema);
                OracleHelper.ExecProcedureNonQuery(queryfunc, null, System.Data.CommandType.Text, datasource, schema);

            }
            else
            {
                string queryfuncupdate = string.Format("update FUNCIONARIO set dsemail = '{0}',dssenha ='{1}' where cdfuncionario = {2}", usuario, senha, cdfuncionario);
                OracleHelper.ExecProcedureNonQuery(queryfuncupdate, null, System.Data.CommandType.Text, datasource, schema);

            }

        }

        //public static void CadastrarColaborador(string nomeuser, string usuario, string senha, string cdforn, string datasource, string schema)
        //{
        //    string queryuser = string.Format("INSERT INTO USUARIO(CDUSUARIO,CDDOMINIO,STUSUARIO,STEXCLUIDO)VALUES('{0}',1,1,0)", usuario);
        //    string queryfunc = string.Format("INSERT INTO FUNCIONARIO(nmfuncionario,dsemail,cdusuariooptimus,cdentidadepai,dssenha) (SELECT '{0}', '{1}', cdusuariooptimus,{2},'{3}' FROM USUARIO WHERE CDUSUARIO = '{4}')", nomeuser, usuario, cdforn, senha, usuario);
        //    OracleHelper.ExecProcedureNonQuery(queryuser, null, System.Data.CommandType.Text, datasource, schema);
        //    OracleHelper.ExecProcedureNonQuery(queryfunc, null, System.Data.CommandType.Text, datasource, schema);
        //}

        public static UsuarioRetorno Login(string usuario, string senha, string datasource, string schema)
        {
            UsuarioRetorno Result = new UsuarioRetorno();

            Result.Logado = false;

            string queryend = "select cdfuncionario,cdentidadepai,nmfuncionario,cdusuariooptimus,dtadmissao from funcionario where stexcluido = 0 and upper(dsemail) = :dsemail and dssenha = :dssenha";

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryend, connection);
                command.Parameters.Add("dsemail", usuario.ToUpper());
                command.Parameters.Add("dssenha", senha);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Result.cdfuncionario = Convert.ToInt32(reader["cdfuncionario"].ToString());
                        Result.cdentidadepai = reader["cdentidadepai"].ToString();
                        Result.nmfuncionario = reader["nmfuncionario"].ToString();
                        Result.cdusuariooptimus = reader["cdusuariooptimus"].ToString();
                        Result.dtcadastro = reader["dtadmissao"].ToString();
                    }
                }
            }

            if (Result.cdfuncionario > 0)
            {
                Result.Logado = true;

                List<string> perfis = new List<string>();

                using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
                {
                    string sql = "select P.CDPERFIL,P.NMPERFIL from PERFIL P,usuaperfil PU  where P.CDPERFIL=PU.CDPERFIL AND PU.CDUSUARIOOPTIMUS=" + Result.cdusuariooptimus;
                    OracleCommand command = new OracleCommand(sql, connection);
                    connection.Open();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            perfis.Add(reader["NMPERFIL"].ToString());
                        }
                    }
                }

                string admin = (from a in perfis
                                where a == "PARCEIROADM"
                                select a).FirstOrDefault();


                if (string.IsNullOrEmpty(admin))
                {
                    string parcuser = (from a in perfis
                                       where a == "PARCEIRO"
                                       select a).FirstOrDefault();

                    Result.perfil = "PARCEIRO";

                }
                else
                {
                    Result.perfil = admin;
                }

            }
            else
            {


                StringBuilder query = new StringBuilder();
                query.AppendLine(" select f.cdentifornecedor,e.nmentidade from fornecedor f, entidade e ");
                query.AppendLine(" where f.cdentifornecedor = e.cdentidade and e.stexcluido = 0 and ");
                query.AppendLine(" f.stparceiroativo = 1 and f.stparceiro = 1 and upper(f.parceirousuario) = :dsemail and  f.parceirosenha = :dssenha ");


                using (OracleConnection connection2 = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
                {
                    OracleCommand command = new OracleCommand(query.ToString(), connection2);
                    command.Parameters.Add("dsemail", usuario.ToUpper());
                    command.Parameters.Add("dssenha", senha);
                    connection2.Open();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Result.cdfuncionario = Convert.ToInt32(reader["cdentifornecedor"].ToString());
                            Result.cdentidadepai = reader["cdentifornecedor"].ToString();
                            Result.nmfuncionario = reader["nmentidade"].ToString();
                            Result.cdusuariooptimus = "";
                            Result.dtcadastro = "";// reader["dtadmissao"].ToString();
                        }
                    }
                }


            }


            return Result;

        }

        public static List<UsuarioRetorno> RetornaColaborador(string nome, string email, string cdforn, string datasource, string schema)
        {
            List<UsuarioRetorno> Result = new List<UsuarioRetorno>();

            string queryend = "select cdfuncionario,cdentidadepai,nmfuncionario,cdusuariooptimus,dtadmissao,dsemail from funcionario where  stexcluido=0 ";
            if (!string.IsNullOrEmpty(nome))
            {
                queryend += string.Format(" and upper(nmfuncionario) like upper('%{0}%')", nome);
            }
            if (!string.IsNullOrEmpty(email))
            {
                queryend += string.Format(" and dsemail like '%{0}%'", email);
            }
            if (!string.IsNullOrEmpty(cdforn))
            {
                queryend += string.Format(" and cdentidadepai ={0}", cdforn);
            }

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryend, connection);

                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UsuarioRetorno user = new UsuarioRetorno();
                        user.dtcadastro = reader["dtadmissao"].ToString();
                        user.cdfuncionario = Convert.ToInt32(reader["cdfuncionario"].ToString());
                        user.cdentidadepai = reader["cdentidadepai"].ToString();
                        user.nmfuncionario = reader["nmfuncionario"].ToString();
                        user.cdusuariooptimus = reader["cdusuariooptimus"].ToString();
                        user.dsemail = reader["dsemail"].ToString();

                        Result.Add(user);
                    }
                }
            }
            return Result;
        }

        public static List<UsuarioRetorno> RetornaColaboradoresByIdPai(string cdcolaboradorpai, string datasource, string schema)
        {
            List<UsuarioRetorno> Result = new List<UsuarioRetorno>();

            string queryend = "select cdfuncionario,cdentidadepai,nmfuncionario,cdusuariooptimus,dtadmissao,dsemail from funcionario where  stexcluido=0 ";

            if (!string.IsNullOrEmpty(cdcolaboradorpai))
            {
                queryend += string.Format(" and cdentidadepai ={0} or cdfuncionario = {1}", cdcolaboradorpai, cdcolaboradorpai);
            }

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryend, connection);
                UsuarioRetorno user = new UsuarioRetorno();
                user.nmfuncionario = "TODOS";
                user.cdusuariooptimus = "0";
                user.dsemail = "TODOS";

                Result.Add(user);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new UsuarioRetorno();
                        user.dtcadastro = reader["dtadmissao"].ToString();
                        user.cdfuncionario = Convert.ToInt32(reader["cdfuncionario"].ToString());
                        user.cdentidadepai = reader["cdentidadepai"].ToString();
                        user.nmfuncionario = reader["nmfuncionario"].ToString().ToUpper();
                        user.cdusuariooptimus = reader["cdusuariooptimus"].ToString();
                        user.dsemail = reader["dsemail"].ToString().ToUpper();

                        Result.Add(user);
                    }
                }
            }
            return Result;
        }

        public static bool TemColaboradorByNome(string nome, string datasource, string schema)
        {
            string queryend = string.Format("select CDUSUARIO from USUARIO where CDUSUARIO = '{0}'", nome);

            string user = "";

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryend, connection);

                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = reader["CDUSUARIO"].ToString();
                    }
                }
            }
            return !string.IsNullOrEmpty(user);
        }

        public static bool ExcluirFuncionario(string cdfuncionario, string datasource, string schema)
        {
            string queryfuncupdate = string.Format("update FUNCIONARIO set stexcluido = 1 where cdfuncionario = {0}", cdfuncionario);

            string queryuserupdate = string.Format("update usuario set stexcluido= 1 where cdusuariooptimus in (select cdusuariooptimus from funcionario where cdfuncionario = {0})", cdfuncionario);

            OracleHelper.ExecProcedureNonQuery(queryfuncupdate, null, System.Data.CommandType.Text, datasource, schema);

            OracleHelper.ExecProcedureNonQuery(queryuserupdate, null, System.Data.CommandType.Text, datasource, schema);

            return true;
        }


        public static UsuarioRetorno RetornaColaboradorByid(string cdcolaborador, string datasource, string schema)
        {
            UsuarioRetorno Result = new UsuarioRetorno();

            string queryend = "select cdfuncionario,cdentidadepai,nmfuncionario,cdusuariooptimus,dtadmissao,dsemail,dssenha from funcionario where stexcluido = 0 and cdfuncionario = " + cdcolaborador;


            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryend, connection);

                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UsuarioRetorno user = new UsuarioRetorno();
                        user.dtcadastro = reader["dtadmissao"].ToString();
                        user.cdfuncionario = Convert.ToInt32(reader["cdfuncionario"].ToString());
                        user.cdentidadepai = reader["cdentidadepai"].ToString();
                        user.nmfuncionario = reader["nmfuncionario"].ToString();
                        user.cdusuariooptimus = reader["cdusuariooptimus"].ToString();
                        user.dsemail = reader["dsemail"].ToString();
                        user.senha = reader["dssenha"].ToString();
                        Result = user;
                    }
                }
            }
            return Result;
        }

        public static List<Parceiro> RetornaParceiros(string datasource, string schema)
        {
            //select f.cdentifornecedor, nmentidade from fornecedor f, entidade e  where stparceiro = 1 and e.cdentidade = f.cdentifornecedor
            List<Parceiro> Result = new List<Parceiro>();

            string queryend = "select f.cdentifornecedor, nmentidade from fornecedor f, entidade e  where stparceiro = 1 and e.cdentidade = f.cdentifornecedor";


            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryend, connection);
                Parceiro user = new Parceiro();
                //user.cdparceiro = Convert.ToInt32(0);
                //user.nmparceiro = "TODOS";
                //Result.Add(user);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new Parceiro();
                        user.cdparceiro = Convert.ToInt32(reader["cdentifornecedor"].ToString());
                        user.nmparceiro = reader["nmentidade"].ToString().ToUpper();
                        Result.Add(user);
                    }
                }
            }
            return Result;
        }

    }
}