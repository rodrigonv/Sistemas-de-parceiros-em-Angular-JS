using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;
using System.Configuration;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class NFEInutilizacao
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

        public NFEInutilizacao()
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

        public NFEInutilizacao(
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

        public void SalvarXMLInutilizacao()
        {
            WorkFlowServico _WorkFlowServico = new WorkFlowServico("NFEInutilizacao");
            _WorkFlowServico.AddEvento("INÍCIO PROCESSO");

            NFEHelper nhp = new NFEHelper();
            string XML = string.Empty;
           
            int CDNOTAFISCALINUTNUMERACAO = 0;
            
            int StatusNfe = 0;
            MemoryStream oMemoryStream;
            XmlDocument docProc;

            DataTable DTNfe = null;
           
            string CaminhoXMLautorizado = string.Empty;
            string CaminhoArquivoRetorno = string.Empty;
            string nomeArquivoProc = string.Empty;
            string nomeArquivo = string.Empty;
            string xmlAutorizado = string.Empty;
            string IDINUTILIZACAO = string.Empty;
            FileInfo fileAutorizacaoCompleto = null;
            try
            {
                if (multiplasfiliais.Equals("TRUE"))
                {
                    DTNfe = nhp.InutilizacaoIntegracaoFilial(cdentifilial, datasource, schema);
                }
                else
                {
                    DTNfe = nhp.InutilizacaoIntegracao(datasource, schema);
                }

                if (DTNfe.Rows.Count > 0)
                {
                    _WorkFlowServico.AddEvento("QTDE INUTILIZACAO:" + DTNfe.Rows.Count.ToString());

                    foreach (DataRow dtrow in DTNfe.Rows)
                    {
                        XML = string.Empty;

                        docProc = new XmlDocument();

                        CDNOTAFISCALINUTNUMERACAO = Convert.ToInt32(dtrow["CDNOTAFISCALINUTNUMERACAO"]);

                        IDINUTILIZACAO = dtrow["IDINUTILIZACAO"].ToString();

                        StatusNfe = Convert.ToInt32(dtrow["CDSTATUS"]);

                        _WorkFlowServico.AddEvento(string.Format("NFS INUTI: {0} STATUS:{1}", CDNOTAFISCALINUTNUMERACAO, StatusNfe));

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
                                        throw new Exception("O XML NÃO FOI GERADO CD: " + CDNOTAFISCALINUTNUMERACAO.ToString());

                                    oMemoryStream = Util.StringXmlToStream(XML);

                                    _WorkFlowServico.AddEvento("SALVAR XML NA PASTA");
                                    docProc.Load(oMemoryStream);
                                    /*
                                     35151234567890123455001000000001000000001-ped-inu.xml
                                     */

                                    string IdInuti = Util.PegarIDInutilizacao(XML);

                                    docProc.Save(string.Format("{0}\\{1}-ped-inu.xml", DiretorioEnviar, IdInuti));

                                    _WorkFlowServico.AddEvento("ATUALIZAR STATUS NA NFS INUTI: " + CDNOTAFISCALINUTNUMERACAO.ToString());

                                    //Atualiza o status da nfe para NF-e Integrada
                                    nhp.UpdateStatusInutilizacao(CDNOTAFISCALINUTNUMERACAO, 1, 0, datasource, schema);

                                    _WorkFlowServico.AddEvento("NFS INUTI ATUALIZADA : " + CDNOTAFISCALINUTNUMERACAO.ToString());

                                    //log DE ENVIADO OK
                                    _WorkFlowServico.AddEvento("NFS INUTI ENVIADA PARA SEFAZ: " + CDNOTAFISCALINUTNUMERACAO.ToString());

                                    //LOG DOC-e detalhamento da NFE
                                    //dtEmissao = DateTime.Now.ToString("dd/MM/yyyy");


                                    //Log.For(this, PastaLogCliente).Info(_WorkFlowServico.FinishWorkFlow());

                                    //nhp.InserirHistoricoItem(XML,
                                    //    "",
                                    //    "401",
                                    //    "Inutilização integrada",
                                    //    CDNOTAFISCALINUTNUMERACAO.ToString(), datasourceDoce, schemaDoce);

                                    break;
                                }
                            #endregion
                            #region Retorno
                            case 1:
                                {
                                    //35151436491100010255001000011112000011119-inu.xml
                                    /*
                                    <retInutNFe versao="2.00" xmlns="http://www.portalfiscal.inf.br/nfe">
                                    <infInut>
                                        <tpAmb>1</tpAmb>
                                        <verAplic>SP_NFE_PL_006q</verAplic>
                                        <cStat>102</cStat>
                                        <xMotivo>Inutilização de número homologado</xMotivo>
                                        <cUF>35</cUF>
                                        <ano>15</ano>
                                        <CNPJ>14364911000102</CNPJ>
                                        <mod>55</mod>
                                        <serie>1</serie>
                                        <nNFIni>11110</nNFIni>
                                        <nNFFin>11110</nNFFin>
                                        <dhRecbto>2015-01-30T12:02:14</dhRecbto>
                                        <nProt>135150063643955</nProt>
                                    </infInut>
                                    </retInutNFe>
                                     */
                                    IDINUTILIZACAO = IDINUTILIZACAO.Substring(2, IDINUTILIZACAO.Length - 2);

                                    string xmlAutorizacao = string.Empty;
                                    string xmlAutorizacaoCompleto = string.Empty;
                                    //FileInfo fileAutorizacaoCompleto = null;
                                    Util.RetInutilizacao oDadosRetInuti = new Util.RetInutilizacao();

                                    List<Util.RetInutilizacao> lstArqRetInut = new List<Util.RetInutilizacao>();

                                    System.IO.DirectoryInfo dirInfo = new DirectoryInfo(DiretorioRetorno);

                                    System.IO.FileInfo[] ArquivosRetornoEnvio = dirInfo.EnumerateFiles("*-inu.xml", SearchOption.AllDirectories).AsParallel().ToArray();
                                    

                                    //Carregar a lista de arquivos de retorno
                                    foreach (FileInfo fi in ArquivosRetornoEnvio.ToList())
                                    {
                                        xmlAutorizacao = Util.LerArquivo(fi.FullName);
                                        oDadosRetInuti = Util.PegarRetornoInutilizacao(xmlAutorizacao,fi.Name);
                                        oDadosRetInuti.CaminhoArquivo = fi.FullName;
                                        oDadosRetInuti.NomeArquivo = fi.Name;
                                        lstArqRetInut.Add(oDadosRetInuti);
                                    }

                                    //Lista de rejeições
                                    //Processamento do Lote – o lote foi processado (cStat=128), a validação de cada evento do lote
                                    List<Util.RetInutilizacao> lstRej = (from z in lstArqRetInut
                                                                         where z.IdInutilizacao == IDINUTILIZACAO && z.cStat != "102"
                                                                      select z).ToList();
                                    //Arquivo com aprovação
                                    //Recebido pelo Sistema de Registro de Eventos, com vinculação do evento na NF-e, o 
                                    //Evento será armazenado no repositório do Sistema de Registro de Eventos com a vinculação do Evento à respectiva NF-e (cStat=135);
                                    Util.RetInutilizacao drcAprov = (from z in lstArqRetInut
                                                                     where z.IdInutilizacao == IDINUTILIZACAO && z.cStat == "102"
                                                                  select z).FirstOrDefault();

                                    //Arquivo completo
                                    fileAutorizacaoCompleto = (from a in ArquivosRetornoEnvio
                                                               where a.Name.Contains(IDINUTILIZACAO)
                                                               select a).FirstOrDefault();
                                    //Retorno aprovado pela sefaz
                                    if (drcAprov != null)
                                    {
                                        _WorkFlowServico.AddEvento(string.Format("Inutlizacao nº: {0} dt sefaz:{1}-{2}-{3} Encontrou o retorno.", 
                                            CDNOTAFISCALINUTNUMERACAO, drcAprov.dhRecbto.ToString("dd/mM/yyyy hh:mm:ss"), drcAprov.cStat, drcAprov.xMotivo));

                                        //Ler o conteudo do arquivo
                                        //xmlAutorizacao = Util.LerArquivo(string.Concat(DiretorioRetorno, "\\", nomeArquivoProc));
                                        xmlAutorizacao = Util.LerArquivo(drcAprov.CaminhoArquivo);
                                        //Caminho fisico do arquivo
                                        CaminhoArquivoRetorno = drcAprov.CaminhoArquivo;
                                        RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes.Util.RetInutilizacao ret = Util.PegarRetornoInutilizacao(xmlAutorizacao, drcAprov.NomeArquivo);
                                        //DateTime dtReciboNf = Convert.ToDateTime(drcAprov.retEvento.dhRegEvento, new CultureInfo("pt-BR"));

                                        //string DTYYYYMM_ = dtReciboNf.ToString("yyyyMM");

                                        if (fileAutorizacaoCompleto == null)
                                        {
                                            _WorkFlowServico.AddEvento(string.Format("XML autorizado não encontrado {0}", fileAutorizacaoCompleto.FullName));
                                        }
                                        else
                                        {
                                            _WorkFlowServico.AddEvento(string.Format("Status {0} da nota fiscal nº {1}", drcAprov.cStat, CDNOTAFISCALINUTNUMERACAO));

                                            xmlAutorizacaoCompleto = Util.LerArquivo(fileAutorizacaoCompleto.FullName);

                                            nhp.InutilizacaoUpdate(CDNOTAFISCALINUTNUMERACAO, 100, Convert.ToInt32(drcAprov.cStat), xmlAutorizacaoCompleto, datasource, schema);

                                            //nhp.InserirHistoricoItem(xmlAutorizacaoCompleto, drcAprov.nProt, drcAprov.cStat, drcAprov.xMotivo, 
                                                //CDNOTAFISCALINUTNUMERACAO.ToString(), datasourceDoce, schemaDoce);
                                        }
                                    }
                                    else //não encontrou o autorizado
                                    {
                                        if (lstRej.Count > 0)
                                        {
                                            //grava todos outros status
                                            foreach (Util.RetInutilizacao item in lstRej)
                                            {
                                                string xmlErro = Util.LerArquivo(item.CaminhoArquivo);
                                                nhp.InutilizacaoUpdate(CDNOTAFISCALINUTNUMERACAO, 99, Convert.ToInt32(item.cStat), xmlErro, datasource, schema);
                                                //nhp.InserirHistoricoItem(xmlErro, item.nProt, item.cStat, item.xMotivo, CDNOTAFISCALINUTNUMERACAO.ToString(), datasourceDoce, schemaDoce);

                                                ////mover o arquivo para pasta de erros
                                                //System.IO.File.Delete(item.CaminhoArquivo);
                                                System.IO.File.Move(item.CaminhoArquivo, DiretorioErro + "\\" + item.NomeArquivo + DateTime.Now.ToString());
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
                    Log.For(this, PastaLogCliente).Info(_WorkFlowServico.FinishWorkFlow());
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Log.For(this, PastaLogCliente).Error(_WorkFlowServico.FinishWorkFlow() + Environment.NewLine + ex.ToString());

                    objemailadm.SUBJECT = "ERRO NFE INUTILIZACAO " + cnpjEmpresa.ToString() + " SCHEMA:" + schema;
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
