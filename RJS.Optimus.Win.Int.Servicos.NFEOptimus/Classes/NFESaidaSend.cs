using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.IO;
using System.Xml;
using RJS.Optimus.Biblioteca;
using System.Web.Caching;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class NFESaidaSend
    {
        string PastaLogCliente = string.Empty;
        string emailEmpresa = string.Empty;
        string smtp = string.Empty;
        string porta = string.Empty;
        string usuario = string.Empty;
        string senha = string.Empty;
        string cliente = string.Empty;
        //string cnpjcliente = string.Empty;
        bool EnableSSL = false;
        long cnpjEmpresa = 0;
        string multiplasfiliais = string.Empty;
        string pastaSchema = string.Empty;
        string versaoxml = string.Empty;
        int cdentifilial = 0;
        string datasource = string.Empty;
        string schema = string.Empty;
        string datasourceDoce = string.Empty;
        string schemaDoce = string.Empty;
        string DiretorioEnviar = string.Empty;
        ObjEmail objemailadm = null;
        public NFESaidaSend(
            string _cdentifilial,
            string _emailEmpresa,
            string _smtp,
            string _porta,
            string _usuario,
            string _senha,
            string _EnableSSL,
            string _cliente,
            string _cnpjCliente,
            string _multiplasfiliais,
            string _pastaSchema,
            string _versaoxml,
            string _DiretorioEnviar,
            string _datasource,
            string _schema,
            string _datasourceDoce,
            string _schemaDoce,
            string _pastaLogWs,
            ObjEmail _objemailadm)
        {
            emailEmpresa = _emailEmpresa;
            smtp = _smtp;
            porta = _porta;
            usuario = _usuario;
            senha = _senha;
            cliente = _cliente;
            //cnpjcliente = _cnpjcliente;
            EnableSSL = _EnableSSL == "1";
            cnpjEmpresa = Convert.ToInt64(_cnpjCliente);
            multiplasfiliais = _multiplasfiliais == "1" ? "TRUE" : "FALSE";
            pastaSchema = _pastaSchema;
            versaoxml = _versaoxml;
            cdentifilial = Convert.ToInt32(_cdentifilial);
            datasource = _datasource;
            schema = _schema;
            datasourceDoce = _datasourceDoce;
            schemaDoce = _schemaDoce;
            DiretorioEnviar = _DiretorioEnviar;
            PastaLogCliente = string.Format(@"{0}\{1}_{2}_{3}", _pastaLogWs, schema, cdentifilial, _cnpjCliente);
            objemailadm = _objemailadm;
        }
        //public void teste()
        //{
        //    try
        //    {
        //        Log.For(this, PastaLogCliente).Error("teste");
        //        Log.For(this, PastaLogCliente).Info("teste");
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}


        public static Cache Cache
        {
            get
            {
                EnsureHttpRuntime();
                return System.Web.HttpRuntime.Cache;
            }
        }
        private static void EnsureHttpRuntime()
        {
            if (null == _httpRuntime)
            {
                try
                {
                    // Monitor.Enter(typeof(AppMain));
                    if (null == _httpRuntime)
                    {
                        // Create an Http Content to give us access to the cache.
                        _httpRuntime = new System.Web.HttpRuntime();
                    }
                }
                finally
                {
                    //Monitor.Exit(typeof(AppMain));
                }
            }
        }

        public NFESaidaSend()
        {
            emailEmpresa = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_USUARIO").ToString();
            smtp = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_SMTP").ToString();
            porta = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_PORTA").ToString();
            usuario = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_USUARIO").ToString();
            senha = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_SENHA").ToString();
            cliente = ConfigurationManager.AppSettings.Get("CLIENTE").ToString();
            //cnpjcliente = ConfigurationManager.AppSettings.Get("CNPJ").ToString();
            EnableSSL = Biblioteca.RJSOptimusConverter.ToBoolean(ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_ENABLESSL"), false);
            cnpjEmpresa = Convert.ToInt64(ConfigurationManager.AppSettings.Get("CNPJ"));
            multiplasfiliais = ConfigurationManager.AppSettings.Get("MULTIPLASFILIAIS");
            pastaSchema = ConfigurationManager.AppSettings.Get("PASTA_SCHEMAS");
            versaoxml = ConfigurationManager.AppSettings.Get("VERSAOXML").ToString();
            cdentifilial = Convert.ToInt32(ConfigurationManager.AppSettings.Get("CDENTIFILIAL"));
            DiretorioEnviar = ConfigurationManager.AppSettings.Get("PASTA_XML_ENVIO");
        }

        /// <summary>
        /// Salva o xml na pasta do  UNINFE para envio para a sefaz.
        /// </summary>
        public void SalvarXMLIntegracaoUNINFE()
        {
            WorkFlowServico _WorkFlowServico = new WorkFlowServico("NFESaidaSend");
            _WorkFlowServico.AddEvento("INÍCIO PROCESSO");
            NFEHelper nhp = new NFEHelper();
            string XML = string.Empty;
            string ChaveNfe = string.Empty;
            int CdNotaFiscalSaida = 0;
            int StatusNfe = 0;
            MemoryStream oMemoryStream;
            XmlDocument docProc;
            string TPEMISSAO = string.Empty;
            DataTable DTNfe = null;
            try
            {
                if (!System.IO.Directory.Exists(DiretorioEnviar))
                    throw new Exception(string.Format("O diretório {0} não existe.", DiretorioEnviar));

                if (multiplasfiliais.Equals("TRUE"))
                {
                    DTNfe = nhp.NotaFiscalIntegracaoFilial(cdentifilial, datasource, schema);
                }
                else
                {
                    DTNfe = nhp.NotaFiscalIntegracao(datasource, schema);
                }


                if (DTNfe.Rows.Count > 0)
                {
                    _WorkFlowServico.AddEvento("QTDE NFS:" + DTNfe.Rows.Count.ToString());

                    foreach (DataRow dtrow in DTNfe.Rows)
                    {
                        XML = string.Empty;
                        ChaveNfe = string.Empty;
                        docProc = new XmlDocument();

                        CdNotaFiscalSaida = Convert.ToInt32(dtrow["CDNOTAFISCALSAIDA"]);

                        StatusNfe = Convert.ToInt32(dtrow["CDNFESTATUS"]);
                        /*
                         Forma de emissão da NF-e
                        1 - Normal;
                        2 - Contingência FS
                        3 - Contingência SCAN
                        4 - Contingência DPEC
                        5 - Contingência FSDA
                        6 - Contingência SVC - AN
                        7 - Contingência SVC - RS
                        9 - Contingência off-line NFC-e
                         */
                        TPEMISSAO = dtrow["CDTIPOEMISSAONFE"].ToString();

                        if (string.IsNullOrEmpty(TPEMISSAO))
                        {
                            throw new Exception("Informe o CDTIPOEMISSAONFE na tabela de filial 1- normal acima de 2 olhar a contingencia do estado");
                        }

                        if (TPEMISSAO != "1")
                        {
                            _WorkFlowServico.AddEvento(string.Format("#CONTINGENCIA #NFS: {0} STATUS:{1} TIPO EMISSAO{2}", CdNotaFiscalSaida, StatusNfe, TPEMISSAO));
                        }

                        _WorkFlowServico.AddEvento(string.Format("NFS: {0} STATUS:{1}", CdNotaFiscalSaida, StatusNfe));

                        /*
                           0  	Aguardando integração da NF-e   
                           1  	NF-e Integrada com                 
                           2  	Erro integração da NF-e            
                           100	Autorizado uso da NF-e pela SEFAZ       
                           200	Aguardando integração de Cancelamento   
                           201	Cancelamento Integrado      
                           202	Erro de Cancelamento Integrado
                           300	Cancelamento autorizado pela SEFAZ      
                           299	Cancelamento NAO autorizado pela SEFAZ  
                           99 	NF-e Não Autorizada pela SEFAZ                                       
                        */

                        string dtEmissao = string.Empty;

                        string NovaChaveDPEC = string.Empty;

                        switch (StatusNfe)
                        {
                            case 0: //ENVIO
                                {
                                    XML = dtrow["TXNDDLAYOUT"].ToString();

                                    _WorkFlowServico.AddEvento("CARREGAR XML DO BANCO");
                                    if (string.IsNullOrEmpty(XML))
                                    {
                                        objemailadm.SUBJECT = "O XML NÃO FOI GERADO CDNOTAFISCALSAIDA: " + CdNotaFiscalSaida.ToString() + " SCHEMA:" + schema;
                                        objemailadm.BODY = "O XML NÃO FOI GERADO CDNOTAFISCALSAIDA: " + CdNotaFiscalSaida.ToString() + " SCHEMA:" + schema;
                                        Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");
                                    }
                                    else
                                    {
                                        //alterar o xml para envio em contingencia
                                        if (TPEMISSAO != "1")
                                        {
                                            XML = Util.AlterarChaveETpemisaoNFE(XML, TPEMISSAO, false, out  NovaChaveDPEC);
                                            _WorkFlowServico.AddEvento("CHAVE CONTINGENCIA: " + NovaChaveDPEC + " CDNOTAFISCALSAIDA:" + CdNotaFiscalSaida.ToString());
                                        }


                                        oMemoryStream = Util.StringXmlToStream(XML);

                                        //Validacao do XML
                                        Util ut = new Util();

                                        string arqSchema = string.Empty;

                                        if (versaoxml == "2")
                                        {
                                            arqSchema = "\\nfe_v2.00.xsd";
                                        }
                                        else if (versaoxml == "3")
                                        {
                                            arqSchema = "\\nfe_v3.10.xsd";
                                        }
                                        else
                                        {
                                            arqSchema = "\\nfe_v4.00.xsd";
                                        }


                                        //Log.For(this, PastaLogCliente).Info("Arquivo xsd :" + pastaSchema + arqSchema);

                                        ut.ValidaXMLNFE(XML.Trim(), pastaSchema + arqSchema);

                                        if (string.IsNullOrEmpty(ut.MsgValidacaoXML))
                                        {
                                            _WorkFlowServico.AddEvento("SALVAR XML NA PASTA");

                                            try
                                            {

                                                //docProc.Load(oMemoryStream);

                                                if (TPEMISSAO == "1")
                                                {
                                                    //docProc.Save(string.Format("{0}\\{1}-nfe.xml", DiretorioEnviar, dtrow["NRNFECHAVE"].ToString()));
                                                    Util.SaveMemoryStreamToFile(string.Format("{0}\\{1}-nfe.xml", DiretorioEnviar, dtrow["NRNFECHAVE"].ToString()), oMemoryStream);
                                                }
                                                else
                                                {
                                                    //docProc.Save(string.Format("{0}\\{1}-nfe.xml", DiretorioEnviar, NovaChaveDPEC));
                                                    Util.SaveMemoryStreamToFile(string.Format("{0}\\{1}-nfe.xml", DiretorioEnviar, NovaChaveDPEC), oMemoryStream);
                                                }

                                                _WorkFlowServico.AddEvento("ATUALIZAR STATUS NA NFS: " + CdNotaFiscalSaida.ToString());

                                                //Atualiza o status da nfe para NF-e Integrada
                                                nhp.NotaFiscalAtualizarStatus(CdNotaFiscalSaida, 1, NovaChaveDPEC, datasource, schema);

                                                _WorkFlowServico.AddEvento("NFS ATUALIZADA : " + CdNotaFiscalSaida.ToString());

                                                //log DE ENVIADO OK
                                                _WorkFlowServico.AddEvento("NFS ENVIADA PARA SEFAZ: " + CdNotaFiscalSaida.ToString());
                                                _WorkFlowServico.AddEvento("NFS CHAVE: " + dtrow["NRNFECHAVE"].ToString());

                                                //LOG DOC-e detalhamento da NFE

                                                dtEmissao = Convert.ToDateTime(dtrow["DTEMISSAO"]).ToString("dd/MM/yyyy");

                                                nhp.InserirHistoricoNFE(cnpjEmpresa.ToString(),
                                                    dtrow["NRNOTA"].ToString(),
                                                    dtrow["CDSERIE"].ToString(),
                                                    CdNotaFiscalSaida.ToString(),
                                                    dtrow["CDENTIFILIAL"].ToString(),
                                                    dtEmissao,
                                                    StatusNfe.ToString(),
                                                    dtrow["TXNDDLAYOUT"].ToString(),
                                                    string.Empty,
                                                    datasourceDoce,
                                                    schemaDoce);
                                            }
                                            catch (Exception exdsd)
                                            {
                                                _WorkFlowServico.AddEvento("ERRO : CDNOTA:" + CdNotaFiscalSaida.ToString() + " CHAVE:" + dtrow["NRNFECHAVE"].ToString() + exdsd.ToString());
                                                nhp.NotaFiscalAtualizarStatus(CdNotaFiscalSaida, 2, NovaChaveDPEC, datasource, schema);

                                                objemailadm.SUBJECT = "[C] Carregar xml ERRO: " + CdNotaFiscalSaida.ToString() + " SCHEMA:" + schema;
                                                objemailadm.BODY = "ERRO : CDNOTA:" + CdNotaFiscalSaida.ToString() + " CHAVE:" + dtrow["NRNFECHAVE"].ToString() + " - " + exdsd.ToString();
                                                Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");

                                            }
                                        }
                                        else
                                        {
                                            Log.For(this, PastaLogCliente).Info(_WorkFlowServico.FinishWorkFlow());
                                            Log.For(this, PastaLogCliente).Error("ERRO DE VALIDACAO:" + Environment.NewLine + ut.MsgValidacaoXML);
                                            string msgerrotraduzida = Biblioteca.RJSOptimusUtil.TraduzMensagemErroNfe(ut.MsgValidacaoXML);
                                            nhp.NotaFiscalAtualizarStatus(CdNotaFiscalSaida, 2, string.Empty, datasource, schema, msgerrotraduzida);
                                        }
                                    }

                                    break;
                                }

                            case 200: //CANCELAMENTO
                                {
                                    try
                                    {
                                        Log.For(this, PastaLogCliente).Info("CANCELAMENTO CDNOTA:" + CdNotaFiscalSaida.ToString());
                                        XML = dtrow["TXNDDCANCLAYOUT"].ToString();

                                        if (string.IsNullOrEmpty(XML))
                                            throw new Exception("O XML não foi gerado CdNotaFiscalSaida:" + CdNotaFiscalSaida.ToString());

                                        oMemoryStream = Util.StringXmlToStream(XML);

                                        docProc.Load(oMemoryStream);
                                        docProc.Save(string.Format("{0}\\{1}-env-canc.xml", DiretorioEnviar, dtrow["NRNFECHAVE"].ToString()));
                                        Log.For(this, PastaLogCliente).Info(string.Format("{0}\\{1}-env-canc.xml", DiretorioEnviar, dtrow["NRNFECHAVE"].ToString()));
                                        //Atualiza o status da nfe para enviado para a Sefaz
                                        nhp.NotaFiscalAtualizarStatus(CdNotaFiscalSaida, 201, string.Empty, datasource, schema);

                                        //LOG DOC-e detalhamento da NFE
                                        nhp.UpdateHistoricoCanc(XML, string.Empty, dtrow["CDNOTAFISCALSAIDA"].ToString(), datasourceDoce, schemaDoce);
                                        Log.For(this, PastaLogCliente).Info("FIM CANCELAMENTO");
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.For(this, PastaLogCliente).Error(_WorkFlowServico.FinishWorkFlow() + Environment.NewLine + ex.ToString());

                                        objemailadm.SUBJECT = "CANCELAMENTO ERRO: " + CdNotaFiscalSaida.ToString() + " SCHEMA:" + schema;
                                        objemailadm.BODY = _WorkFlowServico.FinishWorkFlow() + Environment.NewLine + ex.ToString();
                                        Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");
                                    }

                                    break;
                                }
                            default:
                                {
                                    //TODO: LOG
                                    throw new Exception("Status não configurado " + StatusNfe.ToString());
                                }
                        }

                    }
                    _WorkFlowServico.AddEvento("FIM EXECUÇÃO");
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Log.For(this, PastaLogCliente).Error(_WorkFlowServico.FinishWorkFlow() + Environment.NewLine + ex.ToString());

                    string cha = Cache[ChaveNfe].ToString();

                    if (string.IsNullOrEmpty(cha))
                    {
                        Cache.Insert(ChaveNfe,
                                      ChaveNfe,
                                      null,
                                      Cache.NoAbsoluteExpiration,
                                      TimeSpan.FromSeconds(900));

                        objemailadm.SUBJECT = "NFE SEND ERRO: CdNotaFiscalSaida: " + CdNotaFiscalSaida.ToString() + " SCHEMA:" + schema + " CNPJ:" + cnpjEmpresa;
                        objemailadm.BODY = "ERRO : " + ex.ToString();
                        Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");
                    }

                }
                catch (Exception exs)
                {
                    RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOptimus", string.Concat(_WorkFlowServico.FinishWorkFlow(), Environment.NewLine, exs.ToString()), System.Diagnostics.EventLogEntryType.Error);
                }
            }
        }

        public static object _httpRuntime { get; set; }
    }
}
