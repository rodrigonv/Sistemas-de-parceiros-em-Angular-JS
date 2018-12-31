using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Net.Mail;
using OpenPop.Pop3;
using System.IO;
namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class NFEEntrada
    {
        //string aliasCliente = string.Empty;
        string cdentifilial = string.Empty;
        string usuario = string.Empty;
        string senha = string.Empty;
        int porta = 0;
        string server = string.Empty;
        string pastaTmpEntrada = string.Empty;
        string pastaEntradaOK = string.Empty;
        string pastaEntradaERRO = string.Empty;
        string pastaSchema = string.Empty;
        bool EnableSSL = false;
        long cnpjEmpresa = 0;
        string datasource = string.Empty;
        string schema = string.Empty;

        string PastaLogCliente = string.Empty;
        ObjEmail objemailadm = null;
        public NFEEntrada(
           // string _aliasCliente,
            string _cdentifilial, 
            string _usuario, 
            string _senha, 
            string _porta, 
            string _server,
            string _pastaTmpEntrada, 
            string _pastaEntradaOK, 
            string _pastaEntradaERRO, 
            string _pastaSchema, 
            string _EnableSSL, 
            string _cnpjEmpresa, 
            string _datasource, 
            string _schema,
            string _pastaLogWs,
            ObjEmail _objemailadm)
        {
            usuario = _usuario;
            senha = _senha;
            porta = Convert.ToInt32(_porta);
            server = _server;
            pastaTmpEntrada = _pastaTmpEntrada;
            pastaEntradaOK = _pastaEntradaOK;
            pastaEntradaERRO = _pastaEntradaERRO;
            pastaSchema = _pastaSchema;
            EnableSSL = _EnableSSL == "1";
            cnpjEmpresa = Convert.ToInt64(_cnpjEmpresa);
            datasource = _datasource;
            schema = _schema;
            cdentifilial = _cdentifilial;
            //aliasCliente = _aliasCliente;
            PastaLogCliente = string.Format(@"{0}\{1}_{2}_{3}", _pastaLogWs, schema, cdentifilial, _cnpjEmpresa);
            objemailadm = _objemailadm;
        }

        public NFEEntrada()
        {
            usuario = ConfigurationManager.AppSettings.Get("EMAIL_RECEBER_USUARIO");
            senha = ConfigurationManager.AppSettings.Get("EMAIL_RECEBER_SENHA");
            porta = Convert.ToInt32(ConfigurationManager.AppSettings.Get("EMAIL_RECEBER_PORTA"));
            server = ConfigurationManager.AppSettings.Get("EMAIL_RECEBER_POP");
            pastaTmpEntrada = ConfigurationManager.AppSettings.Get("PASTA_TMP_ENTRADA");
            pastaEntradaOK = ConfigurationManager.AppSettings.Get("PASTA_XML_ENTRADA_OK");
            pastaEntradaERRO = ConfigurationManager.AppSettings.Get("PASTA_XML_ENTRADA_ERRO");
            pastaSchema = ConfigurationManager.AppSettings.Get("PASTA_SCHEMAS");
            EnableSSL = Biblioteca.RJSOptimusConverter.ToBoolean(ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_ENABLESSL"), false);
            cnpjEmpresa = Convert.ToInt64(ConfigurationManager.AppSettings.Get("CNPJ"));
        }
  

        //StringBuilder chaveserro = new StringBuilder();
        public void BuscarXMLEntrada()
        {
            WorkFlowServico _WorkFlowServico = new WorkFlowServico("NFEEntrada.xmlEmail");

            try
            {
                using (Pop3Client pop3Client = new Pop3Client())
                {
                    //Buscar o email
                    _WorkFlowServico.AddEvento("INÍCIO BUSCAR E-MAIL");
                    _WorkFlowServico.AddEvento("INÍCIO CONECTAR SERVIDOR");
                    pop3Client.Connect(server, porta, EnableSSL);
                    _WorkFlowServico.AddEvento("FIM CONECTAR SERVIDOR");
                    pop3Client.Authenticate(usuario, senha, AuthenticationMethod.UsernameAndPassword);

                    List<string> lstArquivosAnexos = new List<string>();

                    OpenPop.Mime.Message msg = null;

                    string xml = string.Empty;

                    _WorkFlowServico.AddEvento("QTDE E-MAIL Nº:" + pop3Client.GetMessageCount());

                    int totalmsg = pop3Client.GetMessageCount();

                    //for (int i = 1; i <= pop3Client.GetMessageCount(); i++)
                    for (int i = totalmsg; i > 0; --i)
                    //for (int i = 1; i <= totalmsg; i++)
                    {
                        try
                        {
                            msg = pop3Client.GetMessage(i);
                            bool dtok = true;
                            DateTime dataok = DateTime.MinValue;
                            System.Net.Mail.MailMessage msg1 = msg.ToMailMessage();
                            try
                            {
                                if (msg.Headers.DateSent >= DateTime.Now.AddDays(-12))
                                {

                                }
                                dataok = Convert.ToDateTime(msg.Headers.Date);
                            }
                            catch
                            {
                                dtok = false;
                            }

                            if (dtok)
                            {
                                if (dataok != DateTime.MinValue)
                                {
                                    if (dataok >= DateTime.Now.AddDays(-12))
                                    {
                                        SalvaXMLAnexoNaPasta(msg1);
                                    }
                                }
                                else
                                {
                                    if (msg.Headers.DateSent >= DateTime.Now.AddDays(-12))
                                    {
                                        SalvaXMLAnexoNaPasta(msg1);
                                    }
                                }
                            }
                        }
                        catch (Exception exs)
                        {
                            _WorkFlowServico.AddEvento("ERRO: " + exs.ToString());

                        }
                    }

                    _WorkFlowServico.AddEvento("FIM BUSCAR E-MAIL");

                    if (totalmsg > 0)
                    {
                        Log.For(this, PastaLogCliente).Info(_WorkFlowServico.FinishWorkFlow());
                    }

                }//end using

            }
            catch (Exception ex)
            {
                try
                {
                    Log.For(this, PastaLogCliente).Error(_WorkFlowServico.FinishWorkFlow() + Environment.NewLine + ex.ToString());

                    objemailadm.SUBJECT = "ERRO NFE ENTRADA BUSCAR NO E-MAIL" + cnpjEmpresa.ToString() + " SCHEMA:" + schema;
                    objemailadm.BODY = ex.ToString();
                    Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");

                }
                catch (Exception exs)
                {
                    RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOptimus", string.Concat(_WorkFlowServico.FinishWorkFlow(), Environment.NewLine, exs.ToString()), System.Diagnostics.EventLogEntryType.Error);
                }
            }
        }

        private void SalvaXMLAnexoNaPasta(System.Net.Mail.MailMessage msg1)
        {
            if (msg1.Attachments.Count() > 0)
            {
                foreach (Attachment attach in msg1.Attachments)
                {
                    if (attach.Name != null)
                    {
                        if (attach.Name.ToUpper().Contains(".XML"))
                        {
                            if (!System.IO.File.Exists(pastaTmpEntrada + "\\" + attach.Name))
                            {
                                Util.SalvarAnexoEmailNoDisco(attach, pastaTmpEntrada + "\\", attach.Name);
                            }
                        }
                    }
                }
            }
        }

        public void BuscarXMLnaPasta()
        {
            WorkFlowServico _WorkFlowServico = new WorkFlowServico("NFEEntrada.xmlPasta");
            NFEHelper nfh = new NFEHelper();
            System.IO.DirectoryInfo dirInfoAuto = new DirectoryInfo(pastaTmpEntrada);
            System.IO.FileInfo[] ArquivosRetornoNfe = dirInfoAuto.GetFiles("*.xml", SearchOption.TopDirectoryOnly);

            foreach (FileInfo fi2 in ArquivosRetornoNfe.ToList())
            {
                try
                {
                    NFE _NFE = Util.CarregarNfe(pastaTmpEntrada + "\\" + fi2.Name);

                    //Validar se já existe na tabela NFE
                    string P_DEST_CNPJ = _NFE.dEST_CNPJ;
                    string P_EMIT_CNPJ = _NFE.EMIT_CNPJ;
                    string p_DEST_CPF = _NFE.DEST_CPF;
                    int P_IDE_NNF = _NFE.IDE_NNF;
                    int P_IDE_SERIE = Convert.ToInt32(_NFE.IDE_SERIE);
                    DateTime P_IDE_DEMI = _NFE.IDE_DEMI;

                    //Util ut = new Util();

                    //Primeira Validação com o XSD da sefaz
                    //ut.ValidaXMLNFE(pastaTmpEntrada + "\\" + attach.Name, pastaSchema);

                    //Caso não esteja valido deleta o arquivo
                    //if (string.IsNullOrEmpty(ut.MsgValidacaoXML))
                    //{
                    //TODO:                                   
                    //Verificar se a nota é valida na SEFAZ

                    //Carregar as informações do XML


                    //}
                    //else
                    //{
                    //    _WorkFlowServico.AddEvento(string.Format("XML INVÁLIDO: {0} ERRO:{1}", attach.Name, ut.MsgValidacaoXML));
                    //    System.IO.File.Move(string.Concat(pastaTmpEntrada, "\\", attach.Name), string.Concat(pastaEntradaERRO, "\\", attach.Name));
                    //}

                    if (nfh.NotaFiscalEntradaValidar(string.IsNullOrEmpty(P_DEST_CNPJ) ? p_DEST_CPF : P_DEST_CNPJ, P_EMIT_CNPJ, P_IDE_NNF, P_IDE_SERIE, P_IDE_DEMI, datasource, schema) == 0)
                    {
                        //Gravar na tabela NFE do ORACLE
                        nfh.NotasFiscalEntradaInsert(_NFE, datasource, schema);

                        //Insert com sucesso
                        if (System.IO.File.Exists(string.Concat(pastaEntradaOK, "\\", fi2.Name)))
                        {
                            System.IO.File.Delete(string.Concat(pastaEntradaOK, "\\", fi2.Name));
                        }

                        System.IO.File.Move(string.Concat(pastaTmpEntrada, "\\", fi2.Name), string.Concat(pastaEntradaOK, "\\", fi2.Name));


                        _WorkFlowServico.AddEvento(string.Format("NF Nº{0} CNPJ_EMIT:{1} SERIE:{2} CNPJ_CPF_DEST:{3}.", _NFE.IDE_NNF.ToString(), _NFE.EMIT_CNPJ.ToString(), _NFE.IDE_SERIE.ToString(), _NFE.DEST_CNPJ != null ? _NFE.DEST_CNPJ.ToString() : _NFE.DEST_CPF.ToString()));
                        //chaveserro.AppendLine(pastaTmpEntrada + "\\" + fi2.Name + "OK");
                    }
                    else
                    {
                        _WorkFlowServico.AddEvento(string.Format("NF Nº{0} CNPJ_EMIT:{1} SERIE:{2} CNPJ_DEST:{3} JÁ IMPORTADA.", _NFE.IDE_NNF.ToString(), _NFE.EMIT_CNPJ.ToString(), _NFE.IDE_SERIE.ToString(), _NFE.DEST_CNPJ == null ? _NFE.DEST_CPF : _NFE.DEST_CNPJ));
                        System.IO.File.Delete(string.Concat(pastaTmpEntrada, "\\", fi2.Name));
                        //chaveserro.AppendLine(pastaTmpEntrada + "\\" + fi2.Name + "JA");
                    }
                }
                catch (Exception ex)
                {
                    //chaveserro.AppendLine(pastaTmpEntrada + "\\" + fi2.Name + "  -->EX:" + ex.ToString());

                    Log.For(this, PastaLogCliente).Error(pastaTmpEntrada + "\\" + fi2.Name + "  -->EX:" + ex.ToString());
                    try
                    {
                        System.IO.File.Move(string.Concat(pastaTmpEntrada, "\\", fi2.Name), string.Concat(pastaEntradaERRO, "\\", fi2.Name));

                        objemailadm.SUBJECT = "ERRO NFE ENTRADA BUSCAR NA PASTA" + cnpjEmpresa.ToString() + " SCHEMA:" + schema;
                        objemailadm.BODY = ex.ToString();
                        Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");

                    }
                    catch (Exception exs)
                    {
                        Log.For(this, PastaLogCliente).Error(pastaTmpEntrada + "\\" + fi2.Name + "  ao tentar mover-->EX:" + exs.ToString());
                    }

                }
            }

            if (ArquivosRetornoNfe.Count() > 0)
            {
                Log.For(this, PastaLogCliente).Info(_WorkFlowServico.FinishWorkFlow());
                //string aaaa = chaveserro.ToString();
            }

        }
    }
}
