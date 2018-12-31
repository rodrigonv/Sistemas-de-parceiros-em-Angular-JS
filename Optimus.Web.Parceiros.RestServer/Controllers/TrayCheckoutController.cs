using Newtonsoft.Json;
using Optimus.Web.Parceiros.RestServer.Database;
using Optimus.Web.Parceiros.RestServer.Models;
using Optimus.Web.Parceiros.RestServer.Models.Traycheckout;
using Optimus.Web.Parceiros.RestServer.Util;
using Oracle.DataAccess.Client;
using RJS.Optimus.Biblioteca;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Optimus.Web.Parceiros.RestServer.Controllers
{
    [EnableCors("*", "*", "GET, POST")]
    [RoutePrefix("traycheckout")]
    public class TrayCheckoutController : ApiController
    {
        [AcceptVerbs("POST")]
        [Route("")]
        public HttpResponseMessage TrayPostTransaction(HttpRequestMessage request, [FromBody] TrayCheckoutPost tc)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();

            TrayCheckoutPaymentResponse tckResponse = new TrayCheckoutPaymentResponse();

            try
            {
                TrayCheckoutPayment tcp = new TrayCheckoutPayment();

                double vrtotalpedido = 0;

                CreateTransaction ctrans = new CreateTransaction();
                if (tc.payment_method_id == "6")
                {
                    tcp.payment_payment_method_id = tc.payment_method_id;
                    tcp.payment_split = "1";
                }
                else
                {
                    //dados do cartao vem do post
                    tcp.payment_card_cvv = tc.card_cvv;
                    tcp.payment_card_expdate_month = tc.card_expdate_month;
                    tcp.payment_card_expdate_year = tc.card_expdate_year;
                    tcp.payment_card_name = tc.card_name;
                    tcp.payment_card_number = tc.card_number;
                    tcp.payment_payment_method_id = tc.payment_method_id;
                    tcp.payment_split = tc.split.ToString();
                }

                //dados do produto
                List<PedidoPaypal> lstRet = PedidosADO.RetornaPedidoPagamento(tc.order_number, cf.datasource, cf.schema);
                vrtotalpedido = PedidosADO.RetornaValorTotalPedido(tc.order_number, cf.datasource, cf.schema);

                foreach (var prod in lstRet)
                {
                    transaction_product tp = new transaction_product();
                    tp.quantity = Convert.ToInt32(prod.quantidade);
                    tp.price_unit = prod.preco;
                    tp.description = prod.nmproduto;
                    tp.sku_code = prod.cdproduto;
                    tcp.Products.Add(tp);
                    //vrtotalpedido += Convert.ToInt32(prod.quantidade) * prod.preco;
                }

                string cdenderecoentrega = lstRet[0].cdenderecoentrega;
                double frete = lstRet[0].frete;
                //vrtotalpedido += frete;
                //Endereco do cliente
                Endereco ende = EnderecoADO.RetornaEnderecoById(cdenderecoentrega, cf.datasource, cf.schema);
                //frete
                if (frete > 0 )
                {
                    tcp.shipping_price = frete;
                    tcp.shipping_type = lstRet[0].txfrete;
                }
                


                //pegar os dados do cliente
                string cdcliente = lstRet[0].cdcliente;
                cliente clie = ClienteADO.RetornaClientById(cdcliente, cf.datasource, cf.schema);

                clientecontato contato = (from a in clie.Contatos
                                          where a.stdefault == "1" && !string.IsNullOrEmpty(a.email) && !string.IsNullOrEmpty(a.telresidencial) && !string.IsNullOrEmpty(a.dddresi)
                                          select a).FirstOrDefault();

                tcp.customer_addresses_city = ende.txcidade;
                tcp.customer_addresses_neighborhood = ende.txbairro;
                tcp.customer_addresses_number = ende.txnumero;
                tcp.customer_addresses_postal_code = ende.cdcep;
                tcp.customer_addresses_state = ende.cduf;
                tcp.customer_addresses_street = ende.txlogradouro;
                tcp.customer_addresses_type_address = "D";//entrega



                /*
                1	Contato     
                2	Pessoal     
                3	Profissional
                4	Bancário    
                5	Comercial   
                6	Referência  
                7	Marketing   
                8	Financeiro  
                9	Logistica                    
                 */
                /* tray contato
                    Residencial	H
                    Celular	M
                    Comercial	W
                 */
                string tipocontato = "H";

                //switch (contato.cdtipocontato)
                //{
                //    case "1":
                //    case "2":
                //    case "3":
                //    case "4":
                //    case "6":
                //    case "7":
                //    case "9":
                //    case "8":
                //        {
                //            tipocontato = "H";
                //            break;
                //        }
                //    case "5":
                //        {
                //            tipocontato = "W";
                //            break;
                //        }
                //    default:
                //        break;
                //}

                /* tipo tel
                4	Pessoal    
                1	Comercial  
                2	Residencial
                3	Celular                     
                */
                //clientecontatotelefone tel = (from t in contato.Telefones
                //                              where t.cdtipotelefone == "4"
                //                              select t).FirstOrDefault();

                //if (tel == null)
                //{
                //    tel = (from t in contato.Telefones
                //           where t.cdtipotelefone == "1"
                //           select t).FirstOrDefault();
                //}

                //if (tel == null)
                //{
                //    tel = (from t in contato.Telefones
                //           where t.cdtipotelefone == "2"
                //           select t).FirstOrDefault();
                //}

                //if (tel == null)
                //{
                //    tel = (from t in contato.Telefones
                //           where t.cdtipotelefone == "3"
                //           select t).FirstOrDefault();
                //}

                tcp.customer_contacts_number_contact = contato.dddresi + contato.telresidencial;//telefone

                /* tray
    Residencial	H
    Celular	M
    Comercial	W
 */

                tcp.customer_contacts_type_contact = tipocontato;


                tcp.customer_cpf = string.IsNullOrEmpty(clie.CPF) ? clie.CNPJ : clie.CPF;


                /*
                1 	Comercial      
                2 	Pessoal        
                3 	NF-e           
                21	Envio de e-mail
                 */
                clientecontatoemail email = (from e in contato.Emails
                                             where e.cdtipoemail == "1"
                                             select e).FirstOrDefault();
                if (email == null)
                {
                    email = (from e in contato.Emails
                             where e.cdtipoemail == "2"
                             select e).FirstOrDefault();
                }
                if (email == null)
                {
                    email = (from e in contato.Emails
                             where e.cdtipoemail == "3"
                             select e).FirstOrDefault();
                }

                if (email == null)
                {
                    tcp.customer_email = contato.email;
                    tcp.customer_name = clie.nome;
                }
                else
                {
                    tcp.customer_email = email.txemail;
                    tcp.customer_name = clie.nome;
                }



                // nome cpf email

                /*tray
                    2  	  Diners  
                    3  	  Visa  
                    4  	  Mastercard  
                    5  	  American Express  
                    6  	  Boleto Bancário  
                    7  	  Itaú Shopline (Transferência)  
                    14    Peela  
                    15    Discover  
                    16    Elo  
                    18    Aura  
                    19    JCB  
                    22    Transf. Online Bradesco  
                    23    Transf. Online Banco do Brasil 
                 */
                /*Optimus
                    * 1 	Dinheiro        
                    2 	Visa            
                    3 	Master          
                    4 	Transferencia   
                    5 	Paypal          
                    6 	Traycheckout    
                    7 	Boleto          
                    8 	Diners club     
                    9 	American Express
                    10	Elo             
                    11	Aura            
                 
                 */
                string cdformaopt = "";
                switch (tc.payment_method_id)
                {
                    case "3": cdformaopt = "2"; break;
                    case "2": cdformaopt = "8"; break;
                    case "4": cdformaopt = "3"; break;
                    case "5": cdformaopt = "9"; break;
                    case "6": cdformaopt = "7"; break;
                    case "16": cdformaopt = "10"; break;
                    case "18": cdformaopt = "11"; break;

                    default: cdformaopt = "4";
                        break;
                }

                List<pedidopagamento> lstpag = PagamentoADO.GravaPagamento(tc.order_number, "6", vrtotalpedido.ToString(), "N", "N", tcp.payment_split, "N", "0", tc.cdforn, "0", tckResponse.token_transaction, tckResponse.transaction_id, tckResponse.payment_response, tckResponse.status_id + "-" + tckResponse.status_name, tckResponse.payment_method_name, cf.datasource, cf.schema, cdformaopt);

                string codpagamento = lstpag[0].cdpag;
                string formdata = string.Empty;
                string urltrans = string.Empty;
                string qstring = string.Empty;
                tcp.transaction_order_number = tc.order_number;

                tckResponse = ctrans.CriarTransacao(tcp, codpagamento, cf.token, out formdata, out urltrans, out qstring);
                Util.LogUtil.GravaLog(this, "urltrans-->" + urltrans, cf.Cnpj, Log.TipoLog.info);
                Util.LogUtil.GravaLog(this, "formdata-->" + formdata, cf.Cnpj, Log.TipoLog.info);
                Util.LogUtil.GravaLog(this, "qstring-->" + qstring, cf.Cnpj, Log.TipoLog.info);

                tckResponse.codpagamento = codpagamento;

                string tkcstring = JsonConvert.SerializeObject(tckResponse);
                Util.LogUtil.GravaLog(this, "RETORNO TRAYCHECOUT-->" + tkcstring, cf.Cnpj, Log.TipoLog.info);

                //if (tckResponse.sucessResponse)
                //{
                //    PagamentoADO.GravaPagamento(tc.order_number, "5", vrtotalpedido.ToString(), "N", "N", tckResponse.split, "N", codpagamento, tc.cdforn, "0", tckResponse.token_transaction, tckResponse.transaction_id, tckResponse.payment_response, tckResponse.status_id + "-" + tckResponse.status_name, tckResponse.payment_method_name, cf.datasource, cf.schema);

                //    // tckResponse.codpagamento = lstpag[0].cdpag;
                //}


            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "TrayPostTransaction : " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, tckResponse);
        }

        [AcceptVerbs("GET")]
        [Route("finalize/{token}/{PayerID}/{codpag}")]
        public HttpResponseMessage RetornaFinalizaPagamentoTray(string token, string PayerID, string codpag)
        {
            ConfigAppMembers cf = Util.ConfigApp.getConfig();
            string ret = "";
            try
            {
                List<pedidopagamento> lstpag = PagamentoADO.BuscaPagamentoTituloByCodpag(codpag, cf.datasource, cf.schema);
                //PagamentoADO.GravaPagamento(lstpag[0].codped, "5", lstpag[0].valorpago, "N", "S", "1", "N", codpag, "", "A", token, PayerID, "", "", "", cf.datasource, cf.schema);
                ret = "ok";
            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaFinalizaPagamentoTray : " + e.ToString(), cf.Cnpj, Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [AcceptVerbs("POST")]
        [Route("notification")]
        public HttpResponseMessage RetornaFinalizaPagamentoTray(HttpRequestMessage request, [FromBody] TrayCheckoutNotificacao tn)
        {
            try
            {
                string posttray = JsonConvert.SerializeObject(tn);

                var queryString = request.RequestUri.ParseQueryString();
                string copagto = queryString["cp"];
                Util.LogUtil.GravaLog(this, "RetornaFinalizaPagamentoTray NOTIFICACAO OPTOKEN ANTES: " + queryString["opttoken"].ToString(), "traycheckout", Log.TipoLog.info);
                string opttoken = queryString["opttoken"].ToString().Replace(' ', '+');
                Util.LogUtil.GravaLog(this, "RetornaFinalizaPagamentoTray NOTIFICACAO copagto: " + copagto, "traycheckout", Log.TipoLog.info);
                Util.LogUtil.GravaLog(this, "RetornaFinalizaPagamentoTray NOTIFICACAO OPTOKEN DEPOIS: " + opttoken, "traycheckout", Log.TipoLog.info);
                Util.LogUtil.GravaLog(this, "RetornaFinalizaPagamentoTray NOTIFICACAO : " + posttray, "traycheckout", Log.TipoLog.info);
                int statusret = 0;
                TrayCheckoutNotification tcn = new TrayCheckoutNotification();
                TrayCheckouttransactionRet tcret = tcn.ConfirmaRecebimento(string.IsNullOrEmpty(tn.token_transaction) ? tn.token : tn.token_transaction, out statusret);
                string tcnstr = JsonConvert.SerializeObject(tcret);
                Util.LogUtil.GravaLog(this, "RetornaFinalizaPagamentoTray NOTIFICACAO confirma: " + tcnstr, "traycheckout", Log.TipoLog.info);
                /*
                    Aguardando Pagamento	4
                    Em Processamento	5
                    Aprovada	6
                    Cancelada	7
                    Em Contestação	24
                    Em Monitoramento	87
                    Em Recuperação	88
                    Reprovada	89
                 */

                //atualizar o pagamento de acordo com o status
                string token = Util.Cripto.Decrypt(opttoken.Replace(" ", "+"), ConfigurationManager.AppSettings.Get("CHAVE"));
                string[] val = token.Split('|');

                string datasource = val[1];//datasource
                string schema = val[2];//schema


                PedidoRetTrayNotificacao prt = PedidosADO.RetornaPedidoTrayckoutConfirm(copagto, datasource, schema);

                //Retorna os dados do pagamento para gravar novamente o pagamento
                if (tcret.status_id == "6")
                {
                    List<pedidopagamento> lstpag = PagamentoADO.GravaPagamento(prt.codped, "6", prt.valorpago,"N", "S", prt.parcelas, "N", prt.codpag, prt.operador, "0", tcret.token_transaction, tcret.transaction_id, tcret.payment_response, tcret.status_id + "-" + tcret.status_name, tcret.payment_payment_method_name, datasource, schema, prt.cdformapagamento);
                }
                else if (tcret.status_id == "7" || tcret.status_id == "89")
                {
                    List<pedidopagamento> lstpag = PagamentoADO.GravaPagamento(prt.codped, "6", prt.valorpago, "N", "N", prt.parcelas, "S", prt.codpag, prt.operador, "0", tcret.token_transaction, tcret.transaction_id, tcret.payment_response, tcret.status_id + "-" + tcret.status_name, tcret.payment_payment_method_name, datasource, schema, prt.cdformapagamento);
                }
                var aa = tn.token;

            }
            catch (Exception e)
            {
                Util.LogUtil.GravaLog(this, "RetornaFinalizaPagamentoTray NOTIFICACAO : " + e.ToString(), "traycheckout", Log.TipoLog.erro);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ERRO");
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }



}
