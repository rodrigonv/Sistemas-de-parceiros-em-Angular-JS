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
    public class PedidosADO
    {
        public static List<Pedido> RetornaPedidos(string cdentifilial, string email, string datasource, string schema)
        {
            List<Pedido> lstpedido = new List<Pedido>();
            Pedido pedPaiRetorno = new Pedido();
            PedidoItem pedItemRetorno = new PedidoItem();
            PedidoStatus pedStatusRetorno = new PedidoStatus();

            List<string> Cursores = new List<string>();
            Cursores.Add("P_CURSOR_PED");
            Cursores.Add("P_CURSOR_ITEMPED");
            Cursores.Add("P_CURSOR_STATUS_PED");
            List<OracleParameter> lstParam = new List<OracleParameter>();
            DataSet dtsRetorno = null;

            DataRow[] dtrItens = null;
            DataRow[] dtrStatus = null;

            int cont = 0;
            try
            {

                lstParam.Add(new OracleParameter("PCDENTIFILIAL", cdentifilial));
                lstParam.Add(new OracleParameter("P_EMAIL", email));

                dtsRetorno = OracleHelper.ExecProcedure("PK_APP.SP_RETORNA_PEDIDOS", lstParam, Cursores, Util.OracleHelper.StrConn(schema, datasource));

                foreach (DataRow rowPedido in dtsRetorno.Tables[0].Rows)
                {
                    pedPaiRetorno = new Pedido();
                    //pedido
                    pedPaiRetorno.cdpedoptimus = rowPedido["CDPEDOPTIMUS"].ToString();
                    pedPaiRetorno.cdpedweb = rowPedido["CDPEDWEB"].ToString();
                    //pedPaiRetorno.vrfrete = rowPedido["VRFRETE"].ToString();
                    pedPaiRetorno.vrtotal = Util.Formatar.FormataValorDecimal(rowPedido["VRTOTAL"].ToString());
                    pedPaiRetorno.vrdesconto = Util.Formatar.FormataValorDecimal(rowPedido["VRDESCONTO"].ToString());
                    //pedPaiRetorno.CodCliOptimus = rowPedido["CODCLIOPTIMUS"].ToString();
                    pedPaiRetorno.codobjcorreio = rowPedido["CODOBJCORREIO"].ToString();
                    pedPaiRetorno.statusrastreio = rowPedido["STATUSRASTREIO"].ToString();
                    pedPaiRetorno.formpagamento = rowPedido["FORMPAGAMENTO"].ToString();
                    //pedPaiRetorno.Nsu = rowPedido["NSU"].ToString();
                    pedPaiRetorno.parcelas = rowPedido["Parcelas"].ToString();
                    pedPaiRetorno.statuspedido = rowPedido["STATUSPEDIDO"].ToString();
                    // pedPaiRetorno.DataPagamento = rowPedido["DTPAGAMENTO"].ToString();
                    // pedPaiRetorno.CdFilial = rowPedido["CDFILIAL"].ToString();
                    pedPaiRetorno.dtpedido = Convert.ToDateTime(rowPedido["DTPEDIDO"].ToString()).ToString("dd/MM/yyyy HH:mm");
                    pedPaiRetorno.urlnota = rowPedido["URLNOTA"].ToString();
                    dtrItens = dtsRetorno.Tables[1].Select("CDPEDIDO = " + pedPaiRetorno.cdpedoptimus);


                    foreach (DataRow rowItem in dtrItens)
                    {
                        pedItemRetorno = new PedidoItem();
                        pedItemRetorno.cdpedoptimus = rowItem["CDPEDIDO"].ToString();
                        //pedItemRetorno.CdPedItem = cont.ToString();
                        pedItemRetorno.vrunitario = Util.Formatar.FormataValorDecimal(rowItem["VRUNITARIO"].ToString());
                        pedItemRetorno.quantidade = Convert.ToInt32(rowItem["QTDE"].ToString());
                        pedItemRetorno.cdproduto = rowItem["CDPRODUTO"].ToString();
                        pedItemRetorno.nmproprodduto = rowItem["NMPROPRODDUTO"].ToString();
                        pedItemRetorno.ean = rowItem["EAN"].ToString();
                        pedItemRetorno.vrtotal = Util.Formatar.FormataValorDecimal(Convert.ToString(Convert.ToInt32(rowItem["QTDE"].ToString()) * Convert.ToDouble(rowItem["VRUNITARIO"].ToString())));
                        pedPaiRetorno.Itens.Add(pedItemRetorno);
                    }

                    dtrStatus = dtsRetorno.Tables[2].Select("CDPEDIDO = " + pedPaiRetorno.cdpedoptimus);

                    foreach (DataRow rowItem in dtrStatus)
                    {
                        pedStatusRetorno = new PedidoStatus();
                        pedStatusRetorno.cdpedoptimus = rowItem["CDPEDIDO"].ToString();
                        pedStatusRetorno.cdpedweb = rowItem["PEFACDPEDIDOWEB"].ToString();
                        //pedItemRetorno.CdPedItem = cont.ToString();
                        pedStatusRetorno.nmstatus = rowItem["NMSTATUS"].ToString();
                        pedStatusRetorno.cdstatuspedido = rowItem["cdstatuspedido"].ToString();
                        pedStatusRetorno.statussite = rowItem["PEFATXSTATUSSITE"].ToString();
                        pedStatusRetorno.datastatus = Convert.ToDateTime(rowItem["DATA"].ToString()).ToString("dd/MM/yyyy HH:mm");
                        pedPaiRetorno.Status.Add(pedStatusRetorno);

                    }

                    lstpedido.Add(pedPaiRetorno);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dtsRetorno.Dispose();
            }

            return lstpedido;

        }

        public static Pedido RetornaPedido(string cdentifilial, string email, int nrpedido, string datasource, string schema)
        {
            List<Pedido> lstpedido = new List<Pedido>();
            Pedido pedPaiRetorno = new Pedido();
            PedidoItem pedItemRetorno = new PedidoItem();
            PedidoStatus pedStatusRetorno = new PedidoStatus();

            List<string> Cursores = new List<string>();
            Cursores.Add("P_CURSOR_PED");
            Cursores.Add("P_CURSOR_ITEMPED");
            Cursores.Add("P_CURSOR_STATUS_PED");
            List<OracleParameter> lstParam = new List<OracleParameter>();
            DataSet dtsRetorno = null;

            DataRow[] dtrItens = null;
            DataRow[] dtrStatus = null;

            int cont = 0;
            try
            {

                lstParam.Add(new OracleParameter("PCDENTIFILIAL", cdentifilial));
                lstParam.Add(new OracleParameter("P_EMAIL", email));
                lstParam.Add(new OracleParameter("P_NRPEDIDO", nrpedido));

                dtsRetorno = OracleHelper.ExecProcedure("PK_APP.SP_RETORNA_PEDIDOS", lstParam, Cursores, Util.OracleHelper.StrConn(schema, datasource));

                foreach (DataRow rowPedido in dtsRetorno.Tables[0].Rows)
                {
                    pedPaiRetorno = new Pedido();
                    //pedido
                    pedPaiRetorno.cdpedoptimus = rowPedido["CDPEDOPTIMUS"].ToString();
                    pedPaiRetorno.cdpedweb = rowPedido["CDPEDWEB"].ToString();
                    //pedPaiRetorno.vrfrete = rowPedido["VRFRETE"].ToString();
                    pedPaiRetorno.vrtotal = Util.Formatar.FormataValorDecimal(rowPedido["VRTOTAL"].ToString());
                    pedPaiRetorno.vrdesconto = Util.Formatar.FormataValorDecimal(rowPedido["VRDESCONTO"].ToString());
                    //pedPaiRetorno.CodCliOptimus = rowPedido["CODCLIOPTIMUS"].ToString();
                    pedPaiRetorno.codobjcorreio = rowPedido["CODOBJCORREIO"].ToString();
                    pedPaiRetorno.statusrastreio = rowPedido["STATUSRASTREIO"].ToString();
                    pedPaiRetorno.formpagamento = rowPedido["FORMPAGAMENTO"].ToString();
                    //pedPaiRetorno.Nsu = rowPedido["NSU"].ToString();
                    pedPaiRetorno.parcelas = rowPedido["Parcelas"].ToString();
                    pedPaiRetorno.statuspedido = rowPedido["STATUSPEDIDO"].ToString();
                    // pedPaiRetorno.DataPagamento = rowPedido["DTPAGAMENTO"].ToString();
                    // pedPaiRetorno.CdFilial = rowPedido["CDFILIAL"].ToString();
                    pedPaiRetorno.dtpedido = Convert.ToDateTime(rowPedido["DTPEDIDO"].ToString()).ToString("dd/MM/yyyy HH:mm");
                    pedPaiRetorno.urlnota = rowPedido["URLNOTA"].ToString();
                    dtrItens = dtsRetorno.Tables[1].Select("CDPEDIDO = " + pedPaiRetorno.cdpedoptimus);


                    foreach (DataRow rowItem in dtrItens)
                    {
                        pedItemRetorno = new PedidoItem();
                        pedItemRetorno.cdpedoptimus = rowItem["CDPEDIDO"].ToString();
                        //pedItemRetorno.CdPedItem = cont.ToString();
                        pedItemRetorno.vrunitario = Util.Formatar.FormataValorDecimal(rowItem["VRUNITARIO"].ToString());
                        pedItemRetorno.quantidade = Convert.ToInt32(rowItem["QTDE"].ToString());
                        pedItemRetorno.cdproduto = rowItem["CDPRODUTO"].ToString();
                        pedItemRetorno.nmproprodduto = rowItem["NMPROPRODDUTO"].ToString();
                        pedItemRetorno.ean = rowItem["EAN"].ToString();
                        pedItemRetorno.vrtotal = Util.Formatar.FormataValorDecimal(Convert.ToString(Convert.ToInt32(rowItem["QTDE"].ToString()) * Convert.ToDouble(rowItem["VRUNITARIO"].ToString())));
                        pedPaiRetorno.Itens.Add(pedItemRetorno);
                    }

                    dtrStatus = dtsRetorno.Tables[2].Select("CDPEDIDO = " + pedPaiRetorno.cdpedoptimus);

                    foreach (DataRow rowItem in dtrStatus)
                    {
                        pedStatusRetorno = new PedidoStatus();
                        pedStatusRetorno.cdpedoptimus = rowItem["CDPEDIDO"].ToString();
                        pedStatusRetorno.cdpedweb = rowItem["PEFACDPEDIDOWEB"].ToString();
                        //pedItemRetorno.CdPedItem = cont.ToString();
                        pedStatusRetorno.nmstatus = rowItem["NMSTATUS"].ToString();
                        pedStatusRetorno.cdstatuspedido = rowItem["cdstatuspedido"].ToString();
                        pedStatusRetorno.statussite = rowItem["PEFATXSTATUSSITE"].ToString();
                        pedStatusRetorno.datastatus = Convert.ToDateTime(rowItem["DATA"].ToString()).ToString("dd/MM/yyyy HH:mm");
                        pedPaiRetorno.Status.Add(pedStatusRetorno);

                    }

                    lstpedido.Add(pedPaiRetorno);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dtsRetorno.Dispose();
            }

            return lstpedido.FirstOrDefault();

        }


        public static List<PedidoConsulta> RetornaPedidosForn(PedidoPesquisa ped, string cdentifilial, string datasource, string schema, out string queryt)
        {

            List<PedidoConsulta> lstret = new List<PedidoConsulta>();

            StringBuilder query = new StringBuilder();

            query.AppendLine("select * from vw_consultacupom where 1=1 ");

            if (!string.IsNullOrEmpty(ped.datainicio) && !string.IsNullOrEmpty(ped.datafim))
            {
                query.AppendLine(string.Format("and to_date(dataped,'DD/MM/YYYY') between to_date('{0}','DD/MM/YYYY') and to_date('{1}','DD/MM/YYYY')", ped.datainicio, ped.datafim));
            }

            if (!string.IsNullOrEmpty(ped.cpfcnpj))
            {
                var pcpf = ped.cpfcnpj.Replace(".", "").Replace("/", "").Replace("-", "");
                query.AppendLine(string.Format("and cpfcnpj = '{0}'", pcpf));
            }

            if (!string.IsNullOrEmpty(ped.statuspedido))
            {
                query.AppendLine(string.Format("and cdstatpedido = {0}", ped.statuspedido));
            }

            if (!string.IsNullOrEmpty(ped.codigopedido))
            {
                query.AppendLine(string.Format("and cdpedido = {0}", ped.codigopedido));
            }
            if (ped.perfil == "PARCEIRO")
            {
                if (!string.IsNullOrEmpty(ped.cdforn))
                {
                    query.AppendLine(string.Format("and CDFORN = {0}", ped.cdforn));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ped.cdpai))
                {
                    query.AppendLine(string.Format("and CDFORN = {0}", ped.cdpai));
                }
            }

            if (!string.IsNullOrEmpty(ped.nome))
            {
                query.AppendLine(string.Format("and nmentidade like upper('%{0}%')", ped.nome));
            }

            if (!string.IsNullOrEmpty(cdentifilial))
            {
                query.AppendLine(string.Format("and cdentifilial ={0}", cdentifilial));
            }
            queryt = query.ToString();

            PedidoConsulta pc;
            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(query.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    Produto pr = null;
                    while (reader.Read())
                    {
                        pc = new PedidoConsulta();
                        pc.cdentifilial = reader["CDENTIFILIAL"].ToString();
                        pc.cdpedido = reader["CDPEDIDO"].ToString();
                        pc.cdstatpedido = reader["CDSTATPEDIDO"].ToString();
                        //pc.cdstatuspedido = reader["cdstatuspedido"].ToString();
                        pc.cpfcnpj = reader["CPFCNPJ"].ToString();
                        pc.dataped = reader["DATAPED"].ToString();
                        pc.nmentidade = reader["NMENTIDADE"].ToString();
                        pc.nmstatus = reader["NMSTATUS"].ToString();
                        pc.quantidade = reader["QUANTIDADE"].ToString();
                        pc.statusnfe = reader["STATUSNFE"].ToString();
                        pc.valortotal = Convert.ToDouble(reader["VALORTOTAL"].ToString());
                        pc.cdnotafiscalsaida = string.IsNullOrEmpty(reader["CDNOTAFISCALSAIDA"].ToString()) ? "0" : reader["CDNOTAFISCALSAIDA"].ToString();
                        pc.flalteravel = reader["flalteravel"].ToString();
                        pc.comissao = Convert.ToDouble(reader["comissao"].ToString());
                        pc.over = Convert.ToDouble(reader["over"].ToString());
                        pc.totalcomissao = Convert.ToDouble(reader["totalcomissao"].ToString());
                        lstret.Add(pc);
                    }
                }
            }
            return lstret;
        }

        public static string AtualizaFrete(string cdpedido, double vrfrete, string txtipofrete, string tokenaxado, string cdentifilial, string cdenderecoentregra, string datasource, string schema)
        {

            string cdmodalidade = string.Empty;
            StringBuilder queryRetModa = new StringBuilder();
            queryRetModa.AppendLine(" SELECT nmtranspmodalidade,cdservicoobjeto,instr(upper('" + txtipofrete.Trim() + "'),upper(TRIM(nmtranspmodalidade))) ORDEM ");
            queryRetModa.AppendLine(" FROM transpmodalidade WHERE stexcluido=0 and instr(upper('" + txtipofrete.Trim() + "'),upper(TRIM(nmtranspmodalidade)))>0 and cdentifilial = " + cdentifilial + " ORDER BY 3");
            DataSet dsmoda = new DataSet();
            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(queryRetModa.ToString(), connection);
                connection.Open();

                using (OracleDataAdapter adapter = new OracleDataAdapter(queryRetModa.ToString(), connection))
                {
                    adapter.Fill(dsmoda);
                }

                //pegar a primeira
                cdmodalidade = dsmoda.Tables[0].Rows[0]["cdservicoobjeto"].ToString();

                string queryupdate = " UPDATE pedidofaturado SET pefacdservicoentrega =:pefacdservicoentrega, pefavrfrete=:pefavrfrete,pefatxtipofrete =:pefatxtipofrete, pefatxrastreioweb = :pefatxrastreioweb, pefacdenderecoentrega=:pefacdenderecoentrega where  pefanrpedido =:pefanrpedido";

                command = new OracleCommand(queryupdate.ToString(), connection);
                command.BindByName = true;
                command.Parameters.Add("pefacdservicoentrega", cdmodalidade);
                command.Parameters.Add("pefavrfrete", vrfrete);
                command.Parameters.Add("pefatxtipofrete", txtipofrete);
                command.Parameters.Add("pefatxrastreioweb", tokenaxado);
                command.Parameters.Add("pefanrpedido", cdpedido);
                command.Parameters.Add("pefacdenderecoentrega", cdenderecoentregra);

                //connection.Open();
                command.ExecuteNonQuery();
            }

            return "ok";
        }


        public static List<PedidoPaypal> RetornaPedidoPagamento(string codigopedido, string datasource, string schema)
        {

            List<PedidoPaypal> lstret = new List<PedidoPaypal>();

            StringBuilder query = new StringBuilder();

            query.AppendLine(" select cagecdcadastro,pefacdenderecoentrega,p.pefatxtipofrete, pi.prodcdproduto as cdproduto , p.pefanrpedido as codped,UPPER(nmproduto) as nmproduto, pi.pefivrpreconota as preco,pi.pefiqtfaturada as quantidade, nvl(max(p.pefavrfrete),0) as frete, ");
            query.AppendLine("  ft_comissaoparceiro(pi.pefivrprecovenda*pi.pefiqtfaturada,pi.pefivrpreconota* pi.pefiqtfaturada ,NVL((select max(cdformapagamento) from pedidofaturadopagamento where pefanrpedido =p.pefanrpedido),0),NVL((select max(pefpqtparcelas) from pedidofaturadopagamento where pefanrpedido =p.pefanrpedido),0),1,1) as totalcomissao,   ");
            query.AppendLine("  ft_comissaoparceiro(pi.pefivrprecovenda*pi.pefiqtfaturada,pi.pefivrpreconota* pi.pefiqtfaturada ,NVL((select max(cdformapagamento) from pedidofaturadopagamento where pefanrpedido =p.pefanrpedido),0),NVL((select max(pefpqtparcelas) from pedidofaturadopagamento where pefanrpedido =p.pefanrpedido),0),1,0) as over,   ");
            query.AppendLine("  ft_comissaoparceiro(pi.pefivrprecovenda*pi.pefiqtfaturada,pi.pefivrpreconota* pi.pefiqtfaturada ,NVL((select max(cdformapagamento) from pedidofaturadopagamento where pefanrpedido =p.pefanrpedido),0),NVL((select max(pefpqtparcelas) from pedidofaturadopagamento where pefanrpedido =p.pefanrpedido),0),0,1) as comissao ");
            query.AppendLine(" from pedidofaturado p, pedidofaturadoitem pi , vw_tele_produto vw ");
            query.AppendLine(" where p.pefanrpedido = pi.pefanrpedido ");
            query.AppendLine(" and pi.prodcdproduto = vw.cdproduto ");
            query.AppendLine(" and p.pefanrpedido =" + codigopedido);
            query.AppendLine(" group by cagecdcadastro,pefacdenderecoentrega,p.pefatxtipofrete, pi.prodcdproduto,p.pefanrpedido , UPPER(nmproduto),pefivrmercadoria,pi.pefivrpreconota,pi.pefiqtfaturada,pi.pefivrprecovenda");


            PedidoConsulta pc;
            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(query.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    PedidoPaypal ped = null;
                    while (reader.Read())
                    {
                        ped = new PedidoPaypal();
                        ped.cdproduto = reader["cdproduto"].ToString();
                        ped.codped = reader["codped"].ToString();
                        ped.frete = Convert.ToDouble(reader["frete"].ToString());
                        ped.nmproduto = reader["nmproduto"].ToString();
                        ped.preco = Convert.ToDouble(reader["preco"].ToString());
                        ped.quantidade = Convert.ToDouble(reader["quantidade"].ToString());
                        ped.txfrete = reader["pefatxtipofrete"].ToString();
                        ped.cdenderecoentrega = reader["pefacdenderecoentrega"].ToString();
                        ped.cdcliente = reader["cagecdcadastro"].ToString();
                        ped.comissao = Convert.ToDouble(reader["comissao"].ToString());
                        ped.over = Convert.ToDouble(reader["over"].ToString());
                        ped.totalcomissao = Convert.ToDouble(reader["totalcomissao"].ToString());
                        lstret.Add(ped);
                    }
                }
            }
            return lstret;
        }

        public static double RetornaValorTotalPedido(string codpedpagar, string datasource, string schema)
        {
            double ret = 0;

            StringBuilder query = new StringBuilder();

            query.AppendLine("select sum(total) as totalpagar from (");
            query.AppendLine("select pi.pefiqtfaturada * pi.pefivrpreconota as total");
            query.AppendLine("from pedidofaturadoitem pi ");
            query.AppendLine("where pi.pefanrpedido =" + codpedpagar);
            query.AppendLine("union all");
            query.AppendLine("select nvl(max(pf.pefavrfrete),0) as total from pedidofaturado pf where pf.pefanrpedido = " + codpedpagar + " )");

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(query.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret = Convert.ToDouble(reader["totalpagar"].ToString());
                    }
                }
            }

            return ret;

        }

        public static PedidoPagamentoDet RetornaPedidoPagamentoDet(string codpedpagar, string datasource, string schema)
        {
            PedidoPagamentoDet ret = new PedidoPagamentoDet();

            StringBuilder query = new StringBuilder();

            query.AppendLine("select pefpvrpago vrpedido, pefpvrtroco as vrfrete,pefpdtpagamento as dtpagamento,pefprecibocliente as rettransacao,sitefusn as bandeira,");
            query.AppendLine("hostusn as statustray,pefpqtparcelas as parcelas ");
            query.AppendLine("from pedidofaturadopagamento  where pefanrpedido =" + codpedpagar);

            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(query.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret = new PedidoPagamentoDet();
                        ret.bandeira = reader["bandeira"].ToString();
                        ret.dtpagamento = reader["dtpagamento"].ToString();
                        ret.parcelas = reader["parcelas"].ToString();
                        ret.rettransacao = reader["rettransacao"].ToString();
                        ret.statustray = reader["statustray"].ToString();
                        ret.vrfrete = Convert.ToDouble(reader["vrfrete"].ToString());
                        ret.vrpedido = Convert.ToDouble(reader["vrpedido"].ToString());
                    }
                }
            }

            return ret;

        }


        public static PedidoDetalheRet RetornaPedidoDetalhe(string codpedpagar, string datasource, string schema)
        {
            PedidoDetalheRet ret = new PedidoDetalheRet();


            ret.pedidoPagamentoDet = RetornaPedidoPagamentoDet(codpedpagar, datasource, schema);
            ret.lstprodutos = RetornaPedidoPagamento(codpedpagar, datasource, schema);

            PedidoPaypal pedp = (from a in ret.lstprodutos
                                 select a).FirstOrDefault();


            ret.endereco = EnderecoADO.RetornaEnderecoById(pedp.cdenderecoentrega, datasource, schema);

            var cdcliente = (from a in ret.lstprodutos
                             select a).FirstOrDefault();

            ret.cliente = ClienteADO.RetornaClientById(cdcliente.cdcliente, datasource, schema);

            return ret;
        }


        public static PedidoRetTrayNotificacao RetornaPedidoTrayckoutConfirm(string codpedpagar, string datasource, string schema)
        {
            PedidoRetTrayNotificacao ret = new PedidoRetTrayNotificacao();
            StringBuilder query = new StringBuilder();
            query.Append("     SELECT ");
            query.AppendLine(" pefanrpedido codped,");
            query.AppendLine(" fopacdforma forma,");
            query.AppendLine(" pefpvrpago valorpago,");
            query.AppendLine(" pefpqtparcelas parcelas,");
            query.AppendLine(" pefpcdpagamento codpag,");
            query.AppendLine(" pefpcdoperadorpag operador,");
            query.AppendLine(" pefpnrautorizacao nrautorizacao,");
            query.AppendLine(" cdformapagamento");
            query.AppendLine(" FROM pedidofaturadopagamento WHERE  pefpcdpagamento =" + codpedpagar);


            using (OracleConnection connection = new OracleConnection(Util.OracleHelper.StrConn(schema, datasource)))
            {
                OracleCommand command = new OracleCommand(query.ToString(), connection);
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    PedidoPaypal ped = null;
                    while (reader.Read())
                    {
                        ret = new PedidoRetTrayNotificacao();
                        ret.codped = reader["codped"].ToString();
                        ret.forma = reader["forma"].ToString();
                        ret.valorpago = reader["valorpago"].ToString();
                        ret.parcelas = reader["parcelas"].ToString();
                        ret.codpag = reader["codpag"].ToString();
                        ret.operador = reader["operador"].ToString();
                        ret.nrautorizacao = reader["nrautorizacao"].ToString();
                        ret.cdformapagamento = reader["cdformapagamento"].ToString();
                    }
                }
            }

            return ret;

        }
    }
    /*
     select pefpvrpago vrpedido, pefpvrtroco as vrfrete,pefpdtpagamento as dtpagamento,pefprecibocliente as rettransacao,sitefusn as bandeira,
     * hostusn as statustray,pefpqtparcelas as parcelas
from pedidofaturadopagamento  where pefanrpedido =2598
     */
    public class PedidoDetalheRet
    {
        public PedidoDetalheRet()
        {
            this.cliente = new cliente();
            this.lstprodutos = new List<PedidoPaypal>();
            this.pedidoPagamentoDet = new PedidoPagamentoDet();
            this.endereco = new Endereco();
        }
        public cliente cliente { get; set; }
        public List<PedidoPaypal> lstprodutos { get; set; }

        public PedidoPagamentoDet pedidoPagamentoDet { get; set; }

        public Endereco endereco { get; set; }



    }
    public class PedidoPagamentoDet
    {
        public double vrpedido { get; set; }
        public double vrfrete { get; set; }

        public string dtpagamento { get; set; }
        public string rettransacao { get; set; }

        public string bandeira { get; set; }

        public string statustray { get; set; }
        public string parcelas { get; set; }
    }

    public class PedidoRetTrayNotificacao
    {
        public string codped { get; set; }
        public string forma { get; set; }
        public string valorpago { get; set; }
        public string parcelas { get; set; }
        public string codpag { get; set; }
        public string operador { get; set; }
        public string nrautorizacao { get; set; }

        public string cdformapagamento { get; set; }
    }
}