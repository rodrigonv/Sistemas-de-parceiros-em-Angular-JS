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
using System.Diagnostics;
using System.Web.Caching;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class NFESaidaReturn
    {
        //string aliasCliente = string.Empty;
        string PastaLogCliente = string.Empty;
        int cdentifilial = 0;
        string emailEmpresa = string.Empty;
        string smtp = string.Empty;
        string porta = string.Empty;
        string usuario = string.Empty;
        string senha = string.Empty;
        string cliente = string.Empty;
        string cnpjcliente = string.Empty;
        bool EnableSSL = false;
        long cnpjEmpresa = 0;
        string multiplasfiliais = string.Empty;
        string DiretorioRetorno = string.Empty;
        string DiretorioAutorizados = string.Empty;
        string DiretorioBackup = string.Empty;
        string DiretorioErro = string.Empty;
        string DiretorioEnviar = string.Empty;
        string datasource = string.Empty;
        string schema = string.Empty;
        string datasourceDoce = string.Empty;
        string schemaDoce = string.Empty;
        ObjEmail objemailadm = null;
        public NFESaidaReturn(
            //string _aliascliente,
            string _cdentifilial,
            string _emailEmpresa,
            string _smtp,
            string _porta,
            string _usuario,
            string _senha,
            string _EnableSSL,
            string _cliente,
            string _cnpjcliente,
            string _multiplasfiliais,
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

            //aliasCliente = _aliascliente;
            cdentifilial = Convert.ToInt32(_cdentifilial);
            emailEmpresa = _emailEmpresa;
            smtp = _smtp;
            porta = _porta;
            usuario = _usuario;
            senha = _senha;
            cliente = _cliente;
            cnpjcliente = _cnpjcliente;
            EnableSSL = _EnableSSL == "1";
            cnpjEmpresa = Convert.ToInt64(_cnpjcliente); ;
            multiplasfiliais = _multiplasfiliais == "1" ? "TRUE" : "FALSE";
            DiretorioRetorno = _DiretorioRetorno;
            DiretorioAutorizados = _DiretorioAutorizados;
            DiretorioBackup = _DiretorioBackup;
            DiretorioErro = _DiretorioErro;
            DiretorioEnviar = _DiretorioEnviar;
            datasource = _datasource;
            schema = _schema;
            datasourceDoce = _datasourceDoce;
            schemaDoce = _schemaDoce;
            PastaLogCliente = string.Format(@"{0}\{1}_{2}_{3}", _pastaLogWs, schema, cdentifilial, _cnpjcliente);
            objemailadm = _objemailadm;
        }

        public NFESaidaReturn()
        {
            cdentifilial = Convert.ToInt32(ConfigurationManager.AppSettings.Get("CDENTIFILIAL"));
            emailEmpresa = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_USUARIO").ToString();
            smtp = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_SMTP").ToString();
            porta = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_PORTA").ToString();
            usuario = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_USUARIO").ToString();
            senha = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_SENHA").ToString();
            cliente = ConfigurationManager.AppSettings.Get("CLIENTE").ToString();
            cnpjcliente = ConfigurationManager.AppSettings.Get("CNPJ").ToString();
            EnableSSL = Biblioteca.RJSOptimusConverter.ToBoolean(ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_ENABLESSL"), false);

            cnpjEmpresa = Convert.ToInt64(ConfigurationManager.AppSettings.Get("CNPJ"));

            multiplasfiliais = ConfigurationManager.AppSettings.Get("MULTIPLASFILIAIS");

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
        }

        private static System.Web.HttpRuntime _httpRuntime;

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

        public void BuscarXMLRetornoUNINFE()
        {
            WorkFlowServico _WorkFlowServico = new WorkFlowServico("NFESaidaReturn");

            NFEHelper nhp = new NFEHelper();
            Util.DadosRecClass oDadosRec = new Util.DadosRecClass();
            Util.DadosRecClass oDadosRecCanc = new Util.DadosRecClass();
            Util.DadosRecClass oDadosRecCancEnv = new Util.DadosRecClass();
            List<Util.DadosRecClass> lstArqRetProc = new List<Util.DadosRecClass>();
            List<Util.DadosRecClass> lstArqRetProcRec = new List<Util.DadosRecClass>();

            string xmlAutorizacao = string.Empty;
            string xmlAutorizado = string.Empty;
            string dtYYYYMM = string.Empty;
            string chaveNFE = string.Empty;
            string NrNota = string.Empty;
            string Serie = string.Empty;
            string dtEmissao = string.Empty;
            //string TipoPessoa = string.Empty;
            //string CNPJ_EMIT = string.Empty;
            //string CPF_CNPJ_CLI = string.Empty;
            string CaminhoArquivoRetorno = string.Empty;
            string CaminhoNovo = string.Empty;
            string CaminhoXMLautorizado = string.Empty;
            string StatusNfe = string.Empty;
            DateTime TsAlteracao = DateTime.MinValue;
            int CDNOTAFISCALSAIDA = 0;
            DataTable DTNfe = null;
            try
            {
                _WorkFlowServico.AddEvento("INÍCIO nhp.NotasFiscaisBuscarRetorno()");

                if (multiplasfiliais.Equals("TRUE"))
                {
                    DTNfe = nhp.NotaFiscalBuscarRetornoFilial(cdentifilial, datasource, schema);
                }
                else
                {
                    DTNfe = nhp.NotaFiscalBuscarRetorno(datasource, schema);
                }

                _WorkFlowServico.AddEvento("FIM nhp.NotasFiscaisBuscarRetorno()");

                System.IO.DirectoryInfo dirInfo = new DirectoryInfo(DiretorioRetorno);

                // System.IO.FileInfo[] ArquivosErro = dirInfo.GetFiles("*.err");

                System.IO.FileInfo[] ArquivosErro = dirInfo.EnumerateFiles().Where(x => x.LastWriteTime.Date == DateTime.Today && x.Name.Contains(".err")).AsParallel().ToArray();

                System.IO.DirectoryInfo dirInfoValidar = null;
                System.IO.DirectoryInfo dirInfoAuto = null;
                System.IO.DirectoryInfo dirInfoAutoErro = null;
                System.IO.FileInfo[] ArquivosRetorno;
                System.IO.FileInfo[] ArquivosSit = null;
                
                // nao precisa cobian apaga
                //try
                //{
                //    if (DateTime.Now.Hour == 23 && (DateTime.Now.Minute >= 1 && DateTime.Now.Minute <= 2))
                //    {
                //        System.IO.FileInfo[] ArquivosRetornoNfeApagar = dirInfoAuto.EnumerateFiles("*.xml", SearchOption.AllDirectories).AsParallel().ToArray();

                //        foreach (FileInfo f in ArquivosRetornoNfeApagar)
                //        {
                //            if (f.LastWriteTime >= DateTime.Now.AddDays(-7))
                //            {
                //                if (System.IO.File.Exists(f.FullName))
                //                {
                //                    System.IO.File.Delete(f.FullName);
                //                }
                //            }
                //        }
                //    }
                //}
                //catch
                //{

                //}

                if (DTNfe.Rows.Count > 0)
                {
                    dirInfoValidar = new DirectoryInfo(DiretorioEnviar.Replace("Envio", "Validar"));

                    dirInfoAuto = new DirectoryInfo(DiretorioAutorizados);

                    dirInfoAutoErro = new DirectoryInfo(DiretorioErro);

                    System.IO.FileInfo[] ArquivosRetornoNfe = dirInfoAuto.EnumerateFiles("*procNFe.xml", SearchOption.AllDirectories).AsParallel().ToArray();

                    foreach (FileInfo fi2 in ArquivosRetornoNfe.ToList())
                    {
                        if (fi2.CreationTime >= DateTime.Now.AddDays(-45))
                        {
                            xmlAutorizacao = Util.LerArquivo(fi2.FullName);
                            oDadosRec = Util.ReciboProtNFe(xmlAutorizacao, fi2.FullName, fi2.Name);
                            lstArqRetProc.Add(oDadosRec);
                        }
                    }


                    xmlAutorizacao = string.Empty;

                    ArquivosRetorno = dirInfo.EnumerateFiles("*-pro-rec.xml", SearchOption.AllDirectories).AsParallel().ToArray();
                    //GetFiles("*-pro-rec.xml", SearchOption.AllDirectories);

                    //Carregar a lista de arquivos de retorno proc-rec
                    foreach (FileInfo fi in ArquivosRetorno.ToList())
                    {
                        xmlAutorizacao = Util.LerArquivo(string.Concat(DiretorioRetorno, "\\", fi.Name));
                        _WorkFlowServico.AddEvento(string.Format("xml autorizacao:{0}", string.Concat(DiretorioRetorno, "\\", fi.Name)));
                        oDadosRec = Util.Recibo(xmlAutorizacao, string.Concat(DiretorioRetorno, "\\", fi.Name), fi.Name);
                        lstArqRetProcRec.Add(oDadosRec);
                    }
                    //Busca os arquivos de erro


                    //Arquivos com a situacao da nfe
                    ArquivosSit = dirInfo.EnumerateFiles("*-sit.xml", SearchOption.AllDirectories).AsParallel().ToArray();
                    //GetFiles("*-sit.xml", SearchOption.AllDirectories);
                    /*
                      0  	Aguardando integração da NF-e    
                      1  	NF-e Integrada                 
                      2  	Erro integração             
                      100	Autorizado uso da NF-e pela SEFAZ       
                      200	Aguardando integração de Cancelamento   
                      201	Cancelamento Integrado        
                      202	Erro de Cancelamento Integrado
                      300	Cancelamento autorizado pela SEFAZ      
                      299	Cancelamento NAO autorizado pela SEFAZ  
                      99 	NF-e Não Autorizada pela SEFAZ                                       
                  */
                }




                #region Arquivo de retorno de NFE individual
                //Pegar todos os retornos de NFE
                _WorkFlowServico.AddEvento(string.Format("INÍCIO NF AGUARDANDO RETORNO QTDE:{0}", DTNfe.Rows.Count));
                foreach (DataRow dtrNfe in DTNfe.Rows)
                {
                    chaveNFE = dtrNfe["NRNFECHAVE"].ToString();
                    dtYYYYMM = dtrNfe["DTEMISSAO2"].ToString();
                    NrNota = dtrNfe["NRNOTA"].ToString();
                    Serie = dtrNfe["CDSERIE"].ToString();

                    dtEmissao = Convert.ToDateTime(dtrNfe["DTEMISSAO"], new CultureInfo("pt-BR")).ToString("dd/MM/yyyy");

                    TsAlteracao = Convert.ToDateTime(dtrNfe["TSALTERACAO"], new CultureInfo("pt-BR"));
                    _WorkFlowServico.AddEvento("DATA EMISSÃO: " + dtEmissao + " DO BANCO:" + dtrNfe["DTEMISSAO"].ToString());

                    //TipoPessoa = dtrNfe["TIPOPESSOA"].ToString();
                    //CNPJ_EMIT = dtrNfe["CNPJ_EMIT"].ToString();
                    //CPF_CNPJ_CLI = dtrNfe["CPF_CNPJ_CLI"].ToString();
                    StatusNfe = dtrNfe["CDNFESTATUS"].ToString();
                    CDNOTAFISCALSAIDA = Convert.ToInt32(dtrNfe["CDNOTAFISCALSAIDA"]);

                    #region Nota Fiscal Aprovada
                    if (StatusNfe == "1")
                    {

                        //Lista de rejeições
                        List<Util.DadosRecClass> lstRej = (from z in lstArqRetProcRec
                                                           where z.chNFe == chaveNFE && z.cStat != "100"
                                                           select z).ToList();
                        //Arquivo com aprovação
                        Util.DadosRecClass drcAprov = (from z in lstArqRetProc
                                                       where z.chNFe == chaveNFE && z.cStat == "100"
                                                       select z).FirstOrDefault();

                        var ArquivosErroNfe = (from a in ArquivosErro
                                               where a.Name.Contains(chaveNFE)
                                               select a);

                        //string nomeArquivoProc = string.Empty;
                        //string nomeArquivo = string.Empty;

                        //Retorno aprovado pela sefaz
                        if (drcAprov != null)
                        {
                            //nomeArquivoProc = string.Format("{0}-pro-rec.xml", drcAprov.nRec);
                            //nomeArquivo = string.Format("{0}-procNFe.xml", drcAprov.chNFe);

                            _WorkFlowServico.AddEvento(string.Format("Nota fiscal nº: {0} dt sefaz:{1}-{2}-{3} Encontrou o retorno.", CDNOTAFISCALSAIDA, drcAprov.dhRecbto, drcAprov.cStat, drcAprov.xMotivo));

                            //Encontrouretorno = true;

                            //Ler o conteudo do arquivo
                            //xmlAutorizacao = Util.LerArquivo(string.Concat(DiretorioRetorno, "\\", nomeArquivoProc));
                            //xmlAutorizacao = Util.LerArquivo(drcAprov.CaminhoArquivo);

                            //Caminho fisico do arquivo
                            //CaminhoArquivoRetorno = string.Concat(DiretorioRetorno, "\\", nomeArquivoProc);

                            //Caminho para onde o arquivo será movido
                            //CaminhoNovo = string.Concat(DiretorioBackup, "\\", nomeArquivoProc);

                            DateTime dtReciboNf = Convert.ToDateTime(drcAprov.dhRecbto, new CultureInfo("pt-BR"));

                            string DTYYYYMM_ = dtReciboNf.ToString("yyyyMM");

                            //Buscar o arquivo xml autorizado -procNFe.xml

                            //FileInfo arqXmlAprovado = (from a in ArquivosRetornoNfe
                            //                           where a.FullName.Contains(drcAprov.NomeArquivo)
                            //                           select a).FirstOrDefault();

                            //if (arqXmlAprovado != null)
                            //{
                            _WorkFlowServico.AddEvento(string.Format("Status {0} da nota fiscal nº {1}", drcAprov.cStat, CDNOTAFISCALSAIDA));

                            //xmlAutorizado = Util.LerArquivo(arqXmlAprovado.FullName);

                            //Atualizar o historionfe com o xml autorizado pela sefaz
                            nhp.UpdateHistorico(drcAprov.xmlCompleto, CDNOTAFISCALSAIDA.ToString(), datasourceDoce, schemaDoce);

                            nhp.NotasFiscalSaidaAtualizarProc(CDNOTAFISCALSAIDA,
                                drcAprov.xmlCompleto,
                                drcAprov.xmlAutorizado,
                                drcAprov.xMotivo,
                                //dtReciboNf.ToString("dd/MM/yyyy HH:mm:ss"),
                                dtReciboNf,
                                drcAprov.nProt,
                                drcAprov.cStat,
                                string.Empty,
                                string.Empty,
                                datasource,
                                schema);
                            //_WorkFlowServico.AddEvento(string.Format());
                            //Mover o arquivo da pasta de retorno
                            //File.Move(CaminhoArquivoRetorno, CaminhoNovo);
                            nhp.InserirHistoricoItem(drcAprov.xmlAutorizado, drcAprov.nProt, drcAprov.cStat, drcAprov.xMotivo, CDNOTAFISCALSAIDA.ToString(), datasourceDoce, schemaDoce);
                            //}
                            //else
                            //{
                            //    _WorkFlowServico.AddEvento(string.Format("XML autorizado não encontrado no caminho informado {0}", CaminhoXMLautorizado));
                            //}

                            //Historico detalhado DOC-e
                            //nhp.InserirHistoricoItem(drcAprov.xml, drcAprov.nProt, drcAprov.cStat, drcAprov.xMotivo, CDNOTAFISCALSAIDA.ToString());
                            //if (!File.Exists(CaminhoXMLautorizado) && arqXmlAprovado != null)
                            //{
                            //    CaminhoXMLautorizado = arqXmlAprovado.FullName;
                            //}

                            //if (!File.Exists(CaminhoXMLautorizado))
                            //{

                            //}
                            //else
                            //{

                            //}
                        }
                        else //não encontrou o autorizado
                        {
                            if (lstRej.Count > 0)
                            {
                                //grava todos outros status
                                Log.For(this, PastaLogCliente).Info("REJEICAO CDNOTA:" + CDNOTAFISCALSAIDA.ToString());

                                string diretorioEmProcessamento = string.Format("{0}\\EmProcessamento\\", DiretorioAutorizados);

                                foreach (Util.DadosRecClass item in lstRej)
                                {
                                    Log.For(this, PastaLogCliente).Info("REJEICAO:" + item.cStat + " " + item.xMotivo);


                                    //Monta o sit para rejeicao para montar o xml de autorizacao
                                    if (item.cStat == "204")//Rejeicao: Duplicidade de NF-e
                                    {
                                        FileInfo ArquivoSit = (from a in ArquivosSit
                                                               where a.Name.Contains(chaveNFE)
                                                               select a).FirstOrDefault();

                                        Util.DadosRetSit RecSit = null;

                                        if (ArquivoSit != null)
                                        {
                                            RecSit = Util.RetornoSit(ArquivoSit.FullName);
                                        }
                                        else
                                        {
                                            string arqSit = Util.RetornaArquivoSitNfe(chaveNFE, "1"/*producao*/);

                                            //salva na pasta de envio
                                            MemoryStream oMemoryStream = Util.StringXmlToStream(arqSit);
                                            XmlDocument docProc = new XmlDocument();
                                            docProc.Load(oMemoryStream);
                                            docProc.Save(string.Format("{0}\\{1}-ped-sit.xml", DiretorioEnviar, chaveNFE));
                                        }

                                        if (ArquivoSit != null && RecSit != null)
                                        {//Caso tenha verificar se a nf esta aprovada

                                            if (RecSit.cStat == "100")
                                            {//Nota aprovada e nao veio o xml
                                                Log.For(this, PastaLogCliente).Info("STATUS 100 NO ARQUIVO SIT CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                                //Copiar o arquivo {chave}-nfe.xml para pasta aprovados/emprocessamento
                                                System.IO.FileInfo[] ArquivosNfeAssinado = dirInfoAuto.EnumerateFiles("*-nfe.xml", SearchOption.AllDirectories).AsParallel().ToArray();

                                                System.IO.FileInfo[] ArquivosNfeAssinadoErro = dirInfoAutoErro.EnumerateFiles("*-nfe.xml", SearchOption.AllDirectories).AsParallel().ToArray();

                                                string nomearquivo = string.Format("{0}-nfe.xml", chaveNFE);

                                                FileInfo nfeassinada = (from a in ArquivosNfeAssinado
                                                                        where a.FullName.Contains(nomearquivo)
                                                                        select a).FirstOrDefault();

                                                if (nfeassinada == null)//nao achou verifica se esta na pasta erro
                                                {
                                                    nfeassinada = (from a in ArquivosNfeAssinadoErro
                                                                   where a.FullName.Contains(nomearquivo)
                                                                   select a).FirstOrDefault();
                                                }

                                                string arqrej2 = DiretorioRetorno + "\\" + item.NomeArquivo;

                                                if (nfeassinada != null)
                                                {//copiar para a pasta em aprovados/emprocessamento
                                                    Log.For(this, PastaLogCliente).Info("ACHOU NF-E ASSINADA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);


                                                    if (!System.IO.File.Exists(nfeassinada.FullName))
                                                    {
                                                        System.IO.File.Copy(nfeassinada.FullName, string.Format("{0}\\{1}", diretorioEmProcessamento, nomearquivo), true);
                                                    }

                                                    string nomearquivosit = string.Format("{0}\\{1}-ped-sit.xml", diretorioEmProcessamento, chaveNFE);
                                                    if (!System.IO.File.Exists(nomearquivosit))
                                                    {
                                                        //gera o arquivo sit
                                                        string arqSit = Util.RetornaArquivoSitNfe(chaveNFE, "1"/*producao*/);
                                                        Log.For(this, PastaLogCliente).Info("GEROU O SIT:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                                        //salva na pasta de envio
                                                        MemoryStream oMemoryStream = Util.StringXmlToStream(arqSit);
                                                        XmlDocument docProc = new XmlDocument();
                                                        docProc.Load(oMemoryStream);
                                                        docProc.Save(string.Format("{0}\\{1}-ped-sit.xml", diretorioEmProcessamento, chaveNFE));
                                                        //chegou aqui grava o historico
                                                        nhp.InserirHistoricoItem(item.xmlAutorizado, item.nProt, item.cStat, item.xMotivo, CDNOTAFISCALSAIDA.ToString(), datasourceDoce, schemaDoce);

                                                        if (System.IO.File.Exists(arqrej2))
                                                        {
                                                            System.IO.File.Delete(arqrej2);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Log.For(this, PastaLogCliente).Info("NAO ACHOU ARQUIVO ASSINADO:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);

                                                    nhp.InserirHistoricoItem(item.xmlAutorizado, item.nProt, item.cStat, item.xMotivo, CDNOTAFISCALSAIDA.ToString(), datasourceDoce, schemaDoce);

                                                    if (System.IO.File.Exists(arqrej2))
                                                    {
                                                        System.IO.File.Delete(arqrej2);
                                                    }
                                                }
                                            }
                                            else if (RecSit.cStat == "217")//<cStat>217</cStat><xMotivo>Rejeição: NF-e não consta na base de dados da SEFAZ
                                            {
                                                //Tenta enviar novamente
                                                Log.For(this, PastaLogCliente).Info("STATUS 217 NO ARQUIVO SIT CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                                nhp.UpdateStatusNf(CDNOTAFISCALSAIDA.ToString(), "0", datasource, schema);
                                            }
                                            else
                                            {
                                                Log.For(this, PastaLogCliente).Info("STATUS " + RecSit.cStat + " " + RecSit.xMotivo + " NO ARQUIVO SIT CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                            }


                                        }
                                    }
                                    else
                                    {
                                        nhp.InserirHistoricoItem(item.xmlAutorizado, item.nProt, item.cStat, item.xMotivo, CDNOTAFISCALSAIDA.ToString(), datasourceDoce, schemaDoce);
                                        //mover o arquivo para pasta de erros
                                        //System.IO.File.Delete(item.CaminhoArquivo);
                                        string arqrej = DiretorioErro + "\\" + item.NomeArquivo;
                                        if (System.IO.File.Exists(arqrej))
                                        {
                                            System.IO.File.Delete(arqrej);
                                        }
                                        else
                                        {
                                            //Atualizar a nf
                                            nhp.UpdateStatusNf(CDNOTAFISCALSAIDA.ToString(), "99", datasource, schema);//2  	Erro integração 
                                            System.IO.File.Move(item.CaminhoArquivo, DiretorioErro + "\\" + item.NomeArquivo + DateTime.Now.ToString("ddMMyyyyhhmmss"));
                                        }
                                    }
                                }

                            }
                            else
                            {
                                //Arquivos de erro da nf filtrada
                                if (ArquivosErroNfe.Count() > 0)
                                {
                                    Log.For(this, PastaLogCliente).Info("ENCONTROU ERRO CDNOTA:" + CDNOTAFISCALSAIDA.ToString());
                                    string conteudoArquivoErro = string.Empty;

                                    foreach (FileInfo file in ArquivosErroNfe)
                                    {
                                        if (System.IO.File.Exists(file.FullName))
                                        {
                                            conteudoArquivoErro = Util.LerArquivo(file.FullName);
                                        }
                                        else
                                        {
                                            conteudoArquivoErro = DiretorioErro + "\\" + file.Name;
                                        }

                                        conteudoArquivoErro = conteudoArquivoErro.Replace('\n', ' ').Replace('\r', ' ');
                                        //tratar quando nao achar o arquivo na pasta temp

                                        if (conteudoArquivoErro.Contains("Could not find file"))
                                        {
                                            nhp.UpdateStatusNf(CDNOTAFISCALSAIDA.ToString(), "0", datasource, schema);
                                        }
                                        else
                                        {
                                            nhp.UpdateStatusNf(CDNOTAFISCALSAIDA.ToString(), "2", datasource, schema);
                                        }

                                        nhp.InserirHistoricoItem(conteudoArquivoErro + file.FullName + DiretorioErro + "\\" + file.Name, string.Empty, "9999", "SEFAZ COM PROBLEMAS", CDNOTAFISCALSAIDA.ToString(), datasourceDoce, schemaDoce);

                                        if (System.IO.File.Exists(file.FullName))
                                        {
                                            System.IO.File.Move(file.FullName, DiretorioErro + "\\" + file.Name + DateTime.Now.ToString("ddMMyyyyhhmmss"));
                                        }

                                        //2 Erro integração

                                    }
                                }
                                else
                                {
                                    if (ArquivosErroNfe.Count() == 0 && lstRej.Count() == 0)
                                    {
                                        TimeSpan tempoEspera = DateTime.Now.Subtract(TsAlteracao);
                                        Log.For(this, PastaLogCliente).Info("NAO ENCONTROU ERRO E REJEICAO CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                        if (tempoEspera.TotalMinutes >= 3)
                                        {
                                            Log.For(this, PastaLogCliente).Info("NAO ENCONTROU ERRO E REJEICAO E MAIS DE 3 MIN CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                            //Verificar se existe o retorno da situacao
                                            FileInfo ArquivoSit = (from a in ArquivosSit
                                                                   where a.Name.Contains(chaveNFE)
                                                                   select a).FirstOrDefault();

                                            Util.DadosRetSit RecSit = null;

                                            if (ArquivoSit != null)
                                            {
                                                RecSit = Util.RetornoSit(ArquivoSit.FullName);
                                            }

                                            if (ArquivoSit != null && RecSit != null)
                                            {//Caso tenha verificar se a nf esta aprovada

                                                if (RecSit.cStat == "100")
                                                {//Nota aprovada e nao veio o xml
                                                    Log.For(this, PastaLogCliente).Info("STATUS 100 NO ARQUIVO SIT CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                                    //Copiar o arquivo {chave}-nfe.xml para pasta aprovados/emprocessamento
                                                    System.IO.FileInfo[] ArquivosNfeAssinado = dirInfoAuto.EnumerateFiles("*-nfe.xml", SearchOption.AllDirectories).AsParallel().ToArray();

                                                    string nomearquivo = string.Format("{0}-nfe.xml", chaveNFE);

                                                    FileInfo nfeassinada = (from a in ArquivosNfeAssinado
                                                                            where a.FullName.Contains(nomearquivo)
                                                                            select a).FirstOrDefault();

                                                    if (nfeassinada == null)//tenta achar no validado
                                                    {
                                                        System.IO.FileInfo[] ArquivosNfeAssinadoValidado = dirInfoValidar.EnumerateFiles("*-nfe.xml", SearchOption.AllDirectories).AsParallel().ToArray();

                                                        nfeassinada = (from a in ArquivosNfeAssinadoValidado
                                                                       where a.FullName.Contains(nomearquivo)
                                                                       select a).FirstOrDefault();
                                                        if (nfeassinada != null)
                                                        {
                                                            Log.For(this, PastaLogCliente).Info("ACHOU NO VALIDADO CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                                        }

                                                    }

                                                    if (nfeassinada != null)
                                                    {//copiar para a pasta em aprovados/emprocessamento
                                                        string diretorioEmProcessamento = string.Format("{0}\\EmProcessamento\\", DiretorioAutorizados);
                                                        System.IO.File.Copy(nfeassinada.FullName, string.Format("{0}\\{1}", diretorioEmProcessamento, nomearquivo), true);
                                                        //gera o arquivo sit
                                                        string arqSit = Util.RetornaArquivoSitNfe(chaveNFE, "1"/*producao*/);

                                                        //salva na pasta de envio
                                                        MemoryStream oMemoryStream = Util.StringXmlToStream(arqSit);
                                                        XmlDocument docProc = new XmlDocument();
                                                        docProc.Load(oMemoryStream);
                                                        docProc.Save(string.Format("{0}\\{1}-ped-sit.xml", diretorioEmProcessamento, chaveNFE));
                                                    }
                                                    else
                                                    {
                                                        //pegar do banco o xml da nf e gravar na pasta validar para assinar
                                                        //depois o proprio processo vai achar o xml e fazer o fluxo correto
                                                        string xmlLayout = nhp.GetXMLnotaLayout(CDNOTAFISCALSAIDA, datasource, schema);

                                                        if (!string.IsNullOrEmpty(xmlLayout))
                                                        {
                                                            //GRAVA NA PASTA VALIDAR
                                                            XmlDocument docProc = new XmlDocument();
                                                            MemoryStream oMemoryStream = Util.StringXmlToStream(xmlLayout);

                                                            docProc.Load(oMemoryStream);

                                                            string diretorioValidar = DiretorioEnviar.Replace("Envio", "Validar");

                                                            string arquivoSalvarValidar = string.Format("{0}\\{1}-nfe.xml", diretorioValidar, chaveNFE);

                                                            docProc.Save(arquivoSalvarValidar);

                                                            Log.For(this, PastaLogCliente).Info("PROCESSO DO SIT - SALVANDO O XML CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                                        }
                                                        else
                                                        {
                                                            Log.For(this, PastaLogCliente).Info("PROCESSO DO SIT - NAO RETORNOU O LAYOUT DA NOTA CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                                        }
                                                    }
                                                }
                                                else if (RecSit.cStat == "217")//<cStat>217</cStat><xMotivo>Rejeição: NF-e não consta na base de dados da SEFAZ
                                                {
                                                    //Tenta enviar novamente
                                                    Log.For(this, PastaLogCliente).Info("STATUS 217 NO ARQUIVO SIT CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                                    nhp.UpdateStatusNf(CDNOTAFISCALSAIDA.ToString(), "0", datasource, schema);
                                                }
                                                else
                                                {
                                                    Log.For(this, PastaLogCliente).Info("STATUS " + RecSit.cStat + " " + RecSit.xMotivo + " NO ARQUIVO SIT CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                                }
                                            }
                                            else
                                            {
                                                //gera o arquivo sit
                                                string arqSit = Util.RetornaArquivoSitNfe(chaveNFE, "1"/*producao*/);
                                                Log.For(this, PastaLogCliente).Info("GERA O ARQUIVO SIT CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                                //salva na pasta de envio
                                                MemoryStream oMemoryStream = Util.StringXmlToStream(arqSit);
                                                XmlDocument docProc = new XmlDocument();
                                                docProc.Load(oMemoryStream);
                                                docProc.Save(string.Format("{0}\\{1}-ped-sit.xml", DiretorioEnviar, chaveNFE));

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (StatusNfe == "99")
                    {
                        Log.For(this, PastaLogCliente).Info("[S99]NFE STATUS 99 CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                        //Verificar se existe o retorno da situacao
                        FileInfo ArquivoSit = (from a in ArquivosSit
                                               where a.Name.Contains(chaveNFE)
                                               select a).FirstOrDefault();

                        Util.DadosRetSit RecSit = null;

                        if (ArquivoSit != null)
                        {
                            RecSit = Util.RetornoSit(ArquivoSit.FullName);
                        }

                        if (ArquivoSit != null && RecSit != null)
                        {//Caso tenha verificar se a nf esta aprovada

                            if (RecSit.cStat == "100")
                            {//Nota aprovada e nao veio o xml
                                Log.For(this, PastaLogCliente).Info("[S99]STATUS 100 NO ARQUIVO SIT CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                //Copiar o arquivo {chave}-nfe.xml para pasta aprovados/emprocessamento
                                System.IO.FileInfo[] ArquivosNfeAssinado = dirInfoAuto.EnumerateFiles("*-nfe.xml", SearchOption.AllDirectories).AsParallel().ToArray();

                                string nomearquivo = string.Format("{0}-nfe.xml", chaveNFE);

                                FileInfo nfeassinada = (from a in ArquivosNfeAssinado
                                                        where a.FullName.Contains(nomearquivo)
                                                        select a).FirstOrDefault();
                                if (nfeassinada != null)
                                {//copiar para a pasta em aprovados/emprocessamento
                                    string diretorioEmProcessamento = string.Format("{0}\\EmProcessamento\\", DiretorioAutorizados);
                                    System.IO.File.Copy(nfeassinada.FullName, string.Format("{0}\\{1}", diretorioEmProcessamento, nomearquivo), true);
                                    //gera o arquivo sit
                                    string arqSit = Util.RetornaArquivoSitNfe(chaveNFE, "1"/*producao*/);

                                    //salva na pasta de diretorio Em Processamento
                                    MemoryStream oMemoryStream = Util.StringXmlToStream(arqSit);
                                    XmlDocument docProc = new XmlDocument();
                                    docProc.Load(oMemoryStream);
                                    docProc.Save(string.Format("{0}\\{1}-ped-sit.xml", diretorioEmProcessamento, chaveNFE));

                                    //STATUS 1 DA NFE, CAI NO FLUXO NORMAL DE PROCURAR O RETORNO
                                    nhp.UpdateStatusNf(CDNOTAFISCALSAIDA.ToString(), "1", datasource, schema);
                                    Log.For(this, PastaLogCliente).Info("[S99]-> STATUS 1 DA NFE CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                }
                            }
                            else if (RecSit.cStat == "217")//<cStat>217</cStat><xMotivo>Rejeição: NF-e não consta na base de dados da SEFAZ
                            {
                                //Tenta enviar novamente
                                Log.For(this, PastaLogCliente).Info("[S99]STATUS 217 NO ARQUIVO SIT CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                                //TODO:
                                //Consultar DOC-e para ver se nao tem erro da sefaz.
                                nhp.UpdateStatusNf(CDNOTAFISCALSAIDA.ToString(), "0", datasource, schema);
                            }
                            else
                            {
                                Log.For(this, PastaLogCliente).Info("[S99]STATUS " + RecSit.cStat + " " + RecSit.xMotivo + " NO ARQUIVO SIT CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                            }
                        }
                        else
                        {
                            //gera o arquivo sit
                            string arqSit = Util.RetornaArquivoSitNfe(chaveNFE, "1"/*producao*/);
                            Log.For(this, PastaLogCliente).Info("[S99]GERA O ARQUIVO SIT CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                            //salva na pasta de envio
                            MemoryStream oMemoryStream = Util.StringXmlToStream(arqSit);
                            XmlDocument docProc = new XmlDocument();
                            docProc.Load(oMemoryStream);
                            docProc.Save(string.Format("{0}\\{1}-ped-sit.xml", DiretorioEnviar, chaveNFE));

                        }

                    }
                    #endregion

                    #region Nota fiscal Cancelada

                    else if (StatusNfe == "201") //buscar cancelamento
                    {

                        //-ret-env-canc.xml

                        System.IO.FileInfo[] ArquivosRetornoEnvCancelamento = dirInfo.EnumerateFiles("*-ret-env-canc.xml", SearchOption.AllDirectories).AsParallel().ToArray();

                        FileInfo cancEnvProt = (from a in ArquivosRetornoEnvCancelamento
                                                where a.Name.Contains(chaveNFE)
                                                select a).FirstOrDefault();

                        System.IO.FileInfo[] ArquivosRetornoProtCancelamento = dirInfoAuto.EnumerateFiles("*-procEventoNFe.xml", SearchOption.AllDirectories).AsParallel().ToArray();

                        _WorkFlowServico.AddEvento(string.Concat("Nota fiscal cancelada nº:", CDNOTAFISCALSAIDA));

                        string nomeArquivoProtCancelamento = string.Format("{0}_110111_01-procEventoNFe.xml", chaveNFE);

                        string xmlCancelamento = string.Empty;

                        FileInfo cancProt = (from a in ArquivosRetornoProtCancelamento
                                             where a.Name == nomeArquivoProtCancelamento
                                             select a).FirstOrDefault();


                        if (cancProt != null)
                        {
                            _WorkFlowServico.AddEvento(string.Concat("Encontrou o arquivo de retorno"));

                            //Encontrouretorno = true;

                            xmlCancelamento = Util.LerArquivo(cancProt.FullName);

                            oDadosRecCanc = Util.ReciboCancelamento(xmlCancelamento);

                            nhp.UpdateHistoricoCanc(string.Empty, xmlCancelamento, CDNOTAFISCALSAIDA.ToString(), datasourceDoce, schemaDoce);

                            _WorkFlowServico.AddEvento(string.Concat("Arquivo: ", cancProt.Name));

                            //Historico detalhado DOC-e
                            nhp.InserirHistoricoItem(oDadosRecCanc.xmlAutorizado, oDadosRecCanc.nProt, oDadosRecCanc.cStat, oDadosRecCanc.xMotivo, CDNOTAFISCALSAIDA.ToString(), datasourceDoce, schemaDoce);
                            /*
                             * SP
                             <cStat>155</cStat>
                                <xMotivo>Cancelamento homologado fora de prazo</xMotivo>
                             */
                            if (oDadosRecCanc.cStat == "135" || oDadosRecCanc.cStat == "155")//Evento registrado e vinculado a NF-e
                            {
                                StatusNfe = "300";
                            }
                            else if (oDadosRecCanc.cStat == "220")
                            {
                                StatusNfe = "100";
                            }
                            else
                            {
                                #region status da sefaz
                                /*
                                    135	Evento registrado e vinculado a NF-e	WS	-
                                    155	Evento registrado fora do prazo e vinculado a NF-e	WS	-
                                    108	Serviço Paralisado Momentaneamente (curto prazo)	WS	B03
                                    109	Serviço Paralisado sem Previsão	WS	B04
                                    203	Rejeição: Emissor não habilitado para emissão da NF-e	WS	H04
                                    205	Rejeição: NF-e está denegada na base de dados da SEFAZ	WS	H08
                                    213	Rejeição: CNPJ-Base do Emitente difere do CNPJ-Base do Certificado Digital	WS	F03
                                    214	Rejeição: Tamanho da mensagem excedeu o limite estabelecido	WS	B01
                                    215	Rejeição: Falha no schema XML	WS	D01
                                    216	Rejeição: Chave de Acesso é diferente do existente no BD	WS	H07
                                    217	Rejeição: NF-e não consta na base de dados da SEFAZ	WS	H06
                                    219	Rejeição: Circulação da NF-e verificada	WS	H13
                                    220	Rejeição: NF-e autorizada há mais de 24 horas	WS	H10
                                    221	Rejeição: Confirmado o recebimento da NF-e pelo destinatário	WS	H12
                                    222	Rejeição: Protocolo de Autorização de Uso difere do cadastrado	WS	H11
                                    236	Rejeição: Chave de Acesso com dígito verificador inválido	WS	H03
                                    238	Rejeição: Cabeçalho - Versão do arquivo XML superior a Versão vigente	WS	C05
                                    239	Rejeição: Cabeçalho - Versão do arquivo XML não suportada	WS	C06
                                    240	Rejeição: Cancelamento/Inutilização - Irregularidade Fiscal do Emitente	WS	H05
                                    242	Rejeição: Cabeçalho - Falha no Schema XML	WS	C01
                                    243	Rejeição: XML Mal Formado	WS	B02
                                    249	Rejeição: UF da Chave de Acesso diverge da UF autorizadora	WS	H02
                                    252	Rejeição: Ambiente informado diverge do Ambiente de recebimento	WS	H01
                                    266	Rejeição: Na autorização pela SEFAZ Autorizada: não aceitar série diferente de 0-899	WS	H02a
                                    280	Rejeição: Certificado Transmissor inválido	WS	A01
                                    281	Rejeição: Certificado Transmissor Data Validade	WS	A02
                                    282	Rejeição: Certificado Transmissor sem CNPJ	WS	A07
                                    283	Rejeição: Certificado Transmissor - erro Cadeia de Certificação	WS	A03
                                    284	Rejeição: Certificado Transmissor revogado	WS	A05
                                    285	Rejeição: Certificado Transmissor difere ICP-Brasil	WS	A06
                                    286	Rejeição: Certificado Transmissor erro no acesso a LCR	WS	A04
                                    290	Rejeição: Certificado Assinatura inválido	WS	E01
                                    291	Rejeição: Certificado Assinatura Data Validade	WS	E02
                                    292	Rejeição: Certificado Assinatura sem CNPJ	WS	E03
                                    293	Rejeição: Certificado Assinatura - erro Cadeia de Certificação	WS	E04
                                    294	Rejeição: Certificado Assinatura revogado	WS	E06
                                    295	Rejeição: Certificado Assinatura difere ICP-Brasil	WS	E07
                                    296	Rejeição: Certificado Assinatura erro no acesso a LCR	WS	E05
                                    297	Rejeição: Assinatura difere do calculado	WS	F02
                                    298	Rejeição: Assinatura difere do padrão do Projeto	WS	F01
                                    299	Rejeição: XML da área de cabeçalho com codificação diferente de UTF-8	WS	C01a
                                    402	Rejeição: XML da área de dados com codificação diferente de UTF-8	WS	D03
                                    404	Rejeição: Uso de prefixo de namespace não permitido	WS	D02
                                    409	Rejeição: Campo cUF inexistente no elemento nfeCabecMsg do SOAP Header	WS	C02
                                    410	Rejeição: UF informada no campo cUF não é atendida pelo Web Service	WS	C03
                                    411	Rejeição: Campo versaoDados inexistente no elemento nfeCabecMsg do SOAP Header	WS	C04
                                    420	Rejeição: Cancelamento para NF-e já cancelada	WS	H09
                                    489	Rejeição: CNPJ informado inválido (DV ou zeros)	WS	G03
                                    490	Rejeição: CPF informado inválido (DV ou zeros)	WS	G04
                                    491	Rejeição: O tpEvento informado inválido	WS	D04
                                    492	Rejeição: O verEvento informado inválido	WS	D05
                                    493	Rejeição: detEvento não atende o Schema XML específico	WS	D06
                                    494	Rejeição: Chave de Acesso inexistente	WS	G06
                                    501	Rejeição: NF-e autorizada há mais de 30 dias (720 horas)	WS	GA02
                                    502	Rejeição: Erro na Chave de Acesso - Campo Id não corresponde à concatenação dos campos correspondentes	WS	H02c
                                    516	Rejeição: Falha no schema XML - inexiste a tag raiz esperada para a mensagem	WS	D01a
                                    517	Rejeição: Falha no schema XML - inexiste atributo versao na tag raiz da mensagem	WS	D01b
                                    545	Rejeição: Falha no schema XML - versão informada na versaoDados do SOAPHeader diverge da versão da mensagem	WS	D01c
                                    561	Rejeição: Mês de Emissão informado na Chave de Acesso difere do Mês de Emissão da NFe	WS	H07a
                                    572	Rejeição: Erro Atributo ID do evento não corresponde a concatenação dos campos ("ID" + tpEvento + chNFe + nSeqEvento)	WS	G05
                                    573	Rejeição: Duplicidade de Evento	WS	G07
                                    574	Rejeição: O autor do evento diverge do emissor da NF-e	WS	G08
                                    575	Rejeição: O autor do evento diverge do destinatário da NF-e	WS	G09
                                    576	Rejeição: O autor do evento não é um órgão autorizado a gerar o evento	WS	G10
                                    577	Rejeição: A data do evento não pode ser menor que a data de emissão da NF-e	WS	G11
                                    578	Rejeição: A data do evento não pode ser maior que a data do processamento	WS	G12
                                    579	Rejeição: A data do evento não pode ser menor que a data de autorização para NF-e não emitida em contingência	WS	G13
                                    580	Rejeição: O evento exige uma NF-e autorizada	WS	GA01
                                    587	Rejeição: Usar somente o namespace padrão da NF-e	WS	D01d
                                    588	Rejeição: Não é permitida a presença de caracteres de edição no início/fim da mensagem ou entre as tags da mensagem	WS	D01e
                                    594	Rejeição: O número de seqüencia do evento informado é maior que o permitido	WS	G03
                                    613	Rejeição: Chave de acesso difere da existente em BD	WS	H07b
                                    999	Rejeição: Erro não catalogado (mensagem)	WS
                                 */
                                #endregion
                            }

                            nhp.NotasFiscalSaidaAtualizarProc(CDNOTAFISCALSAIDA,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                DateTime.Now,
                                                                string.Empty,
                                                                StatusNfe,
                                                                xmlCancelamento,
                                                                oDadosRecCanc.xmlAutorizado,
                                                                datasource,
                                                                schema);

                            _WorkFlowServico.AddEvento(string.Concat("Nota fiscal atualizada para o status : ", StatusNfe));
                        }
                        else
                        {

                            if (cancEnvProt != null)
                            {

                                //_WorkFlowServico.AddEvento("ACHOU RET-ENV-CANC " + cancEnvProt.FullName);

                                Log.For(this, PastaLogCliente).Info("ACHOU RET-ENV-CANC " + cancEnvProt.FullName);

                                string xmlCancelamentoEnv = Util.LerArquivo(cancEnvProt.FullName);
                                oDadosRecCancEnv = Util.ReciboCancelamento(xmlCancelamentoEnv);

                                if (oDadosRecCancEnv.cStat != "135" || oDadosRecCancEnv.cStat != "155")
                                {
                                    Log.For(this, PastaLogCliente).Info("Status " + oDadosRecCancEnv.cStat + " motivo: " + oDadosRecCancEnv.xMotivo);

                                    nhp.UpdateHistoricoCanc(string.Empty, xmlCancelamentoEnv, CDNOTAFISCALSAIDA.ToString(), datasourceDoce, schemaDoce);
                                    //Historico detalhado DOC-e
                                    nhp.InserirHistoricoItem(oDadosRecCancEnv.xmlAutorizado, oDadosRecCancEnv.nProt, oDadosRecCancEnv.cStat, oDadosRecCancEnv.xMotivo, CDNOTAFISCALSAIDA.ToString(), datasourceDoce, schemaDoce);
                                    //volta a nf para autorizada e grava no log
                                    //nhp.NotasFiscalSaidaAtualizarProc(CDNOTAFISCALSAIDA,
                                    //                                   string.Empty,
                                    //                                   string.Empty,
                                    //                                   string.Empty,
                                    //                                   DateTime.Now,
                                    //                                   string.Empty,
                                    //                                   "100",
                                    //                                   xmlCancelamentoEnv,
                                    //                                   oDadosRecCancEnv.xmlAutorizado,
                                    //                                   datasource,
                                    //                                   schema);
                                    nhp.UpdateStatusNf(CDNOTAFISCALSAIDA.ToString(), "100", datasource, schema);
                                    if (System.IO.File.Exists(cancEnvProt.FullName))
                                    {
                                        System.IO.File.Move(cancEnvProt.FullName, DiretorioAutorizados + "\\" + cancEnvProt.FullName + DateTime.Now.ToString("ddMMyyyyhhmmss"));
                                    }


                                }



                            }
                            else
                            {
                                TimeSpan tempoEspera = DateTime.Now.Subtract(TsAlteracao);
                                Log.For(this, PastaLogCliente).Info("NAO ENCONTROU O RETORNO DO CANCELAMENTO:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);

                                string chavecache = Cache[chaveNFE] as string;

                                if (tempoEspera.TotalMinutes >= 15 && string.IsNullOrEmpty(chavecache))
                                {
                                    Log.For(this, PastaLogCliente).Info("MAIS DE 15 MIN SEM O RETORNO DE CANCELAMENTO :" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);

                                    objemailadm.SUBJECT = "#ATENCAO# + 15 MIN SEM O RETORNO DE CANCELAMENTO :" + cliente + " CDNOTAFISCALSAIDA=" + CDNOTAFISCALSAIDA.ToString() + " CHAVE=" + chaveNFE + " SCHEMA:" + schema;
                                    objemailadm.BODY = "#ATENCAO# + 15 MIN SEM O RETORNO DE CANCELAMENTO :" + cliente + " CDNOTAFISCALSAIDA=" + CDNOTAFISCALSAIDA.ToString() + " CHAVE=" + chaveNFE + " SCHEMA:" + schema;
                                    Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");

                                    Cache.Insert(
                                    chaveNFE,
                                    chaveNFE,
                                    null,
                                    Cache.NoAbsoluteExpiration,
                                    TimeSpan.FromSeconds(900));

                                }
                            }



                            //Log.For(this).Info("[CANC]NFE STATUS 201 CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                            ////Verificar se existe o retorno da situacao
                            //FileInfo ArquivoSitCanc = (from a in ArquivosSit
                            //                           where a.Name.Contains(chaveNFE)
                            //                           select a).FirstOrDefault();

                            //Util.DadosRetSit RecSitCanc = null;

                            //TimeSpan tempoEsperaCancelada = DateTime.Now.Subtract(TsAlteracao);

                            //if (ArquivoSitCanc != null)
                            //{
                            //    RecSitCanc = Util.RetornoSit(ArquivoSitCanc.FullName);
                            //}

                            //if (ArquivoSitCanc == null && RecSitCanc == null && tempoEsperaCancelada.TotalMinutes >= 15)
                            //{
                            //    Log.For(this).Info("NAO ENCONTROU O ARQUIVO DE RETORNO DE CANCELAMENTO  CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                            //    //gera o arquivo sit
                            //    string arqSit = Util.RetornaArquivoSitNfe(chaveNFE, "1"/*producao*/);
                            //    Log.For(this).Info("[CANC]GERA O ARQUIVO SIT CDNOTA:" + CDNOTAFISCALSAIDA.ToString() + " CHAVE:" + chaveNFE);
                            //    //salva na pasta de envio
                            //    MemoryStream oMemoryStream = Util.StringXmlToStream(arqSit);
                            //    XmlDocument docProc = new XmlDocument();
                            //    docProc.Load(oMemoryStream);
                            //    docProc.Save(string.Format("{0}\\{1}-ped-sit.xml", DiretorioEnviar, chaveNFE));
                            //}
                            //else
                            //{
                            //    // <cStat>220</cStat>
                            //    //<xMotivo>Rejeicao: Prazo de Cancelamento Superior ao Previsto na Legislacao</xMotivo>
                            //    if (RecSitCanc.cStat == "220")//volta a nf para 100
                            //    {
                            //        nhp.UpdateStatusNf(CDNOTAFISCALSAIDA.ToString(), "0");
                            //    }
                            //    /*
                            //        <cStat>135</cStat>
                            //        <xMotivo>Evento registrado e vinculado a NF-e</xMotivo>
                            //     */
                            //    else if (RecSitCanc.cStat == "135")
                            //    {
                            //        xmlCancelamento = Util.LerArquivo(cancProt.FullName);
                            //        oDadosRecCanc = Util.ReciboCancelamento(xmlCancelamento);
                            //        nhp.UpdateHistoricoCanc(string.Empty, xmlCancelamento, CDNOTAFISCALSAIDA.ToString());
                            //        //cancelamento autorizado
                            //        nhp.UpdateStatusNf(CDNOTAFISCALSAIDA.ToString(), "300");
                            //    }
                            //}
                            _WorkFlowServico.AddEvento(string.Concat("Não Encontrou o arquivo de retorno: ", nomeArquivoProtCancelamento));
                        }

                    }
                    #endregion
                    #region Nota fiscal Cancelada old
                    //else if (StatusNfe == "201") //buscar cancelamento
                    //{
                    //    _WorkFlowServico.AddEvento(string.Concat("Nota fiscal cancelada nº:", CDNOTAFISCALSAIDA));
                    //    //Atualizar para o status 300
                    //    //-can.xml
                    //    string nomeArquivoProtCancelamento = string.Format("{0}-procCancNFe.xml", chaveNFE);
                    //    string nomeArquivoCancelamento = string.Format("{0}-ped-can.xml", chaveNFE);

                    //    string xmlCancelamento = string.Empty;

                    //    FileInfo cancProt = (from a in ArquivosRetornoProtCancelamento
                    //                         where a.Name == nomeArquivoProtCancelamento
                    //                         select a).FirstOrDefault();


                    //    if (cancProt != null)
                    //    {
                    //        _WorkFlowServico.AddEvento(string.Concat("Encontrou o arquivo de retorno"));

                    //        xmlCancelamento = Util.LerArquivo(cancProt.FullName);

                    //        oDadosRecCanc = Util.ReciboCancelamento(xmlCancelamento);

                    //        nhp.UpdateHistoricoCanc(string.Empty, xmlCancelamento, CDNOTAFISCALSAIDA.ToString());

                    //        _WorkFlowServico.AddEvento(string.Concat("Arquivo: ", cancProt.Name));

                    //        //Historico detalhado DOC-e
                    //        nhp.InserirHistoricoItem(oDadosRecCanc.xml, oDadosRecCanc.nProt, oDadosRecCanc.cStat, oDadosRecCanc.xMotivo, CDNOTAFISCALSAIDA.ToString());

                    //        StatusNfe = "300";

                    //        nhp.NotaFiscalAtualizarNota(
                    //                   CDNOTAFISCALSAIDA,
                    //                   string.Empty,
                    //                   string.Empty,
                    //                   string.Empty,
                    //                   chaveNFE,
                    //                   NrNota,
                    //                   Serie,
                    //                   dtEmissao,
                    //                   TipoPessoa,
                    //                   CNPJ_EMIT,
                    //                   CPF_CNPJ_CLI,
                    //                   string.Empty,
                    //                   string.Empty,
                    //                   StatusNfe,
                    //                   xmlCancelamento,
                    //                   string.Empty,
                    //                   NFEHelper.TipoNFe.Cancelamento);

                    //        _WorkFlowServico.AddEvento(string.Concat("Nota fiscal atualizada para o status : ", StatusNfe));
                    //    }
                    //    else
                    //    {
                    //        _WorkFlowServico.AddEvento(string.Concat("Não Encontrou o arquivo de retorno: ", nomeArquivoProtCancelamento));
                    //    }

                    //}
                    #endregion
                }

                _WorkFlowServico.AddEvento("FIM RETORNO NFE");

                #endregion

                #region Arquivos com erro
                if (ArquivosErro.Count() > 0)
                {
                    RJSOptimusEmail email = new RJSOptimusEmail(smtp, Convert.ToInt32(porta), usuario, senha, EnableSSL);
                    string pastaconfigJSON = ConfigurationManager.AppSettings.Get("PASTA_CONF_JSON").ToString();
                    string erros = Util.GerenciarEmailErro(cnpjcliente, schema, pastaconfigJSON, ArquivosErro);

                    if (!string.IsNullOrWhiteSpace(erros))
                    {
                        objemailadm.SUBJECT = string.Format("ERRO NFE RETURN ARQ ERROS {0} Cnpj:{1} schema={2}.", cliente, cnpjcliente, schema);
                        objemailadm.BODY = erros;
                        Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");
                    }
                }
                #region OLD
                //foreach (FileInfo fl in ArquivosErro)
                //{
                //    string arquivo = string.Empty;
                //    string nomearqerro = string.Empty;

                //    try
                //    {
                //        objemailadm.SUBJECT = string.Format("ERRO NFE RETURN ARQ ERROS {0} Cnpj:{1} schema={2}.", cliente, cnpjcliente, schema);



                //        if (System.IO.File.Exists(fl.FullName))
                //        {
                //            arquivo = Util.LerArquivo(fl.FullName);
                //            nomearqerro = fl.FullName;
                //            System.IO.File.Move(nomearqerro, DiretorioErro + "\\" + fl.Name + DateTime.Now.ToString("ddMMyyyyhhmmss"));
                //        }
                //        else
                //        {
                //            arquivo = Util.LerArquivo(DiretorioErro + "\\" + fl.Name);
                //            nomearqerro = DiretorioErro + "\\" + fl.Name;
                //        }
                //        objemailadm.BODY = nomearqerro + " " + arquivo + Environment.NewLine;

                //    }
                //    catch (Exception em)
                //    {
                //        try
                //        {
                //            objemailadm.SUBJECT = string.Format("[C2] ERRO NFE RETURN ARQ ERROS {0} Cnpj:{1} schema={2}.", cliente, cnpjcliente, schema);
                //            objemailadm.BODY = nomearqerro + Util.LerArquivo(nomearqerro) + em.ToString() + Environment.NewLine;
                //            //Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");
                //        }
                //        catch (Exception ex)
                //        {
                //            Log.For(this, PastaLogCliente).Error(ex.ToString());
                //        }
                //    }
                //}
                //if (ArquivosErro.Count() > 0)
                //{
                //    Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");
                //}
                #endregion
                #endregion

            }
            catch (Exception ex)
            {
                try
                {
                    Log.For(this, PastaLogCliente).Error(_WorkFlowServico.FinishWorkFlow() + Environment.NewLine + ex.ToString());

                    objemailadm.SUBJECT = string.Format("[C3 GLOBAL] ERRO Nota fiscal eletrônica {0} {1} {2} Cnpj:{3} schema={4} {5}.", CDNOTAFISCALSAIDA, chaveNFE, cliente, cnpjcliente, schema, _WorkFlowServico.FinishWorkFlow());
                    objemailadm.BODY = ex.ToString();

                    Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");

                    string a = chaveNFE;

                }
                catch (Exception exs)
                {
                    RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOptimus", string.Concat(_WorkFlowServico.FinishWorkFlow(), Environment.NewLine, exs.ToString()), System.Diagnostics.EventLogEntryType.Error);
                }

            }
        }
    }
}
