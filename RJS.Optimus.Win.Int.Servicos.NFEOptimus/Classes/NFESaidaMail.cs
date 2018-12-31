using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RJS.Optimus.Biblioteca;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Web.Caching;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class NFESaidaMail
    {
        //string aliasCliente = string.Empty;
        string PastaLogCliente = string.Empty;
        string emailEmpresa = string.Empty;
        string smtp = string.Empty;
        string porta = string.Empty;
        string usuario = string.Empty;
        string senha = string.Empty;
        bool EnableSSL = false;
        string MsgPattern = string.Empty;
        string PastaNfePHP = string.Empty;
        string MsgSubject = string.Empty;
        string cliente = string.Empty;
        string urlDanfe = string.Empty;
        bool EnviarEmailTransportador = false;
        string cnpjcliente = string.Empty;
        string datasource = string.Empty;
        string schema = string.Empty;
        string multiplasfiliais = string.Empty;
        int cdentifilial = 0;
        string enviarCopiaCliente = string.Empty;
        string emailAdmin = string.Empty;
        ObjEmail objemailadm = null;
        public NFESaidaMail(
            // string _aliascliente,
                            string _cdentifilial,
                            string _emailEmpresa,
                            string _multiplasfiliais,
                            string _smtp,
                            string _porta,
                            string _usuario,
                            string _senha,
                            string _EnableSSL,
                            string _enviarCopiaCliente,
                            string _emailAdmin,
                            string _EnviarEmailTransportador,
                            string _MsgPattern,
                            string _MsgSubject,
                            string _PastaNfePHP,
                            string _cliente,
                            string _urlDanfe,
                            string _cnpjcliente,
                            string _datasource,
                            string _schema,
                            string _pastaLogWs,
                            ObjEmail _objemailadm)
        {
            //aliasCliente = _aliascliente;
            emailEmpresa = _emailEmpresa;
            cdentifilial = Convert.ToInt32(_cdentifilial);
            smtp = _smtp;
            porta = _porta;
            usuario = _usuario;
            senha = _senha;
            EnableSSL = _EnableSSL == "1";
            MsgPattern = _MsgPattern;
            PastaNfePHP = _PastaNfePHP;
            MsgSubject = _MsgSubject;
            cliente = _cliente;
            urlDanfe = _urlDanfe + "?filial={0}&chave={1}&cliente={2}";
            EnviarEmailTransportador = _EnviarEmailTransportador == "1";
            cnpjcliente = _cnpjcliente;
            datasource = _datasource;
            schema = _schema;
            multiplasfiliais = _multiplasfiliais == "1" ? "TRUE" : "FALSE";
            PastaLogCliente = string.Format(@"{0}\{1}_{2}_{3}", _pastaLogWs, schema, cdentifilial, _cnpjcliente);
            enviarCopiaCliente = _enviarCopiaCliente;
            emailAdmin = _emailAdmin;
            objemailadm = _objemailadm;
        }

        public NFESaidaMail()
        {
            emailEmpresa = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_USUARIO").ToString();
            smtp = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_SMTP").ToString();
            porta = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_PORTA").ToString();
            usuario = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_USUARIO").ToString();
            senha = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_SENHA").ToString();
            EnableSSL = Biblioteca.RJSOptimusConverter.ToBoolean(ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_ENABLESSL"), false);
            MsgPattern = ConfigurationManager.AppSettings.Get("PATTERN_MSG_ENVIO").ToString();
            PastaNfePHP = ConfigurationManager.AppSettings.Get("PASTA_NFEPHP").ToString();
            MsgSubject = ConfigurationManager.AppSettings.Get("PATTERN_MSG_ENVIO_SUBJECT").ToString();
            cliente = ConfigurationManager.AppSettings.Get("CLIENTE").ToString();
            urlDanfe = ConfigurationManager.AppSettings.Get("URLDANFE").ToString();
            EnviarEmailTransportador = Biblioteca.RJSOptimusConverter.ToBoolean(ConfigurationManager.AppSettings.Get("ENVIAR_EMAIL_TRANSPORTADOR"), false);
            cnpjcliente = ConfigurationManager.AppSettings.Get("CNPJ").ToString();
            multiplasfiliais = ConfigurationManager.AppSettings.Get("MULTIPLASFILIAIS");
            cdentifilial = Convert.ToInt32(ConfigurationManager.AppSettings.Get("CDENTIFILIAL"));
            string enviarCopiaCliente = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_COPIA_NFE_CLIENTE").ToString();
            string emailAdmin = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_COPIA_ADM").ToString();
        }
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
        public void EnviarEmailCliente()
        {
            WorkFlowServico _WorkFlowServico = new WorkFlowServico("NFESaidaMail");
            _WorkFlowServico.AddEvento("INÍCIO PROCESSO");
            NFEHelper nfh = new NFEHelper();
            DataTable dt = null;
            try
            {
                //Verificar se a Internet está ativa
                if (RJS.Optimus.Biblioteca.RJSOptimusNetwork.IsConnected())
                {
                    _WorkFlowServico.AddEvento("INÍCIO nfh.NotasFiscaisEnviarEmail()");

                    if (multiplasfiliais.Equals("TRUE"))
                    {

                        dt = nfh.NotaFiscalEnviarEmailFilial(cdentifilial, datasource, schema);
                    }
                    else
                    {
                        dt = nfh.NotaFiscalEnviarEmail(datasource, schema);
                    }

                    _WorkFlowServico.AddEvento("FIM nfh.NotasFiscaisEnviarEmail()");

                    string xml = string.Empty;
                    string emailCliente = string.Empty;
                    string pasta = string.Empty;
                    string chaveNfe = string.Empty;
                    string TxFileNfe = string.Empty;
                    string TxFileNfeAut = string.Empty;
                    int CdNotaFiscalSaida = 0;

                    foreach (DataRow dtr in dt.Rows)
                    {
                        //DADOS
                        emailCliente = dtr["TXEMAIL"].ToString();
                        CdNotaFiscalSaida = Convert.ToInt32(dtr["CDNOTAFISCALSAIDA"]);

                        pasta = dtr["CDENTIFILIAL"].ToString();
                        chaveNfe = dtr["NRNFECHAVE"].ToString();
                        TxFileNfe = dtr["TXFILENFE"].ToString();
                        TxFileNfeAut = dtr["TXFILENFEAUT"].ToString();


                        //não enviar para a lista de dominios informado
                        //mktp.extra.com.br
                        if (EnviarParaDominio(emailCliente))
                        {
                            #region Gravar o arquivo na pasta para o PHP Ler
                            _WorkFlowServico.AddEvento("NRNOTA: " + CdNotaFiscalSaida.ToString() + " CHAVE NFE: " + chaveNfe);

                            string NomeDoArquivo = string.Format("{0}\\{1}\\{2}.xml", PastaNfePHP, pasta, chaveNfe);
                            _WorkFlowServico.AddEvento("NomeDoArquivo: " + NomeDoArquivo);
                            string NomeDoArquivoPdf = string.Format("{0}\\{1}\\{2}.pdf", PastaNfePHP, pasta, chaveNfe);
                            _WorkFlowServico.AddEvento("NomeDoArquivoPdf: " + NomeDoArquivoPdf);
                            //Verifica se o arquivo existe
                            if (!System.IO.File.Exists(NomeDoArquivo))
                            {
                                //Montar o XML
                                StringBuilder XML_NFE = new StringBuilder();
                                XML_NFE.AppendLine(TxFileNfe);

                                _WorkFlowServico.AddEvento("PASTA: " + string.Concat(PastaNfePHP, "\\", pasta));

                                //Se não existe o diretorio cria um novo
                                if (!System.IO.Directory.Exists(string.Concat(PastaNfePHP, "\\", pasta)))
                                    System.IO.Directory.CreateDirectory(string.Concat(PastaNfePHP, "\\", pasta));

                                //Se existe exclui o arquivo
                                if (System.IO.File.Exists(NomeDoArquivo))
                                    System.IO.File.Delete(NomeDoArquivo);

                                _WorkFlowServico.AddEvento("INÍCIO GRAVAÇÃO DO ARQUIVO");
                                _WorkFlowServico.AddEvento("ARQUIVO: " + NomeDoArquivo);
                                StreamWriter sw = new StreamWriter(NomeDoArquivo, false, Encoding.GetEncoding(1252));
                                sw.Write(XML_NFE);
                                sw.Close();

                                _WorkFlowServico.AddEvento("FIM GRAVAÇÃO DO ARQUIVO");

                            }
                            //CARREGAR O XML PARA PEGAR OS DADOS
                            NFE _NFEtrocar = Util.CarregarNfe(NomeDoArquivo);
                            _WorkFlowServico.AddEvento("CARREGA O XML PARA PERGAR OS DADOS:" + NomeDoArquivo);

                            #endregion
                            _WorkFlowServico.AddEvento("INÍCIO DOWNLOAD DO PDF");
                            #region download do pdf
                            bool pdfOK = false;

                            int i = 0;

                            for (i = 1; i < 5; i++)
                            {
                                if (!pdfOK)
                                {
                                    _WorkFlowServico.AddEvento("TENTATIVA Nº:" + i.ToString());
                                    //Log.For(this, PastaLogCliente).Info("TENTATIVA Nº:" + i.ToString());
                                    //gerar o PDF e gravar o PDF
                                    if (i == 1)
                                    {
                                        _WorkFlowServico.AddEvento("URL: " + string.Format(urlDanfe, pasta, chaveNfe, schema));
                                        _WorkFlowServico.AddEvento("ARQUIVO: " + NomeDoArquivoPdf);
                                        //Log.For(this, PastaLogCliente).Info("PDF URL: " + string.Format(urlDanfe, pasta, chaveNfe, cliente));
                                        //Log.For(this, PastaLogCliente).Info("ARQUIVO: " + NomeDoArquivoPdf);
                                    }

                                    if (!System.IO.File.Exists(NomeDoArquivoPdf))//se nao existe faz o download
                                    {
                                        try
                                        {
                                            Util.DownloadFile(string.Format(urlDanfe, pasta, chaveNfe, schema), NomeDoArquivoPdf);
                                            _WorkFlowServico.AddEvento("BAIXOU NA TENTATIVA:" + i.ToString());
                                        }
                                        catch (Exception exw)
                                        {
                                            Util.DownloadFile(string.Format(urlDanfe, pasta, chaveNfe, schema), NomeDoArquivoPdf);
                                            if (!System.IO.File.Exists(NomeDoArquivoPdf))
                                            {
                                                _WorkFlowServico.AddEvento("NAO BAIXOU NA EXECEPTION :" + exw.ToString());
                                            }
                                            else
                                            {
                                                _WorkFlowServico.AddEvento("BAIXOU NA EXECEPTION :" + exw.ToString());
                                            }
                                        }

                                        if (!System.IO.File.Exists(NomeDoArquivoPdf))
                                        {
                                            _WorkFlowServico.AddEvento("NAO CONSEGUIU BAIXAR:" + NomeDoArquivoPdf);
                                        }
                                        else
                                        {
                                            _WorkFlowServico.AddEvento("FIM DOWNLOAD DO PDF TENTATIVA Nº:" + i.ToString());

                                            FileInfo fi = new FileInfo(NomeDoArquivoPdf);
                                            if (fi.Length < 6144)// menor que 6kb
                                            {
                                                if (System.IO.File.Exists(NomeDoArquivoPdf))
                                                    System.IO.File.Delete(NomeDoArquivoPdf);

                                                Util.DownloadFile(string.Format(urlDanfe, pasta, chaveNfe, schema), NomeDoArquivoPdf);
                                            }
                                            else
                                            {
                                                pdfOK = true;
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            RJSOptimusEmail email = new RJSOptimusEmail(smtp, Convert.ToInt32(porta), usuario, senha, EnableSSL);

                            FileInfo fiPdf = new FileInfo(NomeDoArquivoPdf);
                            #region Envio do email
                            if (fiPdf.Length < 6144)
                            {
                                _WorkFlowServico.AddEvento("ERRO GERAR PDF (TAMANHO):" + NomeDoArquivoPdf);
                                Log.For(this, PastaLogCliente).Error(_WorkFlowServico.FinishWorkFlow());
                                objemailadm.SUBJECT = string.Format("ERRO GERAR PDF (TAMANHO) {0} Cnpj:{1} SCHEMA{2}:.", cliente, cnpjcliente, schema);
                                objemailadm.BODY = fiPdf.FullName;
                                Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");
                            }
                            else
                            {
                                //Enviar o e-mail
                                _WorkFlowServico.AddEvento("INÍCIO ENVIO E-MAIL");
                                if (smtp.Trim() == "smtp.optimuserp.com.br")
                                {
                                    email.From = "nfe@optimuserp.com.br";
                                }
                                else
                                {
                                    email.From = emailEmpresa;
                                }
                                email.From = emailEmpresa;
                                bool EmailClienteValido = true;

                                email.Subject = MsgSubject.Replace("#NRNOTA#", _NFEtrocar.IDE_NNF.ToString()).
                                            Replace("#CHAVE#", _NFEtrocar.IDE_ID).
                                            Replace("#DTEMISSAO#", _NFEtrocar.IDE_DEMI.ToString("dd/MM/yyyy")).
                                            Replace("#SERIE#", _NFEtrocar.IDE_SERIE).
                                            Replace("#RAZAO#", _NFEtrocar.RAZAOSOCIAL).
                                            Replace("#CODPEDIDO#", _NFEtrocar.CODPEDIDO).
                                            Replace("#NMCLIENTE#", _NFEtrocar.DEST_NOME); ;

                                //if (Util.ValidarEmail(emailCliente))
                                //{

                                email.To = emailCliente;

                                string BodyTemplate = MsgPattern.ToString();

                                //Pegar do template
                                //se vier o nome do arquivo carrega se nao pega o texto que veio normal
                                if (BodyTemplate.Contains(".html"))
                                {
                                    string pastaTemplate = ConfigurationManager.AppSettings.Get("PASTA_TEMPLATE_HTML").ToString();

                                    string Body = System.IO.File.ReadAllText(pastaTemplate + BodyTemplate);

                                    BodyTemplate = Body;

                                }

                                BodyTemplate = BodyTemplate.Replace("#CNPJ#", Convert.ToUInt64(_NFEtrocar.EMIT_CNPJ).ToString(@"00\.000\.000\/0000\-00"));

                                BodyTemplate = BodyTemplate.Replace("#NRNOTA#", _NFEtrocar.IDE_NNF.ToString()).
                                            Replace("#CHAVE#", _NFEtrocar.IDE_ID).
                                            Replace("#DTEMISSAO#", _NFEtrocar.IDE_DEMI.ToString("dd/MM/yyyy")).
                                            Replace("#SERIE#", _NFEtrocar.IDE_SERIE).
                                            Replace("#RAZAO#", _NFEtrocar.RAZAOSOCIAL).
                                            Replace("#CODPEDIDO#", _NFEtrocar.CODPEDIDO).
                                            Replace("#NMCLIENTE#", _NFEtrocar.DEST_NOME);



                                email.Body = BodyTemplate;

                                _NFEtrocar = null;

                                try
                                {
                                    if (!string.IsNullOrEmpty(emailAdmin))
                                    {
                                        email.ListaEmailsBCC.Add(emailAdmin);
                                    }

                                    if (!string.IsNullOrEmpty(enviarCopiaCliente) && EmailClienteValido)
                                    {
                                        string[] emailscliente = enviarCopiaCliente.Split(';');

                                        foreach (string emailcli in emailscliente)
                                        {
                                            email.ListaEmailsBCC.Add(emailcli);
                                        }
                                    }

                                    email.AddAnexo(new System.Net.Mail.Attachment(NomeDoArquivo));//XML
                                    email.AddAnexo(new System.Net.Mail.Attachment(NomeDoArquivoPdf));//PDF
                                    email.IsBodyHtml = true;
                                    email.Enviar();

                                    //Atualiza o status da NFE como e-mail enviado
                                    nfh.NotaFiscalUpdateEmailEnviado(CdNotaFiscalSaida, 3, datasource, schema);//ele tentar enviar 2x 

                                    #region Enviar e-mail para transportador

                                    if (EnviarEmailTransportador)
                                    {
                                        _WorkFlowServico.AddEvento("INÍCIO ENVIO E-MAIL TRANSPORTADOR");
                                        List<string> EmailsTransportador = new List<string>();

                                        //Carrega o xml e pega o CNPJ do transportador
                                        string _CNPJ = Util.PegarCNPJTransportador(NomeDoArquivo);

                                        if (!string.IsNullOrEmpty(_CNPJ))
                                        {
                                            //Verifica se ja esta na base antes de buscar o transportador
                                            if (CacheLayer.Exists(_CNPJ))
                                            {
                                                EmailsTransportador = CacheLayer.Get<List<string>>(_CNPJ);
                                                _WorkFlowServico.AddEvento("ACHOU NO CACHE");
                                            }
                                            else
                                            {
                                                NFEHelper nh = new NFEHelper();
                                                //Busca na base os e-mail do transportador
                                                EmailsTransportador = nh.RetornaEmailTransportadora(_CNPJ, datasource, schema);

                                                if (EmailsTransportador.Count > 0)
                                                {
                                                    CacheLayer.Add(EmailsTransportador, _CNPJ);
                                                    _WorkFlowServico.AddEvento("ACHOU OS EMAILS : " + EmailsTransportador.Count.ToString());
                                                }
                                            }

                                            if (EmailsTransportador.Count > 0)
                                            {
                                                try
                                                {
                                                    RJSOptimusEmail emailtransp = new RJSOptimusEmail(smtp, Convert.ToInt32(porta), usuario, senha, EnableSSL);

                                                    if (!string.IsNullOrEmpty(emailAdmin))
                                                    {
                                                        emailtransp.ListaEmailsBCC.Add(emailAdmin);
                                                    }

                                                    if (smtp.Trim() == "smtp.optimuserp.com.br")
                                                    {
                                                        emailtransp.From = "nfe@optimuserp.com.br";
                                                    }
                                                    else
                                                    {
                                                        emailtransp.From = emailEmpresa;
                                                    }

                                                    emailtransp.Subject = MsgSubject;
                                                    emailtransp.Body = "Nota fiscal eletronica";

                                                    //TODO lista de e-mails
                                                    foreach (string emailtranspenviar in EmailsTransportador)
                                                    {
                                                        //emailtransp.ListaEmailsBCC.Add(emailtranspenviar);
                                                        _WorkFlowServico.AddEvento("EMAIL: " + emailtranspenviar);
                                                        emailtransp.ListaEmailsObj.Add(new System.Net.Mail.MailAddress(emailtranspenviar));
                                                    }

                                                    emailtransp.AddAnexo(new System.Net.Mail.Attachment(NomeDoArquivo));//XML
                                                    emailtransp.IsBodyHtml = true;
                                                    emailtransp.Enviar();
                                                    _WorkFlowServico.AddEvento("FIM ENVIO E-MAIL TRANSPORTADOR");
                                                }
                                                catch (Exception ext)
                                                {
                                                    _WorkFlowServico.AddEvento("ERRO ENVIO E-MAIL TRANSP:" + ext.ToString());
                                                }
                                            }
                                            else
                                            {
                                                _WorkFlowServico.AddEvento("NÃO RETORNOU E-MAIL TRANSP CNPJ: " + _CNPJ);
                                            }
                                        }
                                        else
                                        {
                                            _WorkFlowServico.AddEvento("NÃO ACHOU TRANSP " + NomeDoArquivo);
                                        }
                                    }
                                    #endregion

                                    _WorkFlowServico.AddEvento("FIM ENVIO E-MAIL ");

                                    Log.For(this, PastaLogCliente).Info(_WorkFlowServico.FinishWorkFlow());
                                }
                                catch (Exception exx)
                                {
                                    nfh.NotaFiscalUpdateEmailEnviado(CdNotaFiscalSaida, datasource, schema);

                                    try
                                    {

                                        Log.For(this, PastaLogCliente).Error(_WorkFlowServico.FinishWorkFlow() + Environment.NewLine + exx.ToString());

                                        string chavecache = Cache[CdNotaFiscalSaida.ToString()] as string;

                                        if (string.IsNullOrEmpty(chavecache))
                                        {
                                            Cache.Insert(
                                                CdNotaFiscalSaida.ToString(),
                                                CdNotaFiscalSaida.ToString(),
                                                null,
                                                Cache.NoAbsoluteExpiration,
                                                TimeSpan.FromSeconds(900));

                                            objemailadm.SUBJECT = string.Format("ERRO ENVIAR E-MAIL CLIENTE: {0} Cnpj: {1} E-MAIL CLIENTE: {2} CDNOTAFISCALSAIDA: {3} SCHEMA: {4}.", cliente, cnpjcliente, emailCliente, CdNotaFiscalSaida, schema);
                                            objemailadm.BODY = exx.ToString();
                                            Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");
                                        }

                                    }
                                    catch (Exception exxx)
                                    {
                                        Log.For(this, PastaLogCliente).Error(_WorkFlowServico.FinishWorkFlow() + Environment.NewLine + exxx.ToString());
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {

                            nfh.NotaFiscalUpdateEmailEnviado(CdNotaFiscalSaida, 3, datasource, schema);



                        }//fim tratamento do dominio    

                    }
                }
                else
                {
                    //Internet fora do ar
                    _WorkFlowServico.AddEvento("SEM CONEXÃO COM A INTERNET");
                    Log.For(this, PastaLogCliente).Error(_WorkFlowServico.FinishWorkFlow());
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Log.For(this, PastaLogCliente).Error(_WorkFlowServico.FinishWorkFlow() + Environment.NewLine + ex.ToString());

                    objemailadm.SUBJECT = string.Format("ERRO CATCH GERAL ENVIAR E-MAIL. CLIENTE: {0} Cnpj: {1}.", cliente, cnpjcliente);
                    objemailadm.BODY = _WorkFlowServico.FinishWorkFlow() + Environment.NewLine + ex.ToString();
                    Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");

                }
                catch (Exception exs)
                {
                    RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOptimus", string.Concat(_WorkFlowServico.FinishWorkFlow(), Environment.NewLine, exs.ToString()), System.Diagnostics.EventLogEntryType.Error);
                }
            }
        }

        public bool EnviarParaDominio(string email)
        {
            bool retorno = true;
            bool temdominio = false;
            string dominiosconfig = ConfigurationManager.AppSettings.Get("DOMINIOS_NAO_ENVIAR").ToString();

            if (dominiosconfig.Contains("|"))
            {
                string[] dominios = dominiosconfig.Split('|');

                foreach (var item in dominios)
                {
                    temdominio = email.Contains(item);
                    if (temdominio)
                    {
                        retorno = !temdominio;
                        break;
                    }
                }
            }
            else
            {
                retorno = !email.Contains(dominiosconfig);
            }
            return retorno;
        }

        public static object _httpRuntime { get; set; }
    }
}
