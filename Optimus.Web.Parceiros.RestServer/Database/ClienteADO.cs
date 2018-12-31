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
    public class ClienteADO
    {

        public static double GravaCliente(cliente pcliente, string datasource, string schema)
        {
            /*
             SP_GRAVAENTIDADE
       ( p_CDENTIDADE IN number,
         p_nome IN VARCHAR2,
         P_CPFCNPJ IN VARCHAR2,
         p_email in VARCHAR2,
         p_telefone in VARCHAR2,
         p_endereco in VARCHAR2,
         p_complemento in VARCHAR2,
         p_numero in VARCHAR2,
         p_bairro in VARCHAR2,
         p_cidade in VARCHAR2,
         p_cep in VARCHAR2,
         p_uf in VARCHAR2,
         P_CODNOVO OUT NUMBER
             */
            double cdentidade = 0;

            try
            {
                using (OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
                {
                    OracleParameter pm = null;
                    con.Open();
                    using (OracleCommand cm = new OracleCommand("pk_entidade.SP_GRAVAENTIDADE", con))
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;

                        cm.Parameters.Clear();


                        pm = cm.Parameters.Add("p_CDENTIDADE", OracleDbType.Double);
                        pm.Direction = System.Data.ParameterDirection.Input;

                        pm.Value = pcliente.cdentidade;

                        //pm = cm.Parameters.Add("p_nome", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.Nome;

                        //pm = cm.Parameters.Add("P_CPFCNPJ", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.CPFCNPJ;

                        //pm = cm.Parameters.Add("p_email", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.Email;

                        //pm = cm.Parameters.Add("p_endereco", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.Endereco;

                        //pm = cm.Parameters.Add("p_complemento", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.Complemento;

                        //pm = cm.Parameters.Add("p_numero", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.Numero;

                        //pm = cm.Parameters.Add("p_bairro", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.Bairro;

                        //pm = cm.Parameters.Add("p_cidade", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.Cidade;

                        //pm = cm.Parameters.Add("p_cep", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.Cep;

                        //pm = cm.Parameters.Add("p_uf", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.Uf;
                        /*
                          p_ddd in VARCHAR2 default null,
                         p_telefone IN VARCHAR2 default null,
                         p_dddcel IN VARCHAR2 default null,
                         p_celular IN VARCHAR2 default null,
                         */

                        //pm = cm.Parameters.Add("p_ddd", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.ddd;

                        //pm = cm.Parameters.Add("p_telefone", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.telefone;

                        //pm = cm.Parameters.Add("p_dddcel", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.dddcel;

                        //pm = cm.Parameters.Add("p_celular", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.telefonecel;

                        //pm = cm.Parameters.Add("P_CDENDERECO", OracleDbType.Double);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.cdendereco;

                        pm = cm.Parameters.Add("P_CDENTIRESP", OracleDbType.Double);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = Convert.ToDouble(pcliente.cdentiresp);

                        //pm = cm.Parameters.Add("P_RG", OracleDbType.Varchar2);
                        //pm.Direction = System.Data.ParameterDirection.Input;
                        //pm.Value = pcliente.rg;

                        pm = cm.Parameters.Add("P_CODNOVO", OracleDbType.Double);
                        pm.Direction = System.Data.ParameterDirection.Output;

                        cm.ExecuteNonQuery();
                        cdentidade = Convert.ToDouble(cm.Parameters["P_CODNOVO"].Value.ToString());

                    }

                }
            }
            catch (Exception ex)
            {
                //Util.Log.For(this, ConfigurationManager.AppSettings.Get("pathlog")).Error(ex.ToString());
                throw ex;
            }
            return cdentidade;
        }

        public static double GravaClienteRest(cliente pcliente, string datasource, string schema)
        {
            double cdentidade = pcliente.cdentidade;

            try
            {
                using (OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
                {
                    OracleParameter pm = null;
                    con.Open();
                    using (OracleCommand cm = new OracleCommand("pk_entidade.SP_GRAVAENTIDADEREST", con))
                    {
                        cm.CommandType = System.Data.CommandType.StoredProcedure;

                        cm.Parameters.Clear();

                        pm = cm.Parameters.Add("p_CDENTIDADE", OracleDbType.Double);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = pcliente.cdentidade;

                        pm = cm.Parameters.Add("p_nome", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = pcliente.nome;

                        pm = cm.Parameters.Add("p_nomefata", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = pcliente.fantasia;

                        pm = cm.Parameters.Add("P_CPFCNPJ", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = pcliente.tipopessoa == "J" ? pcliente.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "") : pcliente.CPF.Replace(".", "").Replace("/", "").Replace("-", "");

                        pm = cm.Parameters.Add("p_ie", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = pcliente.IE;

                        pm = cm.Parameters.Add("p_im", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = pcliente.IM;

                        pm = cm.Parameters.Add("p_sexo", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = string.IsNullOrEmpty(pcliente.sexo) ? "M" : pcliente.sexo;

                        pm = cm.Parameters.Add("p_rg", OracleDbType.Varchar2);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = pcliente.identidade;

                        pm = cm.Parameters.Add("P_CDENTIRESP", OracleDbType.Double);
                        pm.Direction = System.Data.ParameterDirection.Input;
                        pm.Value = string.IsNullOrEmpty(pcliente.cdentiresp) ? 0 : Convert.ToDouble(pcliente.cdentiresp);

                        pm = cm.Parameters.Add("P_CODNOVO", OracleDbType.Double);
                        pm.Direction = System.Data.ParameterDirection.Output;

                        cm.ExecuteNonQuery();
                        cdentidade = Convert.ToDouble(cm.Parameters["P_CODNOVO"].Value.ToString());

                    }

                }

                foreach (clienteendereco ende in pcliente.Enderecos)
                {
                    if (ende.excluir == 1)
                    {
                        string querydelcont = string.Format(" update  entiendereco set stexcluido = 1 where cdendereco = {0}", ende.cdendereco);
                        OracleHelper.ExecProcedureNonQuery(querydelcont, null, System.Data.CommandType.Text, datasource, schema);
                    }
                    else
                    {

                        using (OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
                        {
                            OracleParameter pm = null;
                            con.Open();
                            using (OracleCommand cm = new OracleCommand("pk_entidade.sp_gravaenderest", con))
                            {
                                cm.CommandType = System.Data.CommandType.StoredProcedure;

                                cm.Parameters.Clear();

                                pm = cm.Parameters.Add("p_CDENTIDADE", OracleDbType.Double);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = cdentidade;

                                pm = cm.Parameters.Add("p_cdendereco", OracleDbType.Double);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = string.IsNullOrEmpty(ende.cdendereco) ? 0 : Convert.ToDouble(ende.cdendereco);

                                pm = cm.Parameters.Add("p_endereco", OracleDbType.Varchar2);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = ende.logradouro;

                                pm = cm.Parameters.Add("p_complemento", OracleDbType.Varchar2);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = ende.complemento;

                                pm = cm.Parameters.Add("p_numero", OracleDbType.Varchar2);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = ende.numero;

                                pm = cm.Parameters.Add("p_bairro", OracleDbType.Varchar2);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = ende.bairro;


                                pm = cm.Parameters.Add("p_cidade", OracleDbType.Varchar2);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = ende.cidade;

                                pm = cm.Parameters.Add("p_cep", OracleDbType.Varchar2);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = ende.CEP;

                                pm = cm.Parameters.Add("p_uf", OracleDbType.Varchar2);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = ende.UF;

                                pm = cm.Parameters.Add("p_cdmunicipioibge", OracleDbType.Double);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = string.IsNullOrEmpty(ende.cdmunicipioibge) ? 0 : Convert.ToDouble(ende.cdmunicipioibge);

                                pm = cm.Parameters.Add("p_cdcontato", OracleDbType.Double);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = 0;

                                pm = cm.Parameters.Add("p_cdtipoendereco", OracleDbType.Double);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = Convert.ToDouble(ende.cdtipoendereco);

                                pm = cm.Parameters.Add("p_stprincipal", OracleDbType.Double);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = string.IsNullOrEmpty(ende.principal) ? 0 : Convert.ToDouble(ende.principal);

                                pm = cm.Parameters.Add("p_referencia", OracleDbType.Varchar2);
                                pm.Direction = System.Data.ParameterDirection.Input;
                                pm.Value = ende.referencia;


                                cm.ExecuteNonQuery();
                            }
                        }
                    }

                }


                //inserir os contatos e seus tels e emails

                foreach (clientecontato contato in pcliente.Contatos)
                {

                    double cdcontato = 0;

                    if (contato.excluir == 1)
                    {
                        //delete contato, emails telefones
                        string querydelcont = string.Format(" UPDATE CONTATO SET STEXCLUIDO = 1 WHERE CDCONTATO ={0}", contato.cdcontato);
                        OracleHelper.ExecProcedureNonQuery(querydelcont, null, System.Data.CommandType.Text, datasource, schema);

                        foreach (clientecontatoemail email in contato.Emails)
                        {
                            string querydelemail = string.Format(" UPDATE CONTEMAIL SET STEXCLUIDO = 1 WHERE CDCONTATO ={0} AND CDEMAIL ={1}", contato.cdcontato, email.cdemail);
                            OracleHelper.ExecProcedureNonQuery(querydelemail, null, System.Data.CommandType.Text, datasource, schema);
                        }

                        foreach (var tel in contato.Telefones)
                        {
                            string querydeltel = string.Format(" UPDATE conttelefone SET STEXCLUIDO = 1 WHERE CDCONTATO ={0} and CDTELEFONE={1}", contato.cdcontato, tel.cdtelefone);
                            OracleHelper.ExecProcedureNonQuery(querydeltel, null, System.Data.CommandType.Text, datasource, schema);
                        }

                    }
                    else
                    {
                        if (contato.excluir == 0)
                        {
                            using (OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
                            {
                                OracleParameter pm = null;
                                con.Open();
                                using (OracleCommand cm = new OracleCommand("pk_entidade.SP_GRAVACONTATOREST", con))
                                {
                                    cm.CommandType = System.Data.CommandType.StoredProcedure;

                                    cm.Parameters.Clear();

                                    pm = cm.Parameters.Add("p_CDCONTATO", OracleDbType.Double);
                                    pm.Direction = System.Data.ParameterDirection.Input;
                                    pm.Value = string.IsNullOrEmpty(contato.cdcontato) ? 0 : Convert.ToDouble(contato.cdcontato);

                                    pm = cm.Parameters.Add("p_CDENTIDADE", OracleDbType.Double);
                                    pm.Direction = System.Data.ParameterDirection.Input;
                                    pm.Value = cdentidade;

                                    pm = cm.Parameters.Add("p_nome", OracleDbType.Varchar2);
                                    pm.Direction = System.Data.ParameterDirection.Input;
                                    pm.Value = contato.nome;

                                    pm = cm.Parameters.Add("p_APELIDO", OracleDbType.Varchar2);
                                    pm.Direction = System.Data.ParameterDirection.Input;
                                    pm.Value = contato.apelido;

                                    pm = cm.Parameters.Add("p_CDTIPOCONTATO", OracleDbType.Double);
                                    pm.Direction = System.Data.ParameterDirection.Input;
                                    pm.Value = Convert.ToDouble(contato.cdtipocontato);

                                    pm = cm.Parameters.Add("p_STDEFAULT", OracleDbType.Double);
                                    pm.Direction = System.Data.ParameterDirection.Input;
                                    pm.Value = Convert.ToDouble(contato.stdefault);

                                    pm = cm.Parameters.Add("p_observacao", OracleDbType.Varchar2);
                                    pm.Direction = System.Data.ParameterDirection.Input;
                                    pm.Value = contato.observacao;

                                    pm = cm.Parameters.Add("P_CODNOVO", OracleDbType.Double);
                                    pm.Direction = System.Data.ParameterDirection.Output;

                                    cm.ExecuteNonQuery();
                                    cdcontato = Convert.ToDouble(cm.Parameters["P_CODNOVO"].Value.ToString());

                                    if (!string.IsNullOrEmpty(contato.dddcel) && !string.IsNullOrEmpty(contato.telcelular))
                                    {
                                        using (OracleConnection con2 = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
                                        {
                                            OracleParameter pm2 = null;
                                            con2.Open();
                                            using (OracleCommand cm2 = new OracleCommand("pk_entidade.SP_GRAVATELEFONEREST", con2))
                                            {
                                                cm2.CommandType = System.Data.CommandType.StoredProcedure;

                                                cm2.Parameters.Clear();

                                                pm2 = cm2.Parameters.Add("p_CDTELEFONE", OracleDbType.Double);
                                                pm2.Direction = System.Data.ParameterDirection.Input;
                                                pm2.Value = contato.cdtelefonecel;
                                                pm2 = cm2.Parameters.Add("p_CDCONTATO", OracleDbType.Double);
                                                pm2.Direction = System.Data.ParameterDirection.Input;
                                                pm2.Value = cdcontato;
                                                pm2 = cm2.Parameters.Add("p_nrdd", OracleDbType.Double);
                                                pm2.Direction = System.Data.ParameterDirection.Input;
                                                pm2.Value = Convert.ToDouble(contato.dddcel);
                                                pm2 = cm2.Parameters.Add("p_NRTELEFONE", OracleDbType.Double);
                                                pm2.Direction = System.Data.ParameterDirection.Input;
                                                pm2.Value = Convert.ToDouble(contato.telcelular.Replace("-", ""));
                                                pm2 = cm2.Parameters.Add("p_CDTIPOTELEFONE", OracleDbType.Double);
                                                pm2.Direction = System.Data.ParameterDirection.Input;
                                                pm2.Value = Convert.ToDouble(3);

                                                cm2.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(contato.dddresi) && !string.IsNullOrEmpty(contato.telresidencial))
                                    {
                                        using (OracleConnection con3 = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
                                        {
                                            /*
                                            PROCEDURE sp_gravatelefonerest (p_cdtelefone       IN NUMBER,
                                            p_cdcontato        IN NUMBER,
                                            p_nrdd             IN NUMBER,
                                            p_nrtelefone       IN NUMBER,
                                            p_cdtipotelefone   IN NUMBER)
                                             */
                                            OracleParameter pm3 = null;
                                            con3.Open();
                                            using (OracleCommand cm3 = new OracleCommand("pk_entidade.SP_GRAVATELEFONEREST", con3))
                                            {
                                                cm3.CommandType = System.Data.CommandType.StoredProcedure;

                                                cm3.Parameters.Clear();

                                                pm3 = cm3.Parameters.Add("p_CDTELEFONE", OracleDbType.Double);
                                                pm3.Direction = System.Data.ParameterDirection.Input;
                                                pm3.Value = Convert.ToDouble(contato.cdtelefoneresi);
                                                pm3 = cm3.Parameters.Add("p_CDCONTATO", OracleDbType.Double);
                                                pm3.Direction = System.Data.ParameterDirection.Input;
                                                pm3.Value = cdcontato;
                                                pm3 = cm3.Parameters.Add("p_nrdd", OracleDbType.Double);
                                                pm3.Direction = System.Data.ParameterDirection.Input;
                                                pm3.Value = Convert.ToDouble(contato.dddresi);
                                                pm3 = cm3.Parameters.Add("p_NRTELEFONE", OracleDbType.Double);
                                                pm3.Direction = System.Data.ParameterDirection.Input;
                                                pm3.Value = Convert.ToDouble(contato.telresidencial.Replace("-", ""));
                                                pm3 = cm3.Parameters.Add("p_CDTIPOTELEFONE", OracleDbType.Double);
                                                pm3.Direction = System.Data.ParameterDirection.Input;
                                                pm3.Value = Convert.ToDouble(2);

                                                cm3.ExecuteNonQuery();
                                            }
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(contato.email))
                                    {

                                        using (OracleConnection con4 = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
                                        {
                                            OracleParameter pm4 = null;
                                            con4.Open();
                                            using (OracleCommand cm4 = new OracleCommand("pk_entidade.SP_GRAVAEMAILREST", con4))
                                            {
                                                cm4.CommandType = System.Data.CommandType.StoredProcedure;

                                                cm4.Parameters.Clear();

                                                pm4 = cm4.Parameters.Add("p_CDEMAIL", OracleDbType.Double);
                                                pm4.Direction = System.Data.ParameterDirection.Input;
                                                pm4.Value = contato.cdemail;

                                                pm4 = cm4.Parameters.Add("p_CDCONTATO", OracleDbType.Double);
                                                pm4.Direction = System.Data.ParameterDirection.Input;
                                                pm4.Value = cdcontato;

                                                pm4 = cm4.Parameters.Add("p_EMAIIL", OracleDbType.Varchar2);
                                                pm4.Direction = System.Data.ParameterDirection.Input;
                                                pm4.Value = contato.email;

                                                pm4 = cm4.Parameters.Add("p_CDTIPOEMAIL", OracleDbType.Varchar2);
                                                pm4.Direction = System.Data.ParameterDirection.Input;
                                                pm4.Value = Convert.ToDouble(2);

                                                cm4.ExecuteNonQuery();
                                            }
                                        }

                                    }

                                }
                            }
                        }
                        else
                        {
                            string querydelcont = string.Format(" UPDATE CONTATO SET STEXCLUIDO = 1 WHERE CDCONTATO ={0}", contato.cdcontato);
                            OracleHelper.ExecProcedureNonQuery(querydelcont, null, System.Data.CommandType.Text, datasource, schema);
                        }

                        //foreach (clientecontatoemail email in contato.Emails)
                        //{

                        //    if (email.excluir == 0)
                        //    {
                        //        using (OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
                        //        {
                        //            OracleParameter pm = null;
                        //            con.Open();
                        //            using (OracleCommand cm = new OracleCommand("pk_entidade.SP_GRAVAEMAILREST", con))
                        //            {
                        //                cm.CommandType = System.Data.CommandType.StoredProcedure;

                        //                cm.Parameters.Clear();

                        //                pm = cm.Parameters.Add("p_CDEMAIL", OracleDbType.Double);
                        //                pm.Direction = System.Data.ParameterDirection.Input;
                        //                pm.Value = string.IsNullOrEmpty(email.cdemail) ? 0 : Convert.ToDouble(email.cdemail);

                        //                pm = cm.Parameters.Add("p_CDCONTATO", OracleDbType.Double);
                        //                pm.Direction = System.Data.ParameterDirection.Input;
                        //                pm.Value = cdcontato;

                        //                pm = cm.Parameters.Add("p_EMAIIL", OracleDbType.Varchar2);
                        //                pm.Direction = System.Data.ParameterDirection.Input;
                        //                pm.Value = email.txemail;

                        //                pm = cm.Parameters.Add("p_CDTIPOEMAIL", OracleDbType.Varchar2);
                        //                pm.Direction = System.Data.ParameterDirection.Input;
                        //                pm.Value = Convert.ToDouble(email.cdtipoemail);

                        //                cm.ExecuteNonQuery();
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        string querydelemail = string.Format(" UPDATE CONTEMAIL SET STEXCLUIDO = 1 WHERE CDCONTATO ={0} AND CDEMAIL ={1}", cdcontato, email.cdemail);
                        //        OracleHelper.ExecProcedureNonQuery(querydelemail, null, System.Data.CommandType.Text, datasource, schema);
                        //    }

                        //}
                        //foreach (clientecontatotelefone tel in contato.Telefones)
                        //{

                        //    if (tel.excluir == 0)
                        //    {
                        //        using (OracleConnection con = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
                        //        {
                        //            OracleParameter pm = null;
                        //            con.Open();
                        //            using (OracleCommand cm = new OracleCommand("pk_entidade.SP_GRAVATELEFONEREST", con))
                        //            {
                        //                cm.CommandType = System.Data.CommandType.StoredProcedure;

                        //                cm.Parameters.Clear();

                        //                pm = cm.Parameters.Add("p_CDTELEFONE", OracleDbType.Double);
                        //                pm.Direction = System.Data.ParameterDirection.Input;
                        //                pm.Value = string.IsNullOrEmpty(tel.cdtelefone) ? 0 : Convert.ToDouble(tel.cdtelefone);

                        //                pm = cm.Parameters.Add("p_CDCONTATO", OracleDbType.Double);
                        //                pm.Direction = System.Data.ParameterDirection.Input;
                        //                pm.Value = cdcontato;

                        //                pm = cm.Parameters.Add("p_nrdd", OracleDbType.Double);
                        //                pm.Direction = System.Data.ParameterDirection.Input;
                        //                pm.Value = Convert.ToDouble(tel.ddd);

                        //                pm = cm.Parameters.Add("p_NRTELEFONE", OracleDbType.Double);
                        //                pm.Direction = System.Data.ParameterDirection.Input;
                        //                pm.Value = Convert.ToDouble(tel.telefone.Replace("-", ""));

                        //                pm = cm.Parameters.Add("p_CDTIPOTELEFONE", OracleDbType.Double);
                        //                pm.Direction = System.Data.ParameterDirection.Input;
                        //                pm.Value = Convert.ToDouble(tel.cdtipotelefone);

                        //                cm.ExecuteNonQuery();
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        string querydeltel = string.Format(" UPDATE conttelefone SET STEXCLUIDO = 1 WHERE CDCONTATO ={0} and CDTELEFONE={1}", contato.cdcontato, tel.cdtelefone);
                        //        OracleHelper.ExecProcedureNonQuery(querydeltel, null, System.Data.CommandType.Text, datasource, schema);
                        //    }

                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                //Util.Log.For(this, ConfigurationManager.AppSettings.Get("pathlog")).Error(ex.ToString());
                throw ex;
            }
            return cdentidade;
        }

        public static List<cliente> RetornaClientByParam(clientesearch clis, string datasource, string schema)
        {
            List<cliente> lstResult = new List<cliente>();

            StringBuilder querycli = new StringBuilder();
            querycli.AppendLine("select * from vw_entiafiliado where 1 = 1");

            if (!string.IsNullOrEmpty(clis.cpfcnpj))
            {
                querycli.AppendLine(string.Format(" and cnpj_cpf = '{0}'", clis.cpfcnpj.Replace(".", "").Replace("/", "").Replace("-", "")));
            }
            else
            {
                querycli.AppendLine(string.Format(" and UPPER(razao_nome) like UPPER('%{0}%')", clis.nome));
            }


            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(querycli.ToString(), connection);
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
                        //cli.Email = reader["txemail"].ToString();
                        //cli.Endereco = reader["txlogradouro"].ToString();
                        //cli.Complemento = reader["txcomplemento"].ToString();
                        //cli.Numero = reader["txnumero"].ToString();
                        //cli.Bairro = reader["txbairro"].ToString();
                        //cli.Cidade = reader["txcidade"].ToString();
                        //cli.Uf = reader["cduf"].ToString();
                        //cli.Cep = reader["cdcep"].ToString();
                        //cli.cdendereco = Convert.ToInt32(reader["CDENDERECO"].ToString());
                        cli.identidade = reader["RG"].ToString();
                        cli.cdentiresp = reader["AGECODIGO"].ToString();

                        if (cli.tipopessoa == "J")
                        {
                            cli.CNPJ = reader["CNPJ_CPF"].ToString();
                            cli.CPF = "";
                        }
                        else
                        {
                            cli.CNPJ = "";
                            cli.CPF = reader["CNPJ_CPF"].ToString();
                        }

                        //cli.CPF = reader["CNPJ_CPF"].ToString();
                        //cli.CNPJ = reader["CNPJ_CPF"].ToString();
                        //cli.Enderecofull = cli.Endereco + " | " + cli.Bairro + " | " + cli.Cidade + " | " + cli.Uf;
                        //StringBuilder querytelefone = new StringBuilder();
                        //querytelefone.AppendLine(" select cdtipotelefone,nrddd,nrtelefone");
                        //querytelefone.AppendLine("             from conttelefone ct, contato c");
                        //querytelefone.AppendLine("                where c.cdcontato = ct.cdcontato");
                        //querytelefone.AppendLine("                and c.cdentidade = " + cli.cdentidade);
                        //querytelefone.AppendLine("                and ct.stexcluido = 0");
                        //querytelefone.AppendLine("                and c.stexcluido = 0 ");


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


                        lstResult.Add(cli);
                    }
                }
            }
            return lstResult.OrderBy(a => a.nome).ToList();
        }

        public static List<cliente> RetornaParceiroByParam(clientesearch clis, string datasource, string schema)
        {
            List<cliente> lstResult = new List<cliente>();

            StringBuilder querycli = new StringBuilder();
            querycli.AppendLine("select * from vw_entiparceiro where 1 = 1");

            if (!string.IsNullOrEmpty(clis.cpfcnpj))
            {
                querycli.AppendLine(string.Format(" and cnpj_cpf = '{0}'", clis.cpfcnpj.Replace(".", "").Replace("/", "").Replace("-", "")));
            }
            else
            {
                querycli.AppendLine(string.Format(" and UPPER(razao_nome) like UPPER('%{0}%')", clis.nome));
            }


            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(querycli.ToString(), connection);
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
                        //cli.Email = reader["txemail"].ToString();
                        //cli.Endereco = reader["txlogradouro"].ToString();
                        //cli.Complemento = reader["txcomplemento"].ToString();
                        //cli.Numero = reader["txnumero"].ToString();
                        //cli.Bairro = reader["txbairro"].ToString();
                        //cli.Cidade = reader["txcidade"].ToString();
                        //cli.Uf = reader["cduf"].ToString();
                        //cli.Cep = reader["cdcep"].ToString();
                        //cli.cdendereco = Convert.ToInt32(reader["CDENDERECO"].ToString());
                        cli.identidade = reader["RG"].ToString();
                        cli.cdentiresp = reader["AGECODIGO"].ToString();

                        if (cli.tipopessoa == "J")
                        {
                            cli.CNPJ = reader["CNPJ_CPF"].ToString();
                            cli.CPF = "";
                        }
                        else
                        {
                            cli.CNPJ = "";
                            cli.CPF = reader["CNPJ_CPF"].ToString();
                        }

                        //cli.CPF = reader["CNPJ_CPF"].ToString();
                        //cli.CNPJ = reader["CNPJ_CPF"].ToString();
                        //cli.Enderecofull = cli.Endereco + " | " + cli.Bairro + " | " + cli.Cidade + " | " + cli.Uf;
                        //StringBuilder querytelefone = new StringBuilder();
                        //querytelefone.AppendLine(" select cdtipotelefone,nrddd,nrtelefone");
                        //querytelefone.AppendLine("             from conttelefone ct, contato c");
                        //querytelefone.AppendLine("                where c.cdcontato = ct.cdcontato");
                        //querytelefone.AppendLine("                and c.cdentidade = " + cli.cdentidade);
                        //querytelefone.AppendLine("                and ct.stexcluido = 0");
                        //querytelefone.AppendLine("                and c.stexcluido = 0 ");


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


                        lstResult.Add(cli);
                    }
                }
            }
            return lstResult.OrderBy(a => a.nome).ToList();
        }

        public static cliente RetornaClientById(string cdentidade, string datasource, string schema)
        {
            cliente cli = new cliente();

            string querycli = string.Format("select * from vw_entiafiliado where cdentidade ={0}", cdentidade);


            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(querycli.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cli = new cliente();
                        cli.cdentidade = Convert.ToInt32(reader["cdentidade"].ToString());
                        cli.tipopessoa = reader["tipopessoa"].ToString();
                        cli.nome = reader["razao_nome"].ToString();
                        cli.fantasia = reader["fantasia_apelido"].ToString();
                        cli.identidade = reader["RG"].ToString();
                        cli.cdentiresp = reader["AGECODIGO"].ToString();
                        cli.sexo = reader["sexo"].ToString();
                        if (cli.tipopessoa == "J")
                        {
                            cli.CNPJ = reader["CNPJ_CPF"].ToString();
                        }
                        else
                        {
                            cli.CPF = reader["CNPJ_CPF"].ToString();
                        }

                        cli.IE = reader["NRIE"].ToString();
                        cli.IM = reader["NRIM"].ToString();

                        //string queryContato = " select * from contato where cdentidade = " + cli.cdentidade + " and stexcluido = 0";
                        string queryContato = " select nmapelido,cdcontato,c.cdtipocontato,nmcontato,txobservacao,stdefault, nmtipocontato from contato c,tipocontato tc  where cdentidade = " + cli.cdentidade + " and c.stexcluido = 0 and c.cdtipocontato = tc.cdtipocontato";

                        string queryEmail = " select cdcontato,cdemail,c.cdtipoemail,txemail,nmtipoemail from contemail c,tipoemail te  where cdcontato = {0} and c.stexcluido = 0 and c.cdtipoemail = te.cdtipoemail";

                        string querytelefone = " select cdtelefone,cdcontato,nmtipotelefone,c.cdtipotelefone,nrddd,nrtelefone from conttelefone c, tipotelefone tt where cdcontato = {0} and c.stexcluido = 0 and c.cdtipotelefone = tt.cdtipotelefone";

                        string queryEndereco = " select * from entiendereco e, tipoendereco t  where e.stexcluido = 0 and e.cdtipoendereco = t.cdtipoendereco and cdentidade =" + cli.cdentidade;


                        using (OracleCommand command2 = new OracleCommand(queryContato.ToString(), connection))
                        {
                            using (OracleDataReader reader2 = command2.ExecuteReader())
                            {
                                while (reader2.Read())
                                {
                                    clientecontato cont = new clientecontato();
                                    cont.apelido = reader2["nmapelido"].ToString();
                                    cont.cdcontato = reader2["cdcontato"].ToString();
                                    cont.cdtipocontato = reader2["cdtipocontato"].ToString();
                                    cont.nome = reader2["nmcontato"].ToString();
                                    cont.observacao = reader2["txobservacao"].ToString();
                                    cont.stdefault = reader2["stdefault"].ToString();
                                    cont.nmtipocontato = reader2["nmtipocontato"].ToString();
                                    cont.excluir = 0;
                                    cont.idx = 0;
                                    cont.cdtelefonecel = 0;
                                    cont.dddcel = "";
                                    cont.telcelular = string.Empty;
                                    cont.cdtelefoneresi = 0;
                                    cont.dddresi = "";
                                    cont.telresidencial = string.Empty;
                                    cont.cdemail = 0;
                                    cont.email = string.Empty;

                                    using (OracleCommand command3 = new OracleCommand(string.Format(queryEmail, cont.cdcontato), connection))
                                    {
                                        using (OracleDataReader reader3 = command3.ExecuteReader())
                                        {
                                            while (reader3.Read())
                                            {

                                                if (reader3["cdtipoemail"].ToString() == "2")
                                                {
                                                    cont.cdemail = Convert.ToInt32(reader3["cdemail"].ToString());
                                                    cont.email = reader3["txemail"].ToString();
                                                }
                                                else
                                                {
                                                    cont.cdemail = Convert.ToInt32(reader3["cdemail"].ToString());
                                                    cont.email = reader3["txemail"].ToString();
                                                }

                                            }
                                        }
                                    }
                                    using (OracleCommand command4 = new OracleCommand(string.Format(querytelefone, cont.cdcontato), connection))
                                    {
                                        using (OracleDataReader reader4 = command4.ExecuteReader())
                                        {
                                            while (reader4.Read())
                                            {
                                                if (reader4["cdtipotelefone"].ToString() == "2")
                                                {
                                                    cont.cdtelefoneresi = Convert.ToInt32(reader4["cdtelefone"].ToString());
                                                    cont.dddresi = reader4["nrddd"].ToString();
                                                    cont.telresidencial = reader4["nrtelefone"].ToString();
                                                }
                                                if (reader4["cdtipotelefone"].ToString() == "3")
                                                {
                                                    cont.cdtelefonecel = Convert.ToInt32(reader4["cdtelefone"].ToString());
                                                    cont.dddcel = reader4["nrddd"].ToString();
                                                    cont.telcelular = reader4["nrtelefone"].ToString();
                                                }

                                            }
                                        }
                                    }

                                    cli.Contatos.Add(cont);
                                }
                            }
                        }

                        using (OracleCommand command3 = new OracleCommand(queryEndereco, connection))
                        {
                            using (OracleDataReader reader3 = command3.ExecuteReader())
                            {
                                while (reader3.Read())
                                {
                                    clienteendereco ce = new clienteendereco();
                                    ce.cdcontato = reader3["cdcontato"].ToString();
                                    ce.bairro = reader3["txbairro"].ToString();
                                    ce.cdendereco = reader3["cdendereco"].ToString();
                                    ce.cdenderecoweb = reader3["cdenderecoweb"].ToString();
                                    ce.cdmunicipioibge = reader3["cdmunicipioibge"].ToString();
                                    ce.cdtipoendereco = reader3["cdtipoendereco"].ToString();
                                    ce.CEP = reader3["cdcep"].ToString();
                                    ce.cidade = reader3["txcidade"].ToString();
                                    ce.complemento = reader3["txcomplemento"].ToString();
                                    ce.logradouro = reader3["txlogradouro"].ToString();
                                    ce.nmenderecoweb = reader3["txnmenderecoweb"].ToString();
                                    ce.numero = reader3["txnumero"].ToString();
                                    ce.UF = reader3["cduf"].ToString();
                                    ce.referencia = reader3["txpontoreferencia"].ToString();
                                    ce.nmtipoendereco = reader3["nmtipoendereco"].ToString();
                                    ce.principal = reader3["stprincipal"].ToString();
                                    ce.excluir = 0;
                                    cli.Enderecos.Add(ce);
                                }
                            }
                        }
                    }
                }
            }
            return cli;
        }

        public static cliente RetornaClientByCpfCnpj(string cpfcnpj, string datasource, string schema)
        {
            cliente cli = new cliente();

            string querycli = string.Format("select * from vw_entiafiliado where cnpj_cpf = '{0}'", cpfcnpj.Replace(".", "").Replace("/", "").Replace("-", ""));

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(querycli.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cli = new cliente();
                        cli.cdentidade = Convert.ToInt32(reader["cdentidade"].ToString());
                        cli.tipopessoa = reader["tipopessoa"].ToString();
                        cli.nome = reader["razao_nome"].ToString();
                        cli.fantasia = reader["fantasia_apelido"].ToString();
                        cli.identidade = reader["RG"].ToString();
                        cli.cdentiresp = reader["AGECODIGO"].ToString();
                        cli.sexo = reader["sexo"].ToString();
                        if (cli.tipopessoa == "J")
                        {
                            cli.CNPJ = reader["CNPJ_CPF"].ToString();
                        }
                        else
                        {
                            cli.CPF = reader["CNPJ_CPF"].ToString();
                        }

                        cli.IE = reader["NRIE"].ToString();
                        cli.IM = reader["NRIM"].ToString();

                        //string queryContato = " select * from contato where cdentidade = " + cli.cdentidade + " and stexcluido = 0";
                        string queryContato = " select nmapelido,cdcontato,c.cdtipocontato,nmcontato,txobservacao,stdefault, nmtipocontato from contato c,tipocontato tc  where cdentidade = " + cli.cdentidade + " and c.stexcluido = 0 and c.cdtipocontato = tc.cdtipocontato";

                        string queryEmail = " select cdcontato,cdemail,c.cdtipoemail,txemail,nmtipoemail from contemail c,tipoemail te  where cdcontato = {0} and c.stexcluido = 0 and c.cdtipoemail = te.cdtipoemail";

                        string querytelefone = " select cdtelefone,cdcontato,nmtipotelefone,c.cdtipotelefone,nrddd,nrtelefone from conttelefone c, tipotelefone tt where cdcontato = {0} and c.stexcluido = 0 and c.cdtipotelefone = tt.cdtipotelefone";

                        string queryEndereco = " select * from entiendereco e, tipoendereco t  where e.stexcluido = 0 and e.cdtipoendereco = t.cdtipoendereco and cdentidade =" + cli.cdentidade;


                        using (OracleCommand command2 = new OracleCommand(queryContato.ToString(), connection))
                        {
                            using (OracleDataReader reader2 = command2.ExecuteReader())
                            {
                                while (reader2.Read())
                                {
                                    clientecontato cont = new clientecontato();
                                    cont.apelido = reader2["nmapelido"].ToString();
                                    cont.cdcontato = reader2["cdcontato"].ToString();
                                    cont.cdtipocontato = reader2["cdtipocontato"].ToString();
                                    cont.nome = reader2["nmcontato"].ToString();
                                    cont.observacao = reader2["txobservacao"].ToString();
                                    cont.stdefault = reader2["stdefault"].ToString();
                                    cont.nmtipocontato = reader2["nmtipocontato"].ToString();
                                    cont.excluir = 0;
                                    cont.idx = 0;
                                    cont.cdtelefonecel = 0;
                                    cont.dddcel = "";
                                    cont.telcelular = string.Empty;
                                    cont.cdtelefoneresi = 0;
                                    cont.dddresi = "";
                                    cont.telresidencial = string.Empty;
                                    cont.cdemail = 0;
                                    cont.email = string.Empty;

                                    using (OracleCommand command3 = new OracleCommand(string.Format(queryEmail, cont.cdcontato), connection))
                                    {
                                        using (OracleDataReader reader3 = command3.ExecuteReader())
                                        {
                                            while (reader3.Read())
                                            {

                                                if (reader3["cdtipoemail"].ToString() == "2")
                                                {
                                                    cont.cdemail = Convert.ToInt32(reader3["cdemail"].ToString());
                                                    cont.email = reader3["txemail"].ToString();
                                                }
                                                else
                                                {
                                                    cont.cdemail = Convert.ToInt32(reader3["cdemail"].ToString());
                                                    cont.email = reader3["txemail"].ToString();
                                                }

                                            }
                                        }
                                    }
                                    using (OracleCommand command4 = new OracleCommand(string.Format(querytelefone, cont.cdcontato), connection))
                                    {
                                        using (OracleDataReader reader4 = command4.ExecuteReader())
                                        {
                                            while (reader4.Read())
                                            {
                                                if (reader4["cdtipotelefone"].ToString() == "2")
                                                {
                                                    cont.cdtelefoneresi = Convert.ToInt32(reader4["cdtelefone"].ToString());
                                                    cont.dddresi = reader4["nrddd"].ToString();
                                                    cont.telresidencial = reader4["nrtelefone"].ToString();
                                                }
                                                if (reader4["cdtipotelefone"].ToString() == "3")
                                                {
                                                    cont.cdtelefonecel = Convert.ToInt32(reader4["cdtelefone"].ToString());
                                                    cont.dddcel = reader4["nrddd"].ToString();
                                                    cont.telcelular = reader4["nrtelefone"].ToString();
                                                }

                                            }
                                        }
                                    }

                                    cli.Contatos.Add(cont);
                                }
                            }
                        }

                        using (OracleCommand command3 = new OracleCommand(queryEndereco, connection))
                        {
                            using (OracleDataReader reader3 = command3.ExecuteReader())
                            {
                                while (reader3.Read())
                                {
                                    clienteendereco ce = new clienteendereco();
                                    ce.cdcontato = reader3["cdcontato"].ToString();
                                    ce.bairro = reader3["txbairro"].ToString();
                                    ce.cdendereco = reader3["cdendereco"].ToString();
                                    ce.cdenderecoweb = reader3["cdenderecoweb"].ToString();
                                    ce.cdmunicipioibge = reader3["cdmunicipioibge"].ToString();
                                    ce.cdtipoendereco = reader3["cdtipoendereco"].ToString();
                                    ce.CEP = reader3["cdcep"].ToString();
                                    ce.cidade = reader3["txcidade"].ToString();
                                    ce.complemento = reader3["txcomplemento"].ToString();
                                    ce.logradouro = reader3["txlogradouro"].ToString();
                                    ce.nmenderecoweb = reader3["txnmenderecoweb"].ToString();
                                    ce.numero = reader3["txnumero"].ToString();
                                    ce.UF = reader3["cduf"].ToString();
                                    ce.referencia = reader3["txpontoreferencia"].ToString();
                                    ce.nmtipoendereco = reader3["nmtipoendereco"].ToString();
                                    ce.principal = reader3["stprincipal"].ToString();
                                    ce.excluir = 0;
                                    cli.Enderecos.Add(ce);
                                }
                            }
                        }
                    }
                }
            }
            return cli;
        }

        public static List<ContatoRet> RetornaContatoByCdEntidade(string cdentidade, string datasource, string schema)
        {
            List<ContatoRet> listret = new List<ContatoRet>();

            ContatoRet cont = new ContatoRet();

            StringBuilder querycli = new StringBuilder();

            querycli.AppendLine(" select nmcontato ||' - ' ||tc.nmtipocontato as nmfull, cdentidade,cdcontato,nmcontato,nmapelido,stdefault,txobservacao,c.cdtipocontato, tc.nmtipocontato ");
            querycli.AppendLine(" from contato c , tipocontato tc  ");
            querycli.AppendLine(string.Format(" where c.stexcluido = 0 and tc.stexcluido = 0 and c.cdtipocontato = tc.cdtipocontato and c.cdentidade = {0} ", cdentidade));

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(querycli.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cont = new ContatoRet();
                        cont.nmfull = reader["nmfull"].ToString();
                        cont.cdentidade = reader["cdentidade"].ToString();
                        cont.cdcontato = reader["cdcontato"].ToString();
                        cont.nmcontato = reader["nmcontato"].ToString();
                        cont.nmapelido = reader["nmapelido"].ToString();
                        cont.stdefault = reader["stdefault"].ToString();
                        cont.txobservacao = reader["txobservacao"].ToString();
                        cont.cdtipocontato = reader["cdtipocontato"].ToString();
                        cont.nmtipocontato = reader["nmtipocontato"].ToString();
                        listret.Add(cont);
                    }
                }
            }
            return listret;
        }

        public static bool RetornaRetornaSeTemTelefone(string cdentidade, string datasource, string schema)
        {
            bool ret = false;

            cliente clie = ClienteADO.RetornaClientById(cdentidade, datasource, schema);

            foreach (clientecontato cont in clie.Contatos)
            {
                if (cont.cdtelefoneresi > 0 || cont.cdtelefonecel > 0)
                {
                    ret = true;
                }
            }
            return ret;
        }


    }
}