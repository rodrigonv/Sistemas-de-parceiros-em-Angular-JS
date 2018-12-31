using Optimus.Web.Parceiros.RestServer.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Database
{
    public class EnderecoADO
    {
        public static List<Endereco> RetornaEnderecoByCdContato(string cdcontato, string datasource, string schema)
        {
            List<Endereco> lstResult = new List<Endereco>();

            string queryend = "select cdendereco,cdcontato,cduf,cdcep,txcidade,txlogradouro,txbairro,txnumero,txcomplemento,cdmunicipioibge,cdpais from entiendereco where stexcluido = 0 and cdcontato = " + cdcontato;

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryend, connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    Endereco end = null;
                    while (reader.Read())
                    {
                        end = new Endereco();
                        end.cdendereco = Convert.ToInt32(reader["cdendereco"].ToString());
                        end.cdcep = reader["cdcep"].ToString();
                        end.cdcontato = Convert.ToInt32(reader["cdcontato"].ToString());
                        end.cdmunicipioibge = Convert.ToInt32(reader["cdmunicipioibge"].ToString());
                        end.cdpais = Convert.ToInt32(reader["cdpais"].ToString());
                        end.cduf = reader["cduf"].ToString();
                        end.txbairro = reader["txbairro"].ToString();
                        end.txcidade = reader["txcidade"].ToString();
                        end.txcomplemento = reader["txcomplemento"].ToString();
                        end.txlogradouro = reader["txlogradouro"].ToString();
                        end.txnumero = reader["txnumero"].ToString();
                        lstResult.Add(end);
                    }
                }
            }
            return lstResult.OrderBy(a => a.cdendereco).ToList();
        }

        public static List<Endereco> RetornaEnderecoByCdEntidade(string cdentidade, string datasource, string schema)
        {
            List<Endereco> lstResult = new List<Endereco>();

            string queryend = "select cdendereco,cdcontato,cduf,cdcep,txcidade,txlogradouro,txbairro,txnumero,txcomplemento,cdmunicipioibge,cdpais from entiendereco where stexcluido = 0 and cdentidade = " + cdentidade;

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryend, connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    Endereco end = null;
                    while (reader.Read())
                    {
                        end = new Endereco();
                        end.cdendereco = Convert.ToInt32(reader["cdendereco"].ToString());
                        end.cdcep = reader["cdcep"].ToString();
                        end.cdcontato = Convert.ToInt32(reader["cdcontato"].ToString());
                        end.cdmunicipioibge = Convert.ToInt32(reader["cdmunicipioibge"].ToString());
                        end.cdpais = Convert.ToInt32(reader["cdpais"].ToString());
                        end.cduf = reader["cduf"].ToString();
                        end.txbairro = reader["txbairro"].ToString();
                        end.txcidade = reader["txcidade"].ToString();
                        end.txcomplemento = reader["txcomplemento"].ToString();
                        end.txlogradouro = reader["txlogradouro"].ToString();
                        end.txnumero = reader["txnumero"].ToString();
                        lstResult.Add(end);
                    }
                }
            }
            return lstResult.OrderBy(a => a.cdendereco).ToList();
        }

        public static Endereco RetornaEnderecoById(string cdendereco, string datasource, string schema)
        {
            Endereco Result = new Endereco();

            string queryend = "select cdendereco,cdcontato,cduf,cdcep,txcidade,txlogradouro,txbairro,txnumero,txcomplemento,cdmunicipioibge,cdpais from entiendereco where stexcluido = 0 and cdendereco = " + cdendereco;

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryend, connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    Endereco end = null;
                    while (reader.Read())
                    {
                        end = new Endereco();
                        end.cdendereco = Convert.ToInt32(reader["cdendereco"].ToString());
                        end.cdcep = reader["cdcep"].ToString();
                        end.cdcontato = Convert.ToInt32(reader["cdcontato"].ToString());
                        end.cdmunicipioibge = Convert.ToInt32(reader["cdmunicipioibge"].ToString());
                        end.cdpais = Convert.ToInt32(reader["cdpais"].ToString());
                        end.cduf = reader["cduf"].ToString();
                        end.txbairro = reader["txbairro"].ToString();
                        end.txcidade = reader["txcidade"].ToString();
                        end.txcomplemento = reader["txcomplemento"].ToString();
                        end.txlogradouro = reader["txlogradouro"].ToString();
                        end.txnumero = reader["txnumero"].ToString();
                        Result = end;
                    }
                }
            }
            return Result;
        }
        
    }
}