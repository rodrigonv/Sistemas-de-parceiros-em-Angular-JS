using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Util;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Database
{
    public class ProdutoADO
    {
        public static List<Produto> RetornaProdutoPesquisa(produtosearch valor, string datasource, string schema, string cdentifilial)
        {
            List<Produto> lstResult = new List<Produto>();

            StringBuilder queryNmproduto = new StringBuilder();
            queryNmproduto.AppendLine(string.Format("select * from vw_estoque_parceiro where cdentifilial ={0} ", cdentifilial));
            queryNmproduto.AppendFormat(" and cdproduto not in (select cdproduto from prodparceiro where cdentifornecedor = {0} and stexcluido=0)", valor.codforn);
            if (!string.IsNullOrEmpty(valor.codigoproduto))
            {
                queryNmproduto.AppendLine(string.Format(" and cdproduto = {0}", valor.codigoproduto));
            }
            else if (!string.IsNullOrEmpty(valor.nome))
            {
                queryNmproduto.AppendLine(string.Format(" and UPPER(nmproduto) LIKE UPPER('%{0}%')", valor.nome));
            }

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryNmproduto.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    Produto pr = null;
                    while (reader.Read())
                    {
                        pr = new Produto();
                        pr.cdproduto = reader["cdproduto"].ToString();
                        pr.nmproduto = reader["nmproduto"].ToString();
                        pr.estoque = reader["estoque"].ToString();
                        pr.cdentifilial = reader["cdentifilial"].ToString();
                        pr.cdprodlegado = reader["cdprodlegado"].ToString();
                        pr.preco = reader["preco"].ToString();
                        pr.precoview = pr.estoque == "DISPONÍVEL" ? reader["precoview"].ToString() : "";
                        pr.peso = Convert.ToDouble(reader["NRPESO"].ToString());
                        lstResult.Add(pr);
                    }
                }
            }
            return lstResult;
        }
        public static List<Produto> RetornaProdutoPesquisaSemFilial(produtosearch valor, string datasource, string schema, string cdentifilial)
        {
            List<Produto> lstResult = new List<Produto>();

            StringBuilder queryNmproduto = new StringBuilder();

            queryNmproduto.AppendLine(" select ");
            queryNmproduto.AppendLine(" cdproduto,");
            queryNmproduto.AppendLine(" UPPER(nmproduto) AS nmproduto,");
            queryNmproduto.AppendLine(" CASE WHEN ");
            queryNmproduto.AppendLine(" (select NVL(sum(estoque),0) from vw_tele_produto t where t.cdproduto = v.cdproduto) > 0 THEN 'DISPONÍVEL' ELSE 'INDISPONÍVEL' END AS ESTOQUE,");
            queryNmproduto.AppendLine(" 1 as cdentifilial,");
            queryNmproduto.AppendLine(" v.cdprodlegado,");
            queryNmproduto.AppendLine(" preco,'R$ ' || TRIM(TO_CHAR(preco,'L999G999G999G999D99','NLS_NUMERIC_CHARACTERS = '',. ''NLS_CURRENCY = '' '' ')) as precoview,");
            queryNmproduto.AppendLine(" NRPESO");
            queryNmproduto.AppendLine(" from vw_tele_produto v ");
            queryNmproduto.AppendFormat(" where cdproduto not in (select cdproduto from prodparceiro where cdentifornecedor = {0} and stexcluido=0)", valor.codforn);
            
            if (!string.IsNullOrEmpty(valor.codigoproduto))
            {
                queryNmproduto.AppendLine(string.Format(" and cdproduto = {0}", valor.codigoproduto));
            }
            else if (!string.IsNullOrEmpty(valor.nome))
            {
                queryNmproduto.AppendLine(string.Format(" and UPPER(nmproduto) LIKE UPPER('%{0}%')", valor.nome));
            }
            else
            {
                queryNmproduto.AppendLine(string.Format(" and cdprodlegado = '{0}'", valor.codigolegado));
            }

            queryNmproduto.AppendLine(" group by cdproduto,nmproduto,v.cdprodlegado,preco,NRPESO");

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryNmproduto.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    Produto pr = null;
                    while (reader.Read())
                    {
                        pr = new Produto();
                        pr.cdproduto = reader["cdproduto"].ToString();
                        pr.nmproduto = reader["nmproduto"].ToString();
                        pr.estoque = reader["estoque"].ToString();
                        pr.cdentifilial = reader["cdentifilial"].ToString();
                        pr.cdprodlegado = reader["cdprodlegado"].ToString();
                        pr.preco = reader["preco"].ToString();
                        pr.precoview = pr.estoque == "DISPONÍVEL" ? reader["precoview"].ToString() : "";
                        pr.peso = Convert.ToDouble(reader["NRPESO"].ToString());
                        lstResult.Add(pr);
                    }
                }
            }
            return lstResult;
        }


        public static List<Produto> CadastrarProdParceiro(string cdproduto, string cdentifilial, string cdforn, string datasource, string schema)
        {
            List<Produto> lstret = new List<Produto>();

            string queryprod = string.Format("INSERT INTO prodparceiro (CDENTIFILIAL,CDENTIFORNECEDOR,CDPRODUTO) VALUES({0},{1},{2})", cdentifilial, cdforn, cdproduto);
            OracleHelper.ExecProcedureNonQuery(queryprod, null, System.Data.CommandType.Text, datasource, schema);

            lstret = RetornaProdutoPesquisaParceiro(datasource, schema, cdforn);

            return lstret;

        }

        public static void CadastrarProdParceironolist(string cdproduto, string cdentifilial, string cdforn, string datasource, string schema)
        {
            string queryprod = string.Format("INSERT INTO prodparceiro (CDENTIFILIAL,CDENTIFORNECEDOR,CDPRODUTO) VALUES({0},{1},{2})", cdentifilial, cdforn, cdproduto);
            OracleHelper.ExecProcedureNonQuery(queryprod, null, System.Data.CommandType.Text, datasource, schema);
        }

        public static void ExcluirProdParceiro(string cdprodparceiro, string datasource, string schema)
        {
            string queryprod = string.Format("UPDATE PRODPARCEIRO SET STEXCLUIDO = 1 WHERE cdprodparceiro = {0}", cdprodparceiro);
            OracleHelper.ExecProcedureNonQuery(queryprod, null, System.Data.CommandType.Text, datasource, schema);
        }


        public static List<Produto> RetornaProdutoPesquisaParceiro(string datasource, string schema, string cdforn)
        {
            List<Produto> lstResult = new List<Produto>();

            StringBuilder queryNmproduto = new StringBuilder();
            queryNmproduto.AppendLine(string.Format("select * from vw_estoque_parceiro where cdentifornecedor = {0}", cdforn));

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryNmproduto.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    Produto pr = null;
                    while (reader.Read())
                    {
                        pr = new Produto();
                        pr.cdproduto = reader["cdproduto"].ToString();
                        pr.nmproduto = reader["nmproduto"].ToString();
                        pr.estoque = reader["estoque"].ToString();
                        pr.cdentifilial = reader["cdentifilial"].ToString();
                        pr.cdprodlegado = reader["cdprodlegado"].ToString();
                        pr.preco = reader["preco"].ToString();
                        pr.precoview = pr.estoque == "DISPONÍVEL" ? reader["precoview"].ToString() : "";
                        pr.peso = Convert.ToDouble(reader["NRPESO"].ToString());
                        pr.cdprodparceiro = reader["cdprodparceiro"].ToString();
                        lstResult.Add(pr);
                    }
                }
            }
            return lstResult;
        }

        public static List<Produto> RetornaProdutoPesquisaParceiroSel(string datasource, string schema, produtosearch valor)
        {
            List<Produto> lstResult = new List<Produto>();

            StringBuilder queryNmproduto = new StringBuilder();
            queryNmproduto.AppendLine(string.Format("select * from vw_estoque_parceiro where cdentifornecedor = {0} ", valor.codforn));
            if (!string.IsNullOrEmpty(valor.codigoproduto))
            {
                queryNmproduto.AppendLine(string.Format(" and cdproduto = {0}", valor.codigoproduto));
            }
            else if (!string.IsNullOrEmpty(valor.nome))
            {
                queryNmproduto.AppendLine(string.Format(" and UPPER(nmproduto) LIKE UPPER('%{0}%')", valor.nome));
            }
            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryNmproduto.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    Produto pr = null;
                    while (reader.Read())
                    {
                        pr = new Produto();
                        pr.cdproduto = reader["cdproduto"].ToString();
                        pr.nmproduto = reader["nmproduto"].ToString();
                        pr.estoque = reader["estoque"].ToString();
                        pr.cdentifilial = reader["cdentifilial"].ToString();
                        pr.cdprodlegado = reader["cdprodlegado"].ToString();
                        pr.preco = reader["preco"].ToString();
                        pr.precoview = pr.estoque == "DISPONÍVEL" ? reader["precoview"].ToString() : "";
                        pr.peso = Convert.ToDouble(reader["NRPESO"].ToString());
                        pr.cdprodparceiro = reader["cdprodparceiro"].ToString();
                        lstResult.Add(pr);
                    }
                }
            }
            return lstResult;
        }
    }
}