using RJS.Optimus.Biblioteca;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class NFEGnre
    {

        string PastaLogCliente = string.Empty;
        long cnpjEmpresa = 0;
        string multiplasfiliais = string.Empty;
        string pastaSchema = string.Empty;
        int cdentifilial = 0;
        string datasource = string.Empty;
        string schema = string.Empty;
        DataTable DTNfeGNRE = new DataTable();
        NFEHelper nhp = new NFEHelper();
        System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");

        RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes.NFEHelper.InfoUFICMS infoUF = new NFEHelper.InfoUFICMS();
        WorkFlowServico _WorkFlowServico = new WorkFlowServico("NFEGNRE");

        public void GerarGNRE(
            string _cdentifilial,
            string _cnpjCliente,
            string _multiplasfiliais,
            string _datasource,
            string _schema,
            string _pastaLogWs,
             ObjEmail _objemailadm,
            string datasourceDoce,
            string schemaDoce)
        {
            cnpjEmpresa = Convert.ToInt64(_cnpjCliente);
            multiplasfiliais = _multiplasfiliais == "1" ? "TRUE" : "FALSE";
            cdentifilial = Convert.ToInt32(_cdentifilial);
            datasource = _datasource;
            schema = _schema;
            PastaLogCliente = string.Format(@"{0}\{1}_{2}_{3}", _pastaLogWs, schema, cdentifilial, _cnpjCliente);

            _WorkFlowServico.AddEvento("------------------------GNRE------------------------");
            _WorkFlowServico.AddEvento("FILIAL " + _cdentifilial + " CNPJ " + _cnpjCliente);

            DTNfeGNRE = nhp.RetornaGNRE(_cdentifilial, multiplasfiliais, datasource, schema);


            if (DTNfeGNRE.Rows.Count > 0)
            {
                foreach (DataRow dr in DTNfeGNRE.Rows)
                {
                    string _CDNOTAFISCALSAIDA = dr["CDNOTAFISCALSAIDA"].ToString();
                    string _NRNOTA = dr["NRNOTA"].ToString();
                    string _DTEMISSAO = dr["DTEMISSAO"].ToString();
                    string _CDNFEAMBIENTE = dr["CDNFEAMBIENTE"].ToString();
                    string _CDENTIFILIAL = dr["CDENTIFILIAL"].ToString();
                    string _RAZAOSOCIALEMITENTE = dr["RAZAOSOCIALEMITENTE"].ToString();
                    string _ENDERECOEMITENTE = dr["ENDERECOEMITENTE"].ToString();
                    string _MUNICIPIOEMITENTE = dr["MUNICIPIOEMITENTE"].ToString();
                    string _UFENDERECOEMITENTE = dr["UFENDERECOEMITENTE"].ToString();
                    string _CEPEMITENTE = dr["CEPEMITENTE"].ToString();
                    string _TELEFONE = dr["TELEFONE"].ToString();
                    string _CPFCNPJEMIT = dr["CPFCNPJEMIT"].ToString();
                    string _TIPOPESSOA = dr["TIPOPESSOA"].ToString();
                    string _CPFCNPJDEST = dr["CPFCNPJDEST"].ToString();
                    string _CDNFESTATUS = dr["CDNFESTATUS"].ToString();
                    string _NRNFECHAVE = dr["NRNFECHAVE"].ToString();
                    string _RAZAOSOCIALDESTINATARIO = dr["RAZAOSOCIALDESTINATARIO"].ToString();
                    string _MUNICIPIODESTINATARIO = dr["MUNICIPIODESTINATARIO"].ToString();
                    string _UFFAVORECIDA = dr["UFFAVORECIDA"].ToString();
                    string _CDSTATUSGNRE = dr["CDSTATUSGNRE"].ToString();
                    string _CDSTATUSGNREFCP = dr["CDSTATUSGNREFCP"].ToString();
                    string _CERTSUBJECT = dr["CERTSUBJECT"].ToString();
                    string _CERTTHUMBPRINT = dr["CERTTHUMBPRINT"].ToString();
                    string _VALORGNREICMSDEST = dr["GNREICMSDEST"].ToString();
                    string _VALORGNREFCPDEST = dr["GNREFCPDEST"].ToString();
                    string _PROTGNRE = dr["PROTGNRE"].ToString();
                    string _PROTGNREFCP = dr["PROTGNREFCP"].ToString();
                    infoUF = nhp.RetornaInfosUF(_UFFAVORECIDA, datasource, schema);

                    if (_CDSTATUSGNRE == "0")
                    {
                        _WorkFlowServico.AddEvento("GNRE NORMAL CDNOTAFISCALSAIDA=" + _CDNOTAFISCALSAIDA);
                        int tipognre = 1;//normal
                        try
                        {
                            string retorno = this.EnviaLoteGNRE(
                                                                                _UFFAVORECIDA,
                                                                                infoUF.cdReceitaGNRE,
                                                                                "2",
                                                                                "2",
                                                                                _CPFCNPJEMIT,
                                                                                "10",
                                                                                _NRNOTA,
                                                                                _VALORGNREICMSDEST,
                                                                                _VALORGNREICMSDEST,
                                                                                Convert.ToDateTime(_DTEMISSAO),
                                                                                _RAZAOSOCIALEMITENTE,
                                                                                _RAZAOSOCIALEMITENTE,
                                                                                _ENDERECOEMITENTE,
                                                                                _MUNICIPIOEMITENTE.Substring(2, 5),
                                                                                _UFENDERECOEMITENTE,
                                                                                _CEPEMITENTE,
                                                                                _TELEFONE,
                                                                                _RAZAOSOCIALDESTINATARIO,
                                                                                _MUNICIPIODESTINATARIO.Substring(2, 5),
                                                                                _CPFCNPJDEST,
                                                                                Convert.ToDateTime(_DTEMISSAO),
                                                                                _NRNFECHAVE,
                                                                                _CERTSUBJECT, _CERTTHUMBPRINT, infoUF.urlgnre, _CDNFEAMBIENTE, Convert.ToInt32(_CDNOTAFISCALSAIDA),
                                                                                tipognre, infoUF.cdCampoExtra, PastaLogCliente, _objemailadm, datasourceDoce, schemaDoce, _CDENTIFILIAL
                                                                                );

                            _WorkFlowServico.AddEvento("GNRE ENVIADA : LOTE" + retorno);
                            _WorkFlowServico.AddEvento("----------------------------------------------------");
                            Log.For(this, PastaLogCliente).Info(_WorkFlowServico.FinishWorkFlow());
                        }
                        catch (Exception ex)
                        {
                            nhp.NotafiscalAtualizarGNREStatus(Convert.ToInt32(_CDNOTAFISCALSAIDA), 99, datasource, schema, tipognre);
                            _WorkFlowServico.AddEvento("ERRO GNRE CDNOTAFISCAL=" + _CDNOTAFISCALSAIDA + " catch " + ex.ToString());
                            _WorkFlowServico.AddEvento("--------------------------------------------------");
                            Log.For(this, PastaLogCliente).Info(_WorkFlowServico.FinishWorkFlow());
                            _objemailadm.SUBJECT = "[C] GNRE ERRO: " + cdentifilial.ToString() + " SCHEMA:" + schema;
                            _objemailadm.BODY = _WorkFlowServico.FinishWorkFlow() + " Exception:" + ex.ToString();
                            Util.EnviaEmail(_objemailadm, "opterro@optimuserp.com.br");
                        }
                    }
                    else if (_CDSTATUSGNRE == "1")
                    {
                        try
                        {
                            //buscar o lote
                            string retconsulta = this.ConsultaLoteEnviado(_UFFAVORECIDA, _CERTSUBJECT, _CERTTHUMBPRINT, infoUF.urlgnre, _CDNFEAMBIENTE, Convert.ToInt32(_CDNOTAFISCALSAIDA),
                                1, PastaLogCliente, _objemailadm, _datasource, _schema, datasourceDoce, schemaDoce, _CDENTIFILIAL, _PROTGNRE);
                            Log.For(this, PastaLogCliente).Info(retconsulta);
                        }
                        catch (Exception ex)
                        {

                            Log.For(this, PastaLogCliente).Error("ERRO BUSCA RETORNO-> " + ex.ToString());
                            _objemailadm.SUBJECT = "[C] GNRE FCP BUSCA RETORNO ERRO: " + cdentifilial.ToString() + " SCHEMA:" + schema;
                            _objemailadm.BODY = " Exception:" + ex.ToString();
                            Util.EnviaEmail(_objemailadm, "opterro@optimuserp.com.br");
                        }
                    }

                    //POBREZA

                    if (_CDSTATUSGNREFCP == "0")
                    {
                        _WorkFlowServico.AddEvento("GNRE FCP CDNOTAFISCALSAIDA=" + _CDNOTAFISCALSAIDA);
                        int tipognre = 2;//POBREZA
                        try
                        {

                            string retorno = this.EnviaLoteGNRE(
                                                                                _UFFAVORECIDA,
                                                                                infoUF.cdReceitaGNRECFP,
                                                                                "2",
                                                                                "2",
                                                                                _CPFCNPJEMIT,
                                                                                "10",
                                                                                _NRNOTA,
                                                                                _VALORGNREFCPDEST,
                                                                                _VALORGNREFCPDEST,
                                                                                 Convert.ToDateTime(_DTEMISSAO),
                                                                                _RAZAOSOCIALEMITENTE,
                                                                                _RAZAOSOCIALEMITENTE,
                                                                                _ENDERECOEMITENTE,
                                                                                _MUNICIPIOEMITENTE.Substring(2, 5),
                                                                                _UFENDERECOEMITENTE,
                                                                                _CEPEMITENTE,
                                                                                _TELEFONE,
                                                                                _RAZAOSOCIALDESTINATARIO,
                                                                                _MUNICIPIODESTINATARIO.Substring(2, 5),
                                                                                _CPFCNPJDEST,
                                                                                 Convert.ToDateTime(_DTEMISSAO),
                                                                                _NRNFECHAVE,
                                                                                _CERTSUBJECT, _CERTTHUMBPRINT, infoUF.urlgnre, _CDNFEAMBIENTE, Convert.ToInt32(_CDNOTAFISCALSAIDA),
                                                                                tipognre, infoUF.cdCampoExtra, PastaLogCliente, _objemailadm, datasourceDoce, schemaDoce, _CDENTIFILIAL
                                                                                );

                            _WorkFlowServico.AddEvento("GNRE FCP ENVIADA : LOTE" + retorno);
                            _WorkFlowServico.AddEvento("----------------------------------------------------");
                            Log.For(this, PastaLogCliente).Info(_WorkFlowServico.FinishWorkFlow());
                        }
                        catch (Exception ex)
                        {
                            nhp.NotafiscalAtualizarGNREStatus(Convert.ToInt32(_CDNOTAFISCALSAIDA), 99, datasource, schema, tipognre);
                            _WorkFlowServico.AddEvento("ERRO GNRE FCP CDNOTAFISCAL=" + _CDNOTAFISCALSAIDA + " catch " + ex.ToString());
                            _WorkFlowServico.AddEvento("--------------------------------------------------");
                            Log.For(this, PastaLogCliente).Info(_WorkFlowServico.FinishWorkFlow());
                            _objemailadm.SUBJECT = "[C] GNRE FCP ERRO: " + cdentifilial.ToString() + " SCHEMA:" + schema;
                            _objemailadm.BODY = _WorkFlowServico.FinishWorkFlow() + " Exception:" + ex.ToString();
                            Util.EnviaEmail(_objemailadm, "opterro@optimuserp.com.br");
                        }
                    }
                    else if (_CDSTATUSGNREFCP == "1")
                    {
                        try
                        {
                            //buscar o lote
                            string retconsulta = this.ConsultaLoteEnviado(_UFFAVORECIDA, _CERTSUBJECT, _CERTTHUMBPRINT, infoUF.urlgnre, _CDNFEAMBIENTE, Convert.ToInt32(_CDNOTAFISCALSAIDA),
                                2, PastaLogCliente, _objemailadm, _datasource, _schema, datasourceDoce, schemaDoce, _CDENTIFILIAL, _PROTGNREFCP);
                            Log.For(this, PastaLogCliente).Info(retconsulta);
                        }
                        catch (Exception ex)
                        {

                            Log.For(this, PastaLogCliente).Error("ERRO BUSCA RETORNO-> " + ex.ToString());
                            _objemailadm.SUBJECT = "[C] GNRE FCP BUSCA RETORNO ERRO: " + cdentifilial.ToString() + " SCHEMA:" + schema;
                            _objemailadm.BODY = " Exception:" + ex.ToString();
                            Util.EnviaEmail(_objemailadm, "opterro@optimuserp.com.br");
                        }
                    }


                }
            }
        }


        #region Soap

        public string MontaXmlConsultaLote(string ambiente, string numerorecibo)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendLine(" <gnreDadosMsg xmlns=\"http://www.gnre.pe.gov.br/webservice/GnreResultadoLote\">");
            xml.AppendLine("      <TConsLote_GNRE xmlns=\"http://www.gnre.pe.gov.br\">");
            xml.AppendLine(string.Format("    <ambiente>{0}</ambiente>", ambiente));
            xml.AppendLine(string.Format("     <numeroRecibo>{0}</numeroRecibo>", numerorecibo));
            xml.AppendLine(" </TConsLote_GNRE>");
            xml.AppendLine("</gnreDadosMsg>");

            return xml.ToString();

        }


        public string MontaXmlLoteGnre(
         string UFfavorecida,
         string CodigoReceita,
         string produto,
         string tipoIdentificacaoEmitente,
         string CPFCNPJEmit,
         string tipoDocOrigem,
         string docOrigem,
         string valorPrincipal,
         string valorTotal,
         DateTime dataVencimento,
         string convenio,
         string razaoSocialEmitente,
         string enderecoEmitente,
         string municipioEmitente,
         string ufEnderecoEmitente,
         string cepEmitente,
         string telefoneEmitente,
         string razaoSocialDestinatario,
         string municipioDestinatario,
         string CPFCNPJDest,
         DateTime dataPagamento,
         string chave,
         string cdCampoExtra
     )
        {
            StringBuilder xml = new StringBuilder();

            xml.AppendLine("       <TLote_GNRE xmlns=\"http://www.gnre.pe.gov.br\">");
            xml.AppendLine("        <guias>");
            xml.AppendLine("          <TDadosGNRE>");
            xml.AppendLine(string.Format("            <c01_UfFavorecida>{0}</c01_UfFavorecida>", UFfavorecida));
            xml.AppendLine(string.Format("            <c02_receita>{0}</c02_receita>", CodigoReceita));
            if (UFfavorecida == "MT")
            {
                xml.AppendLine("            <c25_detalhamentoReceita>000055</c25_detalhamentoReceita>");
            }

            if (UFfavorecida == "AC")
            {
                xml.AppendLine("            <c25_detalhamentoReceita>000005</c25_detalhamentoReceita>");
            }

            xml.AppendLine(string.Format("            <c26_produto>{0}</c26_produto>", produto));
            xml.AppendLine("            <c27_tipoIdentificacaoEmitente>1</c27_tipoIdentificacaoEmitente>");
            xml.AppendLine("            <c03_idContribuinteEmitente>");

            xml.AppendLine((CPFCNPJEmit.Length < 14 ? "<CPF>" + CPFCNPJEmit + "</CPF>" : "<CNPJ>" + CPFCNPJEmit + "</CNPJ>"));

            xml.AppendLine("            </c03_idContribuinteEmitente>");
            xml.AppendLine(string.Format("            <c28_tipoDocOrigem>{0}</c28_tipoDocOrigem>", tipoDocOrigem));
            xml.AppendLine(string.Format("            <c04_docOrigem>{0}</c04_docOrigem>", docOrigem));
            xml.AppendLine(string.Format("            <c06_valorPrincipal>{0}</c06_valorPrincipal>", Util.frmNumero2(valorPrincipal)));
            xml.AppendLine(string.Format("            <c10_valorTotal>{0}</c10_valorTotal>", Util.frmNumero2(valorTotal)));

            xml.AppendLine(string.Format("            <c14_dataVencimento>{0}</c14_dataVencimento>", DateTime.Now.ToString("yyyy-MM-dd")));//2015-12-31
            if (UFfavorecida == "RN")
            {

                xml.AppendLine("            <c15_convenio>ICMS 93/2015</c15_convenio>");
            }
            //xml.AppendLine(string.Format("            <c15_convenio>{0}</c15_convenio>", convenio));//SP - PROT.101/07 - 10/08 - 13/
            xml.AppendLine(string.Format("            <c16_razaoSocialEmitente>{0}</c16_razaoSocialEmitente>", razaoSocialEmitente));
            xml.AppendLine(string.Format("            <c18_enderecoEmitente>{0}</c18_enderecoEmitente>", enderecoEmitente));
            xml.AppendLine(string.Format("            <c19_municipioEmitente>{0}</c19_municipioEmitente>", municipioEmitente));
            xml.AppendLine(string.Format("            <c20_ufEnderecoEmitente>{0}</c20_ufEnderecoEmitente>", ufEnderecoEmitente));
            xml.AppendLine(string.Format("            <c21_cepEmitente>{0}</c21_cepEmitente>", cepEmitente));
            if (!string.IsNullOrEmpty(telefoneEmitente))
            {
                xml.AppendLine(string.Format("            <c22_telefoneEmitente>{0}</c22_telefoneEmitente>", telefoneEmitente));
            }

            if (UFfavorecida != "BA" && UFfavorecida != "MT" && UFfavorecida != "PR" && UFfavorecida != "SC")
            {
                //if (UFfavorecida == "GO" || UFfavorecida == "AM" || UFfavorecida == "RS" || UFfavorecida == "RR" || UFfavorecida == "AC" || UFfavorecida == "SE" || UFfavorecida == "PI" || UFfavorecida == "RN" || UFfavorecida == "AL")
                //{
                xml.AppendLine("            <c34_tipoIdentificacaoDestinatario>" + (CPFCNPJDest.Length < 14 ? "2" : "1") + "</c34_tipoIdentificacaoDestinatario>");
                //}
                //else
                //{
                //    xml.AppendLine("            <c34_tipoIdentificacaoDestinatario>" + (CPFCNPJDest.Length < 14 ? "1" : "2") + "</c34_tipoIdentificacaoDestinatario>");
                //}

                xml.AppendLine("            <c35_idContribuinteDestinatario>");

                xml.AppendLine((CPFCNPJDest.Length < 14 ? "<CPF>" + CPFCNPJDest + "</CPF>" : "<CNPJ>" + CPFCNPJDest + "</CNPJ>"));

                xml.AppendLine("            </c35_idContribuinteDestinatario>");
                xml.AppendLine(string.Format("            <c37_razaoSocialDestinatario>{0}</c37_razaoSocialDestinatario>", razaoSocialDestinatario));
                xml.AppendLine(string.Format("            <c38_municipioDestinatario>{0}</c38_municipioDestinatario>", municipioDestinatario));
            }


            //if (dataPagamento < DateTime.Now)
            //{
            //    dataPagamento = DateTime.Now; 
            //}
            xml.AppendLine(string.Format("            <c33_dataPagamento>{0}</c33_dataPagamento>", DateTime.Now.ToString("yyyy-MM-dd")));
            xml.AppendLine("            <c05_referencia>");
            xml.AppendLine("              <periodo>0</periodo>");
            xml.AppendLine("              <mes>" + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) + "</mes>");
            xml.AppendLine("              <ano>" + DateTime.Now.Year.ToString() + "</ano>");
            xml.AppendLine("            </c05_referencia>");

            if (!string.IsNullOrEmpty(cdCampoExtra))
            {
                xml.AppendLine("            <c39_camposExtras>");
                xml.AppendLine("			<campoExtra>");
                xml.AppendLine(string.Format("			<codigo>{0}</codigo>", cdCampoExtra));
                xml.AppendLine("			<tipo>T</tipo>");
                string valorcampoextra = string.Empty;

                if (UFfavorecida == "AL")
                {
                    valorcampoextra = docOrigem;
                }
                else
                {
                    valorcampoextra = chave;
                }
                xml.AppendLine(string.Format("			<valor>{0}</valor>", valorcampoextra));
                xml.AppendLine("			</campoExtra>");
                xml.AppendLine("		  </c39_camposExtras>");
            }

            xml.AppendLine("		  </TDadosGNRE>");
            xml.AppendLine("        </guias>");
            xml.AppendLine("      </TLote_GNRE>");

            return xml.ToString();
        }


        public string MontaSoap(string SoapBody, string actionurl, bool addgnreDados)
        {

            StringBuilder soap = new StringBuilder();

            soap.AppendLine(string.Format("<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:gnr=\"http://www.gnre.pe.gov.br/webservice/{0}\">", actionurl));
            soap.AppendLine("  <soap:Header>");
            soap.AppendLine("     <gnr:gnreCabecMsg>");
            soap.AppendLine("        <gnr:versaoDados>1.00</gnr:versaoDados>");
            soap.AppendLine("     </gnr:gnreCabecMsg>");
            soap.AppendLine("  </soap:Header>");
            soap.AppendLine("  <soap:Body>");
            if (addgnreDados)
            {
                soap.AppendLine("     <gnr:gnreDadosMsg>");
            }


            soap.AppendLine(SoapBody);
            if (addgnreDados)
            {
                soap.AppendLine("      </gnr:gnreDadosMsg>");
            }
            soap.AppendLine("   </soap:Body>");
            soap.AppendLine("</soap:Envelope>");


            return soap.ToString();

        }


        public string EnviaLoteGNRE(
            string UFfavorecida,
            string CodigoReceita,
            string produto,
            string tipoIdentificacaoEmitente,
            string CPFCNPJEmit,
            string tipoDocOrigem,
            string docOrigem,
            string valorPrincipal,
            string valorTotal,
            DateTime dataVencimento,
            string convenio,
            string razaoSocialEmitente,
            string enderecoEmitente,
            string municipioEmitente,
            string ufEnderecoEmitente,
            string cepEmitente,
            string telefoneEmitente,
            string razaoSocialDestinatario,
            string municipioDestinatario,
            string CPFCNPJDest,
            DateTime dataPagamento,
            string chave,
            string certificadoSubject,
            string certificadoThumbPrint,
            string urlUF,
            string cdambienteNF,
            int cdnotafiscalsaida,
            int tipoGnre,
            string CdCampoExtra,
            string pastalogcliente,
            ObjEmail _objemailadm,
            string datasourceDoce,
            string schemaDoce,
            string cdentifilial)
        {
            //var _url = "https://www.testegnre.pe.gov.br/gnreWS/services/GnreLoteRecepcao";
            //var _url = "https://www.gnre.pe.gov.br/gnreWS/services/GnreConfigUF";
            //var _action = "https://www.gnre.pe.gov.br/gnreWS/services/GnreConfigUF/processar";

            var _actionProcessar = string.Empty;
            var _actionConsultar = string.Empty;

            if (UFfavorecida == "SP" || UFfavorecida == "RJ" || UFfavorecida == "ES")
            {
                _actionProcessar = urlUF + "/processar???";
                _actionConsultar = urlUF + "/consultar???";
                throw new Exception("UF favorecida nao tem ws de integracao");
            }
            else
            {
                _actionProcessar = urlUF + "/GnreLoteRecepcao/processar";
                _actionConsultar = urlUF + "/GnreResultadoLote/consultar";
            }


            string loteGNRE = this.MontaXmlLoteGnre(
             UFfavorecida,
             CodigoReceita,
             produto,
             tipoIdentificacaoEmitente,
             CPFCNPJEmit,
             tipoDocOrigem,
             docOrigem,
             valorPrincipal,
             valorTotal,
             dataVencimento,
             convenio,
             razaoSocialEmitente,
             enderecoEmitente,
             municipioEmitente,
             ufEnderecoEmitente,
             cepEmitente,
             telefoneEmitente,
             razaoSocialDestinatario,
             municipioDestinatario,
             CPFCNPJDest,
             dataPagamento,
             chave, CdCampoExtra);

            Log.For(this, PastaLogCliente).Info("LOTE GNRE : " + loteGNRE);

            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(this.MontaSoap(loteGNRE, "GnreLoteRecepcao", true));

            string soapResult = string.Empty;


            soapResult = this.CallWebService(soapEnvelopeXml, urlUF + "/" + "GnreLoteRecepcao", _actionProcessar, certificadoSubject, certificadoThumbPrint);


            XmlDocument xmlSoapResult = new XmlDocument();
            xmlSoapResult.LoadXml(soapResult);
            string RetonoLote = PegarDadosRetorno(soapResult);
            string[] retlotearray = RetonoLote.Split('|');
            string codigo = retlotearray[0];
            string descricao = retlotearray[1];
            string numeroProtocolo = retlotearray[2];
            string dthora = retlotearray[3];

            /*
                 Código Mensagem de validação
                    400 Lote Recebido. Aguardando processamento
                    401 Lote em Processamento
                    402 Lote processado com sucesso
                    403 Processado com pendência
                    404 Erro no processamento do lote. Enviar o lote novamente.
                 */

            if (codigo == "100")//buscar o retorno com o protocolo
            {
                Log.For(this, PastaLogCliente).Info("PROTOCOLO GNRE: " + numeroProtocolo + "DTHORA: " + dthora);
                //grava o protocolo no banco
                nhp.NotafiscalAtualizarGNREStatusProt(cdnotafiscalsaida, numeroProtocolo, datasource, schema, tipoGnre);

                int tentativas = 3;

                bool Erro404 = false;

                string resultConsula = string.Empty;

                CodigoDescricao coddesRetorno = new CodigoDescricao();

                ArquivoGNRE ar;

                string txtRetorno = string.Empty;

                for (int i = 0; i < tentativas; i++)
                {
                    resultConsula = ConsultaLoteGNRE(urlUF + "/" + "GnreResultadoLote", _actionConsultar, certificadoSubject, certificadoThumbPrint, cdambienteNF, numeroProtocolo);

                    coddesRetorno = new CodigoDescricao();

                    txtRetorno = PegarResultadoConsulta(resultConsula, out coddesRetorno);

                    if (coddesRetorno.Codigo == 402 || coddesRetorno.Codigo == 403)
                    {
                        i = 4;

                    }
                    else if (coddesRetorno.Codigo == 404)
                    {
                        Erro404 = true;
                        i = 4;
                    }
                }

                if (!Erro404)//VOLTAR O STATUS PARA 0 NO ELSE TENTAR ENVIAR NOVAMENTE
                {
                    ar = new ArquivoGNRE(txtRetorno);

                    switch (coddesRetorno.Codigo)
                    {
                        case 402:  //Lote Processado com sucesso
                            {
                                //update dos campos da gnre update o status pra 100 autorizado
                                nhp.NotaFiscalAtualizarGNRE(cdnotafiscalsaida,
                                    ar.headerRetornoGNRE.NumeroProtocolo,
                                    ar.detalheRetornoGNRE[0].DtVencimento,
                                    RJSOptimusConverter.ToDouble(ar.detalheRetornoGNRE[0].Juros, 0),
                                    RJSOptimusConverter.ToDouble(ar.detalheRetornoGNRE[0].Multa, 0),
                                    RJSOptimusConverter.ToDouble(ar.detalheRetornoGNRE[0].ValorPrincipal, 0),
                                    ar.detalheRetornoGNRE[0].CodigoBarras,
                                    100,
                                    ar.detalheRetornoGNRE[0].MesAnoReferencia,
                                    ar.detalheRetornoGNRE[0].RepresNumerica,
                                    ar.detalheRetornoGNRE[0].InfoComplementares,
                                    CodigoReceita, datasource, schema, tipoGnre,
                                    RJSOptimusConverter.ToDouble(ar.detalheRetornoGNRE[0].AtualizacaoMonetaria, 0),
                                    ar.detalheRetornoGNRE[0].DtLimitePagamento);

                                string nmstatus = tipoGnre == 1 ? "Lote GNRE Processado com sucesso" : "Lote GNRE FCP Processado com sucesso";

                                nhp.InserirHistoricoItem("", "0", "402", nmstatus, cdnotafiscalsaida.ToString(), datasourceDoce, schemaDoce);

                                break;
                            }
                        case 403:
                            {

                                string erros = string.Empty;

                                //gravar os erros e atualizar o status para 9
                                string nmstatus = tipoGnre == 1 ? "" : "FCP-";
                                if (ar.rejeicaoRetornoGNRE.Count > 0)
                                {
                                    foreach (var item in ar.rejeicaoRetornoGNRE)
                                    {
                                        erros += item.CodigoRejeicao.Trim() + "-" + item.DescricaoRejeicao.Trim();

                                        nhp.InserirHistoricoItem("", "0", item.CodigoRejeicao, "GNRE-" + nmstatus + item.DescricaoRejeicao.Trim(), cdnotafiscalsaida.ToString(), datasourceDoce, schemaDoce);
                                    }

                                    if (erros.Length > 300)
                                    {
                                        erros = erros.Substring(0, 300);
                                    }

                                    nhp.NotaFiscalAtualizarGNREErro(cdnotafiscalsaida, erros, datasource, schema, tipoGnre);
                                    //gravar no log
                                }
                                else
                                {
                                    //nao sei se vai car aqui
                                    Log.For(this, PastaLogCliente).Info("STATUS 403 SEM AS REJEICOES");
                                }

                                break;
                            }
                        default:
                            {
                                string nmstatus = tipoGnre == 1 ? "" : "FCP-";
                                nhp.InserirHistoricoItem("", "0", coddesRetorno.Codigo.ToString(), "GNRE-" + nmstatus + coddesRetorno.Descricao.Trim(), cdnotafiscalsaida.ToString(), datasourceDoce, schemaDoce);
                            }
                            break;
                    }
                }
                else
                {
                    //VOLTA O STATUS PARA 0
                    nhp.NotafiscalAtualizarGNREStatus(cdnotafiscalsaida, 0, datasource, schema, tipoGnre);
                    Log.For(this, PastaLogCliente).Info("VOLTOU O STATUSA PARA 0 ERRO 404: " + numeroProtocolo + "DTHORA: " + dthora);
                }

            }
            else
            {
                nhp.NotaFiscalAtualizarGNREErro(cdnotafiscalsaida, descricao, datasource, schema, tipoGnre);
                string nmstatus = tipoGnre == 1 ? "" : "FCP-";
                nhp.InserirHistoricoItem("", numeroProtocolo, codigo, "GNRE-" + nmstatus + descricao.Trim(), cdnotafiscalsaida.ToString(), datasourceDoce, schemaDoce);

                //gravar no log e enviar e-mail
                _objemailadm.SUBJECT = "[ENVIOD DO LOTE] ERRO: " + cdentifilial.ToString() + " SCHEMA:" + schema;
                _objemailadm.BODY = "CODIGO: " + codigo + " DESCRICAO: " + descricao;
                Util.EnviaEmail(_objemailadm, "opterro@optimuserp.com.br");

            }

            return loteGNRE;

        }

        public string ConsultaLoteEnviado(
            string UFfavorecida,
            string certificadoSubject,
            string certificadoThumbPrint,
            string urlUF,
            string cdambienteNF,
            int cdnotafiscalsaida,
            int tipoGnre,
            string pastalogcliente,
            ObjEmail _objemailadm,
             string _datasource,
            string _schema,
            string _datasourceDoce,
            string _schemaDoce,
            string cdentifilial,
            string numeroProtocolo)
        {
            Log.For(this, PastaLogCliente).Info("CONSULTA PROTOCOLO GNRE: " + numeroProtocolo + "CDFILIAL:" + cdentifilial);


            var _actionProcessar = string.Empty;
            var _actionConsultar = string.Empty;

            if (UFfavorecida == "SP" || UFfavorecida == "RJ" || UFfavorecida == "ES")
            {
                _actionProcessar = urlUF + "/processar???";
                _actionConsultar = urlUF + "/consultar???";
                throw new Exception("UF favorecida nao tem ws de integracao");
            }
            else
            {
                _actionProcessar = urlUF + "/GnreLoteRecepcao/processar";
                _actionConsultar = urlUF + "/GnreResultadoLote/consultar";
            }

            int tentativas = 3;

            bool Erro404 = false;

            string resultConsula = string.Empty;

            CodigoDescricao coddesRetorno = new CodigoDescricao();

            ArquivoGNRE ar;

            string txtRetorno = string.Empty;

            for (int i = 0; i < tentativas; i++)
            {
                resultConsula = ConsultaLoteGNRE(urlUF + "/" + "GnreResultadoLote", _actionConsultar, certificadoSubject, certificadoThumbPrint, cdambienteNF, numeroProtocolo);

                coddesRetorno = new CodigoDescricao();

                txtRetorno = PegarResultadoConsulta(resultConsula, out coddesRetorno);

                if (coddesRetorno.Codigo == 402 || coddesRetorno.Codigo == 403)
                {
                    i = 4;

                }
                else if (coddesRetorno.Codigo == 404)
                {
                    Erro404 = true;
                    i = 4;
                }
            }

            if (!Erro404)
            {
                ar = new ArquivoGNRE(txtRetorno);

                switch (coddesRetorno.Codigo)
                {
                    case 402:  //Lote Processado com sucesso
                        {
                            //update dos campos da gnre update o status pra 100 autorizado
                            nhp.NotaFiscalAtualizarGNRE(cdnotafiscalsaida,
                                ar.headerRetornoGNRE.NumeroProtocolo,
                                ar.detalheRetornoGNRE[0].DtVencimento,
                                RJSOptimusConverter.ToDouble(ar.detalheRetornoGNRE[0].Juros, 0),
                                RJSOptimusConverter.ToDouble(ar.detalheRetornoGNRE[0].Multa, 0),
                                RJSOptimusConverter.ToDouble(ar.detalheRetornoGNRE[0].ValorPrincipal, 0),
                                ar.detalheRetornoGNRE[0].CodigoBarras,
                                100,
                                ar.detalheRetornoGNRE[0].MesAnoReferencia,
                                ar.detalheRetornoGNRE[0].RepresNumerica,
                                ar.detalheRetornoGNRE[0].InfoComplementares,
                                infoUF.cdReceitaGNRE, _datasource, _schema, tipoGnre,
                                RJSOptimusConverter.ToDouble(ar.detalheRetornoGNRE[0].AtualizacaoMonetaria, 0),
                                ar.detalheRetornoGNRE[0].DtLimitePagamento);

                            string nmstatus = tipoGnre == 1 ? "Lote GNRE Processado com sucesso" : "Lote GNRE FCP Processado com sucesso";

                            nhp.InserirHistoricoItem("", "0", "402", nmstatus, cdnotafiscalsaida.ToString(), _datasourceDoce, _schemaDoce);

                            break;
                        }
                    case 403:
                        {

                            string erros = string.Empty;

                            //gravar os erros e atualizar o status para 9
                            string nmstatus = tipoGnre == 1 ? "" : "FCP-";
                            if (ar.rejeicaoRetornoGNRE.Count > 0)
                            {
                                foreach (var item in ar.rejeicaoRetornoGNRE)
                                {
                                    erros += item.CodigoRejeicao.Trim() + "-" + item.DescricaoRejeicao.Trim();

                                    nhp.InserirHistoricoItem("", "0", item.CodigoRejeicao, "GNRE-" + nmstatus + item.DescricaoRejeicao.Trim(), cdnotafiscalsaida.ToString(), _datasourceDoce, _schemaDoce);
                                }

                                if (erros.Length > 300)
                                {
                                    erros = erros.Substring(0, 300);
                                }

                                nhp.NotaFiscalAtualizarGNREErro(cdnotafiscalsaida, erros, datasource, schema, tipoGnre);
                                //gravar no log
                            }
                            else
                            {
                                //nao sei se vai car aqui
                                Log.For(this, PastaLogCliente).Info("STATUS 403 SEM AS REJEICOES");
                            }

                            break;
                        }
                    default:
                        {
                            string nmstatus = tipoGnre == 1 ? "" : "FCP-";
                            nhp.InserirHistoricoItem("", "0", coddesRetorno.Codigo.ToString(), "GNRE-" + nmstatus + coddesRetorno.Descricao.Trim(), cdnotafiscalsaida.ToString(), _datasourceDoce, _schemaDoce);
                        }
                        break;
                }
            }
            else
            {
                nhp.NotafiscalAtualizarGNREStatus(cdnotafiscalsaida, 0, datasource, schema, tipoGnre);
                Log.For(this, PastaLogCliente).Info("VOLTOU O STATUSA PARA 0 ERRO 404: " + numeroProtocolo);
            }

            return txtRetorno;

        }

        public string ConsultaLoteGNRE(string url, string action, string certificadoSubject, string certificadoThumbPrint, string ambiente, string numeroprotocolo)
        {
            //var _url = "https://www.testegnre.pe.gov.br/gnreWS/services/GnreResultadoLote";
            //var _action = "https://www.testegnre.pe.gov.br/gnreWS/services/GnreResultadoLote/consultar";
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(this.MontaSoap(MontaXmlConsultaLote(ambiente, numeroprotocolo), "GnreResultadoLote", false));
            string soapResult = this.CallWebService(soapEnvelopeXml, url, action, certificadoSubject, certificadoThumbPrint);
            return soapResult;
        }

        public string CallWebService(XmlDocument soapEnvelopeXml, string _url, string _action, string certificadoSubject, string certificadoThumbPrint)
        {
            HttpWebRequest webRequest = CreateWebRequest(_url, _action, certificadoSubject, certificadoThumbPrint);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            asyncResult.AsyncWaitHandle.WaitOne();

            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }

            }

            return soapResult;
        }

        private HttpWebRequest CreateWebRequest(string url, string action, string certificadoSubject, string certificadoThumbPrint)
        {
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            string aaa = string.Empty;
            try
            {

                webRequest.Headers.Add("SOAPAction", action);
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";

                X509Certificate2 cert = BuscaConfiguracaoCertificado(certificadoThumbPrint, certificadoSubject);
                aaa = cert.GetSerialNumberString();

                webRequest.ClientCertificates.Add(cert);
            }
            catch (Exception ex)
            {

                throw new Exception(aaa + " -----------------------" + ex.ToString());
            }
            return webRequest;
        }

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        public X509Certificate2 BuscaConfiguracaoCertificado(string CertificadoDigitalThumbPrint, string CertificadoSubject)
        {
            X509Certificate2 x509Cert = null;


            X509Store store = new X509Store("MY", StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2Collection collection1 = null;
            if (!string.IsNullOrEmpty(CertificadoDigitalThumbPrint))
                collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindByThumbprint, CertificadoDigitalThumbPrint, false);
            else
                collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectDistinguishedName, CertificadoSubject, false);


            if (collection1 == null && !string.IsNullOrEmpty(CertificadoSubject))
            {
                collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectDistinguishedName, CertificadoSubject, false);
            }

            for (int i = 0; i < collection1.Count; i++)
            {
                //Verificar a validade do certificado
                if (DateTime.Compare(DateTime.Now, collection1[i].NotAfter) == -1)
                {
                    x509Cert = collection1[i];
                    break;
                }
            }

            //Se não encontrou nenhum certificado com validade correta, vou pegar o primeiro certificado, porem vai travar na hora de tentar enviar a nota fiscal, por conta da validade. Wandrey 06/04/2011
            if (x509Cert == null && collection1.Count > 0)
                x509Cert = collection1[0];

            return x509Cert;
        }


        public string PegarDadosRetorno(string strXml)
        {
            string ret = string.Empty;
            string codigo = string.Empty;
            string descricao = string.Empty;
            string numero = string.Empty;
            string dthora = string.Empty;

            string strxmlAlt = strXml.Replace("ns1:", "");

            MemoryStream memoryStream = Util.StringXmlToStream(strxmlAlt);
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(memoryStream);



                XmlNodeList retsituacaoRecepcaoList = null;
                retsituacaoRecepcaoList = xml.GetElementsByTagName("situacaoRecepcao");

                foreach (XmlNode sitrec in retsituacaoRecepcaoList)
                {
                    XmlElement infsitrec = (XmlElement)sitrec;
                    codigo = infsitrec.GetElementsByTagName("codigo")[0].InnerText;
                    descricao = infsitrec.GetElementsByTagName("descricao")[0].InnerText;
                }

                XmlNodeList retreciboList = null;
                retreciboList = xml.GetElementsByTagName("recibo");

                foreach (XmlNode sitrecibo in retreciboList)
                {
                    XmlElement infsitrecibo = (XmlElement)sitrecibo;
                    numero = infsitrecibo.GetElementsByTagName("numero")[0].InnerText;
                    dthora = infsitrecibo.GetElementsByTagName("dataHoraRecibo")[0].InnerText;
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            ret = codigo + "|" + descricao + "|" + numero + "|" + dthora;

            return ret;

        }

        public struct CodigoDescricao
        {
            public int Codigo;
            public string Descricao;
        }

        public string PegarResultadoConsulta(string strXmlSoap, out CodigoDescricao coddesRetorno)
        {
            string strxmlAlt = strXmlSoap.Replace("ns1:", "");
            string txtRetorno = string.Empty;
            coddesRetorno = new CodigoDescricao();
            MemoryStream memoryStream = Util.StringXmlToStream(strxmlAlt);
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(memoryStream);

                string codigo = string.Empty;
                string descricao = string.Empty;
                XmlNodeList retsituacaoprocessoList = null;
                retsituacaoprocessoList = xml.GetElementsByTagName("situacaoProcess");

                foreach (XmlNode sitcon in retsituacaoprocessoList)
                {
                    XmlElement infsitrec = (XmlElement)sitcon;
                    txtRetorno = infsitrec.InnerText;

                    codigo = infsitrec.GetElementsByTagName("codigo")[0].InnerText;
                    descricao = infsitrec.GetElementsByTagName("descricao")[0].InnerText;
                    coddesRetorno.Codigo = Convert.ToInt32(codigo);
                    coddesRetorno.Descricao = descricao;
                }



                XmlNodeList retsituacaoConsultaList = null;
                retsituacaoConsultaList = xml.GetElementsByTagName("resultado");

                foreach (XmlNode sitcon in retsituacaoConsultaList)
                {
                    XmlElement infsitrec = (XmlElement)sitcon;
                    txtRetorno = infsitrec.InnerText;

                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return txtRetorno;
        }

        #endregion




    }

    #region Classes do retorno

    public class HeaderRetornoGNRE
    {
        /*
    Identificador de cabeçalho (valor 0) N 1 1 1
    Tipo de Identificador do solicitante (1-CPF/2-CNPJ) N 2 2 1
    Identificador do solicitante N 3 16 14
    Número do Protocolo do lote N 17 26 10
    Ambiente (5) N 27 27 1
         */

        public HeaderRetornoGNRE(string plinha)
        {
            linha = plinha;
        }

        private string linha { get; set; }

        public string Identificador { get; set; }
        public string TipoIdentificador { get; set; }
        public string IdentificadorSolicitante { get; set; }
        public string NumeroProtocolo { get; set; }
        public string Ambiente { get; set; }


        public void LerLinhaHeader()
        {
            this.Identificador = linha.Substring(0, 1);
            this.TipoIdentificador = linha.Substring(1, 1);
            this.IdentificadorSolicitante = linha.Substring(2, 14);
            this.NumeroProtocolo = linha.Substring(16, 10);
            this.Ambiente = linha.Substring(26, 1);
        }
    }

    public class DetalheRetornoGNRE
    {
        public DetalheRetornoGNRE(string plinha)
        {
            linha = plinha;
        }
        private string linha { get; set; }

        public string Identificador { get; set; }
        public string SequencialGuia { get; set; }//2 5 4

        /// <summary>
        /// 0 - Processada com sucesso
        /// 1 - Invalidada pelo Portal
        /// 2 - Invalidada pela UF
        /// 3 - Erro de comunicação
        /// </summary>
        public string SituacaoGuia { get; set; }
        public string Convenio { get; set; } //563 592 30
        public string InfoComplementares { get; set; }//593 892 300
        public DateTime DtVencimento { get; set; }//Data de vencimento (DDMMAAAA) 893 900 8
        public DateTime DtLimitePagamento { get; set; }//901 908 8
        /// <summary>
        /// Período de Referência:
        /// 0 - Mensal
        /// 1 - 1a Quinzena
        /// 2 - 2a Quinzena
        /// 3 - 1o Decêndio
        /// 4 - 2o Decêndio
        /// 5 - 3o Decêndio
        /// </summary>
        public string PeriodoReferencia { get; set; }//909 909 1
        public string MesAnoReferencia { get; set; }//910 915 6
        public string Parcela { get; set; }//916 918 3
        public string ValorPrincipal { get; set; }//919 933 15
        public string AtualizacaoMonetaria { get; set; }//934 948 15
        public string Juros { get; set; } //949 963 15
        public string Multa { get; set; }//964 978 15
        public string RepresNumerica { get; set; }//979 1026 48
        public string CodigoBarras { get; set; }//1027 1070 44
        public string NumeroControle { get; set; }//1072 1087 16
        public void LerLinhaDetalhe()
        {
            this.Identificador = linha.Substring(0, 1).Trim();
            this.SequencialGuia = linha.Substring(1, 4).Trim();
            this.SituacaoGuia = linha.Substring(5, 1).Trim();
            this.Convenio = linha.Substring(562, 30).Trim();
            this.InfoComplementares = linha.Substring(592, 300).Trim();

            string devenci = linha.Substring(892, 8).Trim();
            if (!string.IsNullOrEmpty(devenci) && devenci != "00000000")
            {
                this.DtVencimento = new DateTime(Convert.ToInt32(devenci.Substring(4, 4)), Convert.ToInt32(devenci.Substring(2, 2)), Convert.ToInt32(devenci.Substring(0, 2)));
            }
            //this.DtVencimento = linha.Substring(892, 8).Trim();

            string dtlim = linha.Substring(900, 8).Trim();

            if (!string.IsNullOrEmpty(dtlim) && dtlim != "00000000")
            {
                this.DtLimitePagamento = new DateTime(Convert.ToInt32(dtlim.Substring(4, 4)), Convert.ToInt32(dtlim.Substring(2, 2)), Convert.ToInt32(dtlim.Substring(0, 2)));
            }

            this.PeriodoReferencia = linha.Substring(908, 1).Trim();
            this.MesAnoReferencia = linha.Substring(909, 6).Trim();
            this.Parcela = linha.Substring(915, 3).Trim();
            this.ValorPrincipal = Util.ValorGNRE(linha.Substring(918, 15));
            this.AtualizacaoMonetaria = Util.ValorGNRE(linha.Substring(933, 15));
            this.Juros = Util.ValorGNRE(linha.Substring(948, 15));
            this.Multa = Util.ValorGNRE(linha.Substring(963, 15));
            this.RepresNumerica = linha.Substring(978, 48).Trim();
            this.CodigoBarras = linha.Substring(1026, 44).Trim();
            this.NumeroControle = linha.Substring(1071, 16).Trim();

        }
    }

    public class RejeicaoRetornoGNRE
    {
        public RejeicaoRetornoGNRE(string plinha)
        {
            linha = plinha;
        }

        private string linha { get; set; }

        public string Identificador { get; set; }
        public string Sequencial { get; set; }
        public string NomeCampo { get; set; }
        public string CodigoRejeicao { get; set; }
        public string DescricaoRejeicao { get; set; }


        public void LerLinhaRejeicao()
        {
            this.Identificador = linha.Substring(0, 1);
            this.Sequencial = linha.Substring(1, 4);
            this.NomeCampo = linha.Substring(5, 30);
            this.CodigoRejeicao = linha.Substring(35, 3);
            this.DescricaoRejeicao = linha.Substring(38, 355);
        }
    }


    public class ArquivoGNRE
    {
        public ArquivoGNRE(string ArqRetorno)
        {
            this.rejeicaoRetornoGNRE = new List<RejeicaoRetornoGNRE>();
            this.headerRetornoGNRE = new HeaderRetornoGNRE(string.Empty);
            this.detalheRetornoGNRE = new List<DetalheRetornoGNRE>();

            using (StringReader reader = new StringReader(ArqRetorno))
            {
                string linhaArq;
                while ((linhaArq = reader.ReadLine()) != null)
                {
                    string Ident = linhaArq.Substring(0, 1);

                    switch (Ident)
                    {
                        case "0":
                            {
                                HeaderRetornoGNRE hr = new HeaderRetornoGNRE(linhaArq);
                                hr.LerLinhaHeader();
                                this.headerRetornoGNRE = hr;
                                break;
                            }
                        case "1":
                            {
                                DetalheRetornoGNRE dr = new DetalheRetornoGNRE(linhaArq);
                                dr.LerLinhaDetalhe();
                                this.detalheRetornoGNRE.Add(dr);
                                break;
                            }

                        case "2":
                            {
                                RejeicaoRetornoGNRE rr = new RejeicaoRetornoGNRE(linhaArq);
                                rr.LerLinhaRejeicao();
                                this.rejeicaoRetornoGNRE.Add(rr);
                                break;
                            }

                        case "9":
                            {

                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
            }


            foreach (string linhaArq in ArqRetorno.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {


            }
        }
        public HeaderRetornoGNRE headerRetornoGNRE { get; set; }
        public List<DetalheRetornoGNRE> detalheRetornoGNRE { get; set; }
        public List<RejeicaoRetornoGNRE> rejeicaoRetornoGNRE { get; set; }
    }

    #endregion
}
