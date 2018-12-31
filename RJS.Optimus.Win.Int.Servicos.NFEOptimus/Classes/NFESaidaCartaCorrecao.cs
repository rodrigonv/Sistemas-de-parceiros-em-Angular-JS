using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Data;
using System.Globalization;
using RJS.Optimus.Biblioteca;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class NFESaidaCartaCorrecao
    {
        //string aliasCliente = string.Empty;
        string PastaLogCliente = string.Empty;
        long cnpjEmpresa = 0;
        string multiplasfiliais = string.Empty;
        string pastaSchema = string.Empty;
        string DiretorioRetorno = string.Empty;
        string DiretorioAutorizados = string.Empty;
        string DiretorioBackup = string.Empty;
        string DiretorioErro = string.Empty;
        string DiretorioEnviar = string.Empty;
        int cdentifilial = 0;
        string datasource = string.Empty;
        string schema = string.Empty;
        string datasourceDoce = string.Empty;
        string schemaDoce = string.Empty;
        ObjEmail objemailadm = null;

        public NFESaidaCartaCorrecao()
        {
            cnpjEmpresa = Convert.ToInt64(ConfigurationManager.AppSettings.Get("CNPJ"));
            multiplasfiliais = ConfigurationManager.AppSettings.Get("MULTIPLASFILIAIS");
            pastaSchema = ConfigurationManager.AppSettings.Get("PASTA_SCHEMAS");

            DiretorioRetorno = ConfigurationManager.AppSettings.Get("PASTA_XML_RETORNO");
            if (!System.IO.Directory.Exists(DiretorioRetorno))
                throw new Exception(string.Format("O diretório {0} não existe.", DiretorioRetorno));

            DiretorioAutorizados = ConfigurationManager.AppSettings.Get("PASTA_XML_ENVIADO");
            if (!System.IO.Directory.Exists(DiretorioAutorizados))
                throw new Exception(string.Format("O diretório {0} não existe.", DiretorioAutorizados));

            DiretorioBackup = ConfigurationManager.AppSettings.Get("PASTA_BACKUP");
            if (!System.IO.Directory.Exists(DiretorioBackup))
                throw new Exception(string.Format("O diretório {0} não existe.", DiretorioBackup));

            DiretorioErro = ConfigurationManager.AppSettings.Get("PASTA_XML_ERRO");
            if (!System.IO.Directory.Exists(DiretorioErro))
                throw new Exception(string.Format("O diretório {0} não existe.", DiretorioBackup));

            DiretorioEnviar = ConfigurationManager.AppSettings.Get("PASTA_XML_ENVIO");
            if (!System.IO.Directory.Exists(DiretorioEnviar))
                throw new Exception(string.Format("O diretório {0} não existe.", DiretorioEnviar));

            cdentifilial = Convert.ToInt32(ConfigurationManager.AppSettings.Get("CDENTIFILIAL"));
        }

        public NFESaidaCartaCorrecao(
            string _cdentifilial,
            string _cnpjCliente,
            string _multiplasfiliais,
            string _pastaSchema,
            string _DiretorioRetorno,
            string _DiretorioAutorizados,
            string _DiretorioBackup,
            string _DiretorioErro,
            string _DiretorioEnviar,
            string _datasource,
            string _schema,
            string _datasourceDoce,
            string _schemaDoce,
            string _pastaLogWs,
            ObjEmail _objemailadm)
        {
            cnpjEmpresa = Convert.ToInt64(_cnpjCliente);
            multiplasfiliais = _multiplasfiliais == "1" ? "TRUE" : "FALSE";
            pastaSchema = _pastaSchema;
            DiretorioRetorno = _DiretorioRetorno;
            DiretorioAutorizados = _DiretorioAutorizados;
            DiretorioBackup = _DiretorioBackup;
            DiretorioErro = _DiretorioErro;
            DiretorioEnviar = _DiretorioEnviar;
            cdentifilial = Convert.ToInt32(_cdentifilial);
            datasource = _datasource;
            schema = _schema;
            datasourceDoce = _datasourceDoce;
            schemaDoce = _schemaDoce;
            //aliasCliente = _aliasCliente;
            PastaLogCliente = string.Format(@"{0}\{1}_{2}_{3}", _pastaLogWs, schema, cdentifilial, _cnpjCliente);
            objemailadm = _objemailadm;
        }

        public void SalvarXMLCartaCorrecao()
        {
            WorkFlowServico _WorkFlowServico = new WorkFlowServico("NFESaidaCartaCorrecao");
            _WorkFlowServico.AddEvento("INÍCIO PROCESSO");

            NFEHelper nhp = new NFEHelper();
            string XML = string.Empty;
            string ChaveNfe = string.Empty;
            int CdNotaFiscalSaida = 0;
            int CdNotaFiscalSaidaCC = 0;
            int StatusNfe = 0;
            int sequencial = 0;
            MemoryStream oMemoryStream;
            XmlDocument docProc;

            DataTable DTNfe = null;
            string chaveNFE = string.Empty;
            string CaminhoXMLautorizado = string.Empty;
            string CaminhoArquivoRetorno = string.Empty;
            string nomeArquivoProc = string.Empty;
            string nomeArquivo = string.Empty;
            string xmlAutorizado = string.Empty;
            try
            {
                if (multiplasfiliais.Equals("TRUE"))
                {
                    DTNfe = nhp.CartaCorrecaoIntegracaoFilial(cdentifilial, datasource, schema);
                }
                else
                {
                    DTNfe = nhp.CartaCorrecaoIntegracao(datasource, schema);
                }

                if (DTNfe.Rows.Count > 0)
                {
                    _WorkFlowServico.AddEvento("QTDE NFS CC:" + DTNfe.Rows.Count.ToString());

                    foreach (DataRow dtrow in DTNfe.Rows)
                    {
                        XML = string.Empty;

                        docProc = new XmlDocument();

                        CdNotaFiscalSaida = Convert.ToInt32(dtrow["CDNOTAFISCALSAIDA"]);

                        CdNotaFiscalSaidaCC = Convert.ToInt32(dtrow["CDNOTAFISCALSAIDACC"]);

                        StatusNfe = Convert.ToInt32(dtrow["CDSTATUS"]);

                        sequencial = Convert.ToInt32(dtrow["SEQUENCIAL"]);

                        _WorkFlowServico.AddEvento(string.Format("NFS CC: {0} STATUS:{1}", CdNotaFiscalSaida, StatusNfe));

                        /*
                            0  	Aguardando integração da NF-e              
                            1  	NF-e Integrada                             
                            2  	Erro integração da NF-e                    
                            400	Carta de correção autorizada pela SEFAZ    
                            402	Erro de integração de Carta de correção    
                            499	Carta de correção NAO autorizada pela SEFAZ
                            401	Carta de correção integrada                                                   
                        */

                        string dtEmissao = string.Empty;

                        switch (StatusNfe)
                        {
                            #region Envio
                            case 0: //ENVIO
                                {
                                    XML = dtrow["XMLENVIO"].ToString();

                                    _WorkFlowServico.AddEvento("CARREGAR XML DO BANCO");
                                    if (string.IsNullOrEmpty(XML))
                                        throw new Exception("O XML NÃO FOI GERADO CD: " + CdNotaFiscalSaida.ToString());

                                    oMemoryStream = Util.StringXmlToStream(XML);

                                    //Validacao do XML
                                    Util ut = new Util();
                                    //ut.ValidaXMLNFE(XML.Trim(), pastaSchema);

                                    if (string.IsNullOrEmpty(ut.MsgValidacaoXML))
                                    {
                                        _WorkFlowServico.AddEvento("SALVAR XML NA PASTA");
                                        docProc.Load(oMemoryStream);
                                        docProc.Save(string.Format("{0}\\{1}-env-cce.xml", DiretorioEnviar, dtrow["NRNFECHAVE"].ToString()));

                                        _WorkFlowServico.AddEvento("ATUALIZAR STATUS NA NFS CC: " + CdNotaFiscalSaidaCC.ToString());

                                        //Atualiza o status da nfe para NF-e Integrada
                                        nhp.UpdateStatusCC(CdNotaFiscalSaidaCC, 1, 401, datasource, schema);

                                        _WorkFlowServico.AddEvento("NFS CC ATUALIZADA : " + CdNotaFiscalSaidaCC.ToString());

                                        //log DE ENVIADO OK
                                        _WorkFlowServico.AddEvento("NFS CC ENVIADA PARA SEFAZ: " + CdNotaFiscalSaidaCC.ToString());

                                        //LOG DOC-e detalhamento da NFE
                                        dtEmissao = DateTime.Now.ToString("dd/MM/yyyy");

                                        nhp.InserirHistoricoItem(XML,
                                            "",
                                            "401",
                                            "Carta de correção integrada",
                                            CdNotaFiscalSaida.ToString(), datasourceDoce, schemaDoce);
                                    }
                                    else
                                    {
                                        nhp.UpdateStatusCC(CdNotaFiscalSaidaCC, 2, 402, datasource, schema);
                                    }

                                    break;
                                }
                            #endregion
                            #region Retorno
                            case 1:
                                {
                                    //35130314364911000102550010000005031000178455_01-procEventoNFe
                                    string xmlAutorizacao = string.Empty;
                                    //string xmlAutorizacaoCompleto = string.Empty;
                                    // FileInfo fileAutorizacaoCompleto = null;
                                    Util.RetEnvEvento oDadosRetEnv = new Util.RetEnvEvento();

                                    List<Util.RetEnvEvento> lstArqRetProc = new List<Util.RetEnvEvento>();

                                    //System.IO.DirectoryInfo dirInfo = new DirectoryInfo(DiretorioRetorno);
                                    System.IO.DirectoryInfo dirInfoAuto = new DirectoryInfo(DiretorioAutorizados);

                                    //System.IO.FileInfo[] ArquivosRetornoEnvio = dirInfo.GetFiles("*-ret-env-cce.xml", SearchOption.AllDirectories);

                                    System.IO.FileInfo[] ArquivosRetornoAprovados = dirInfoAuto.EnumerateFiles("*-procEventoNFe.xml", SearchOption.AllDirectories).AsParallel().ToArray();

                                    
                                    //Carregar a lista de arquivos de retorno
                                    foreach (FileInfo fi in ArquivosRetornoAprovados.ToList())
                                    {
                                        _WorkFlowServico.AddEvento("LerArquivo " + fi.FullName);
                                        xmlAutorizacao = Util.LerArquivo(fi.FullName);
                                        //xmlAutorizacao = RJSOptimusUtil.RemoveCaracteresEspeciais(xmlAutorizacao, true, true);
                                        _WorkFlowServico.AddEvento("RetornoEvento " + xmlAutorizacao);
                                        oDadosRetEnv = Util.RetornoEvento(xmlAutorizacao, fi.Name, fi.FullName);
                                        lstArqRetProc.Add(oDadosRetEnv);
                                    }

                                    //Lista de rejeições
                                    //Processamento do Lote – o lote foi processado (cStat=128), a validação de cada evento do lote
                                    List<Util.RetEnvEvento> lstRej = (from z in lstArqRetProc
                                                                      where z.retEvento.chNfe == dtrow["NRNFECHAVE"].ToString() && z.retEvento.cStat != "128"
                                                                      select z).ToList();
                                    //Arquivo com aprovação
                                    //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento na NF-e, o 
                                    //Evento será armazenado no repositório do Sistema de Registro de Eventos com a vinculação do Evento à respectiva NF-e (cStat=135);
                                    Util.RetEnvEvento drcAprov = (from z in lstArqRetProc
                                                                  where z.retEvento.chNfe == dtrow["NRNFECHAVE"].ToString() && z.retEvento.cStat == "135" && z.retEvento.nSeqEvento == sequencial
                                                                  select z).FirstOrDefault();

                                    //Arquivo completo
                                    //fileAutorizacaoCompleto = (from a in ArquivosRetornoAprovados
                                    //                           where a.Name.Contains(dtrow["NRNFECHAVE"].ToString())
                                    //                           select a).FirstOrDefault();
                                    //Retorno aprovado pela sefaz
                                    
                                    if (drcAprov != null)
                                    {
                                        _WorkFlowServico.AddEvento(string.Format("Nota fiscal nº: {0} dt sefaz:{1}-{2}-{3} Encontrou o retorno.", CdNotaFiscalSaida, drcAprov.retEvento.dhRegEvento, drcAprov.retEvento.cStat, drcAprov.retEvento.xMotivo));

                                        //Ler o conteudo do arquivo
                                        //xmlAutorizacao = Util.LerArquivo(string.Concat(DiretorioRetorno, "\\", nomeArquivoProc));
                                        //xmlAutorizacao = Util.LerArquivo(drcAprov.CaminhoArquivo);
                                        //Caminho fisico do arquivo
                                        //CaminhoArquivoRetorno = drcAprov.CaminhoArquivo;

                                        //DateTime dtReciboNf = Convert.ToDateTime(drcAprov.retEvento.dhRegEvento, new CultureInfo("pt-BR"));

                                        //string DTYYYYMM_ = dtReciboNf.ToString("yyyyMM");

                                        //if (fileAutorizacaoCompleto == null)
                                        //{
                                        //    _WorkFlowServico.AddEvento(string.Format("XML autorizado não encontrado {0}", fileAutorizacaoCompleto.FullName));
                                        //}
                                        //else
                                        //{
                                        _WorkFlowServico.AddEvento(string.Format("Status {0} da nota fiscal nº {1}", drcAprov.retEvento.cStat, CdNotaFiscalSaida));

                                        //xmlAutorizacaoCompleto = Util.LerArquivo(fileAutorizacaoCompleto.FullName);

                                        nhp.CartaCorrecaoUpdate(CdNotaFiscalSaidaCC, 400, Convert.ToInt32(drcAprov.retEvento.cStat), drcAprov.XMLaprovado, datasource, schema);

                                        nhp.InserirHistoricoItem(drcAprov.XMLaprovado, drcAprov.retEvento.nProt, drcAprov.retEvento.cStat, drcAprov.retEvento.xMotivo, CdNotaFiscalSaida.ToString(), datasourceDoce, schemaDoce);
                                        //}
                                    }
                                    else //não encontrou o autorizado
                                    {

                                        Log.For(this, PastaLogCliente).Info("NAO ENCONTROU RETORNO CCE : " + CdNotaFiscalSaida);
                                        
                                        if (lstRej.Count > 0)
                                        {
                                            //grava todos outros status
                                            foreach (Util.RetEnvEvento item in lstRej)
                                            {
                                                string xmlErro = Util.LerArquivo(item.CaminhoArquivo);
                                                nhp.CartaCorrecaoUpdate(CdNotaFiscalSaidaCC, 499, Convert.ToInt32(item.retEvento.cStat), xmlErro, datasource, schema);
                                                nhp.InserirHistoricoItem(xmlErro, item.retEvento.nProt, item.retEvento.cStat, item.retEvento.xMotivo, CdNotaFiscalSaida.ToString(), datasourceDoce, schemaDoce);

                                                ////mover o arquivo para pasta de erros
                                                //System.IO.File.Delete(item.CaminhoArquivo);
                                                System.IO.File.Move(item.CaminhoArquivo, DiretorioErro + "\\" + item.NomeArquivo);
                                            }

                                        }
                                    }
                                    break;
                                }
                            #endregion

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

                    objemailadm.SUBJECT = "ERRO NFE CC-E" + cnpjEmpresa.ToString() + " SCHEMA:" + schema;
                    objemailadm.BODY = ex.ToString();
                    Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");

                }
                catch (Exception exs)
                {
                    RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOptimus", string.Concat(_WorkFlowServico.FinishWorkFlow(), Environment.NewLine, exs.ToString()), System.Diagnostics.EventLogEntryType.Error);
                }
            }
        }
    }
}
