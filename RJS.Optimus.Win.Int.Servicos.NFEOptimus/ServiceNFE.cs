using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes;
using System.Timers;
using RJS.Optimus.Biblioteca;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus
{
    public partial class ServiceNFE : ServiceBase
    {
        private bool emExecucaoNFESaidaSend;
        private bool emExecucaoNFESaidaReturn;
        private bool emExecucaoNFESaidaEmail;
        private bool emExecucaoNFEEntrada;
        private bool emExecucaoCartaCorrecao;

        private System.Timers.Timer tmNFESaidaSend;
        private System.Timers.Timer tmNFESaidaReturn;
        private System.Timers.Timer tmNFESaidaEmail;
        private System.Timers.Timer tmNFEEntrada;
        private System.Timers.Timer tmCartaoCorrecao;
        private string msgExe = string.Empty;

        public ServiceNFE()
        {
            this.ServiceName = string.Format("Optimus.Win.Int.Servicos.{0}", ConfigurationManager.AppSettings.Get("CLIENTE"));
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                try
                {
                    //Log.For(this, pastacliente).Info("Iniciado em :" + DateTime.Now.ToString() + " " + ConfigurationManager.AppSettings.Get("CLIENTE"));
                    this.GravarLogEventViewer("Iniciado em :" + DateTime.Now.ToString() + " " + ConfigurationManager.AppSettings.Get("CLIENTE"));
                }
                catch (Exception efs)
                {
                    string emailEmpresa = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_USUARIO").ToString();
                    string smtp = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_SMTP").ToString();
                    string porta = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_PORTA").ToString();
                    string usuario = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_USUARIO").ToString();
                    string senha = ConfigurationManager.AppSettings.Get("EMAIL_ENVIAR_SENHA").ToString();
                    string cliente = ConfigurationManager.AppSettings.Get("CLIENTE").ToString();
                    string cnpjcliente = ConfigurationManager.AppSettings.Get("CNPJ").ToString();
                    RJSOptimusEmail email = new RJSOptimusEmail(smtp, Convert.ToInt32(porta), usuario, senha);
                    email.From = emailEmpresa;
                    email.To = "rodrigo.vaz@rjstecnologia.com.br";
                    //email.CC = "vinicius.meira@rjstecnologia.com.br";
                    email.Subject = "LOG erro";
                    email.Body = efs.ToString();
                    email.Enviar();
                }


                int IntervaloNFESaidaSend = Convert.ToInt32(ConfigurationManager.AppSettings.Get("INTERVALO_EXECUCAO_NFESAIDA_SEND")) * 1000;
                int IntervaloNFESaidaReturn = Convert.ToInt32(ConfigurationManager.AppSettings.Get("INTERVALO_EXECUCAO_NFESAIDA_RETURN")) * 1000;
                int IntervaloNFESaidaMail = Convert.ToInt32(ConfigurationManager.AppSettings.Get("INTERVALO_EXECUCAO_NFESAIDA_MAIL")) * 1000;
                int IntervaloNFEEntrada = Convert.ToInt32(ConfigurationManager.AppSettings.Get("INTERVALO_EXECUCAO_NFEENTRADA")) * 1000;
                int IntervaloCartaCorrecao = Convert.ToInt32(ConfigurationManager.AppSettings.Get("INTERVALO_EXECUCAO_CARTACORRECAO")) * 1000;

                #region NFE de saída

                if (ConfigurationManager.AppSettings.Get("PARAR_NFESAIDA_SEND").Equals("FALSE"))
                {
                    tmNFESaidaSend = new System.Timers.Timer();
                    tmNFESaidaSend.Interval = IntervaloNFESaidaSend;
                    tmNFESaidaSend.Elapsed += new ElapsedEventHandler(OnTimedEventNFESaidaSend);
                    tmNFESaidaSend.Enabled = true;
                }
                #endregion

                #region NFE de saída retorno
                if (ConfigurationManager.AppSettings.Get("PARAR_NFESAIDA_RETURN").Equals("FALSE"))
                {
                    tmNFESaidaReturn = new System.Timers.Timer();
                    tmNFESaidaReturn.Interval = IntervaloNFESaidaReturn;
                    tmNFESaidaReturn.Elapsed += new ElapsedEventHandler(OnTimedEventNFESaidaReturn);
                    tmNFESaidaReturn.Enabled = true;
                }
                #endregion

                #region NFE de entrada
                if (ConfigurationManager.AppSettings.Get("PARAR_NFEENTRADA").Equals("FALSE"))
                {
                    tmNFEEntrada = new System.Timers.Timer();
                    tmNFEEntrada.Interval = IntervaloNFEEntrada;
                    tmNFEEntrada.Elapsed += new ElapsedEventHandler(OnTimedEventNFEEntrada);
                    tmNFEEntrada.Enabled = true;
                }
                #endregion

                #region NFE de Saída envio de e-mail
                if (ConfigurationManager.AppSettings.Get("PARAR_NFESAIDA_MAIL").Equals("FALSE"))
                {
                    tmNFESaidaEmail = new System.Timers.Timer();
                    tmNFESaidaEmail.Interval = IntervaloNFESaidaMail;
                    tmNFESaidaEmail.Elapsed += new ElapsedEventHandler(OnTimedEventNFESaidaMail);
                    tmNFESaidaEmail.Enabled = true;
                }
                #endregion

                #region Cartao de correção
                if (ConfigurationManager.AppSettings.Get("PARAR_CARTACORRECAO").Equals("FALSE"))
                {
                    tmCartaoCorrecao = new System.Timers.Timer();
                    tmCartaoCorrecao.Interval = IntervaloCartaCorrecao;
                    tmCartaoCorrecao.Elapsed += new ElapsedEventHandler(OnTimedEventCartaCorrecao);
                    tmCartaoCorrecao.Enabled = true;
                }
                #endregion

            }
            catch (Exception ex)
            {
                this.GravarLogEventViewer(ex.ToString());
                throw ex;
            }
        }

        protected override void OnStop()
        {
        }

        private bool ExecutarNFESaidaSend()
        {
            return !emExecucaoNFESaidaSend;
        }

        private bool ExecutarNFESaidaReturn()
        {
            return !emExecucaoNFESaidaReturn;
        }

        private bool ExecutarNFESaidaEmail()
        {
            return !emExecucaoNFESaidaEmail;
        }

        private bool ExecutarNFEEntrada()
        {
            return !emExecucaoNFEEntrada;
        }

        private bool ExecutarCartaCorrecao()
        {
            return !emExecucaoCartaCorrecao;
        }

        private void OnTimedEventNFESaidaSend(object source, ElapsedEventArgs e)
        {
            if (this.ExecutarNFESaidaSend())
            {
                this.emExecucaoNFESaidaSend = true;
                try
                {
                    if (ConfigurationManager.AppSettings.Get("PARAR_NFESAIDA_SEND").Equals("FALSE"))
                    {
                        msgExe = "Nota fiscal de saída";
                        NFESaidaSend hfess = new NFESaidaSend();
                        hfess.SalvarXMLIntegracaoUNINFE();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        //TODO:
                        //RJSOptimusEmail email = new RJSOptimusEmail(ConfigurationManager.AppSettings.Get("Smtp").ToString(), Convert.ToInt32(ConfigurationManager.AppSettings.Get("Porta")));
                        //email.CC = ConfigurationManager.AppSettings.Get("CC").ToString();
                        //email.From = ConfigurationManager.AppSettings.Get("EmailAdmin").ToString();
                        //email.To = ConfigurationManager.AppSettings.Get("EmailAdmin").ToString();
                        //email.Subject = string.Format("ERRO: {0}.", msgExe);
                        //email.Body = ex.ToString();
                        //email.Enviar();
                        RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOPTIMUS", msgExe + " " + ex.ToString(), EventLogEntryType.Error);

                        throw ex;

                    }//Ultimo nível
                    catch (Exception exf)
                    {
                        RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOPTIMUS", msgExe + " " + exf.ToString(), EventLogEntryType.Error);
                    }
                }
                finally
                {
                    this.emExecucaoNFESaidaSend = false;
                }
            }

        }

        private void OnTimedEventNFESaidaReturn(object source, ElapsedEventArgs e)
        {
            if (this.ExecutarNFESaidaReturn())
            {
                this.emExecucaoNFESaidaReturn = true;
                try
                {
                    if (ConfigurationManager.AppSettings.Get("PARAR_NFESAIDA_RETURN").Equals("FALSE"))
                    {
                        msgExe = "Nota fiscal de saída retorno";
                        NFESaidaReturn hfesr = new NFESaidaReturn();
                        hfesr.BuscarXMLRetornoUNINFE();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        //TODO:
                        //RJSOptimusEmail email = new RJSOptimusEmail(ConfigurationManager.AppSettings.Get("Smtp").ToString(), Convert.ToInt32(ConfigurationManager.AppSettings.Get("Porta")));
                        //email.CC = ConfigurationManager.AppSettings.Get("CC").ToString();
                        //email.From = ConfigurationManager.AppSettings.Get("EmailAdmin").ToString();
                        //email.To = ConfigurationManager.AppSettings.Get("EmailAdmin").ToString();
                        //email.Subject = string.Format("ERRO: {0}.", msgExe);
                        //email.Body = ex.ToString();
                        //email.Enviar();
                        RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOPTIMUS", msgExe + " " + ex.ToString(), EventLogEntryType.Error);

                        throw ex;

                    }//Ultimo nível
                    catch (Exception exf)
                    {
                        RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOPTIMUS", msgExe + " " + exf.ToString(), EventLogEntryType.Error);
                    }
                }
                finally
                {
                    this.emExecucaoNFESaidaReturn = false;
                }
            }

        }

        private void OnTimedEventNFESaidaMail(object source, ElapsedEventArgs e)
        {
            if (this.ExecutarNFESaidaEmail())
            {
                this.emExecucaoNFESaidaEmail = true;
                try
                {
                    if (ConfigurationManager.AppSettings.Get("PARAR_NFESAIDA_MAIL").Equals("FALSE"))
                    {
                        msgExe = "Envio de e-mail nota fiscal de saída";
                        NFESaidaMail hfesm = new NFESaidaMail();
                        hfesm.EnviarEmailCliente();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        //TODO:
                        //RJSOptimusEmail email = new RJSOptimusEmail(ConfigurationManager.AppSettings.Get("Smtp").ToString(), Convert.ToInt32(ConfigurationManager.AppSettings.Get("Porta")));
                        //email.CC = ConfigurationManager.AppSettings.Get("CC").ToString();
                        //email.From = ConfigurationManager.AppSettings.Get("EmailAdmin").ToString();
                        //email.To = ConfigurationManager.AppSettings.Get("EmailAdmin").ToString();
                        //email.Subject = string.Format("ERRO: {0}.", msgExe);
                        //email.Body = ex.ToString();
                        //email.Enviar();
                        RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOPTIMUS", msgExe + " " + ex.ToString(), EventLogEntryType.Error);

                        throw ex;

                    }//Ultimo nível
                    catch (Exception exf)
                    {
                        RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOPTIMUS", msgExe + " " + exf.ToString(), EventLogEntryType.Error);
                    }
                }
                finally
                {
                    this.emExecucaoNFESaidaEmail = false;
                }
            }

        }

        private void OnTimedEventNFEEntrada(object source, ElapsedEventArgs e)
        {
            if (this.ExecutarNFEEntrada())
            {
                this.emExecucaoNFEEntrada = true;
                try
                {
                    if (ConfigurationManager.AppSettings.Get("PARAR_NFEENTRADA").Equals("FALSE"))
                    {
                        msgExe = "Nota fiscal de entrada";
                        NFEEntrada hfee = new NFEEntrada();
                        hfee.BuscarXMLEntrada();

                        //System.Threading.Thread.Sleep(30000);

                        hfee.BuscarXMLnaPasta();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        //TODO:
                        //RJSOptimusEmail email = new RJSOptimusEmail(ConfigurationManager.AppSettings.Get("Smtp").ToString(), Convert.ToInt32(ConfigurationManager.AppSettings.Get("Porta")));
                        //email.CC = ConfigurationManager.AppSettings.Get("CC").ToString();
                        //email.From = ConfigurationManager.AppSettings.Get("EmailAdmin").ToString();
                        //email.To = ConfigurationManager.AppSettings.Get("EmailAdmin").ToString();
                        //email.Subject = string.Format("ERRO: {0}.", msgExe);
                        //email.Body = ex.ToString();
                        //email.Enviar();
                        RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOPTIMUS", msgExe + " " + ex.ToString(), EventLogEntryType.Error);

                        throw ex;

                    }//Ultimo nível
                    catch (Exception exf)
                    {
                        RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOPTIMUS", msgExe + " " + exf.ToString(), EventLogEntryType.Error);
                    }
                }
                finally
                {
                    this.emExecucaoNFESaidaEmail = false;
                }
            }

        }

        private void OnTimedEventCartaCorrecao(object source, ElapsedEventArgs e)
        {
            if (this.ExecutarCartaCorrecao())
            {
                this.emExecucaoCartaCorrecao = true;
                try
                {
                    if (ConfigurationManager.AppSettings.Get("PARAR_CARTACORRECAO").Equals("FALSE"))
                    {
                        msgExe = "Carta de Correção";
                        NFESaidaCartaCorrecao hfcc = new NFESaidaCartaCorrecao();
                        hfcc.SalvarXMLCartaCorrecao();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        //TODO:
                        //RJSOptimusEmail email = new RJSOptimusEmail(ConfigurationManager.AppSettings.Get("Smtp").ToString(), Convert.ToInt32(ConfigurationManager.AppSettings.Get("Porta")));
                        //email.CC = ConfigurationManager.AppSettings.Get("CC").ToString();
                        //email.From = ConfigurationManager.AppSettings.Get("EmailAdmin").ToString();
                        //email.To = ConfigurationManager.AppSettings.Get("EmailAdmin").ToString();
                        //email.Subject = string.Format("ERRO: {0}.", msgExe);
                        //email.Body = ex.ToString();
                        //email.Enviar();
                        RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOPTIMUS", msgExe + " " + ex.ToString(), EventLogEntryType.Error);

                        throw ex;

                    }//Ultimo nível
                    catch (Exception exf)
                    {
                        RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOPTIMUS", msgExe + " " + exf.ToString(), EventLogEntryType.Error);
                    }
                }
                finally
                {
                    this.emExecucaoCartaCorrecao = false;
                }
            }

        }

        /// <summary>
        /// Grava uma mensagem no EventViewer
        /// </summary>
        /// <param name="MSG"></param>
        private void GravarLogEventViewer(string MSG)
        {
            RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("RJS.Optimus.Win.Int.Servicos.NFEOPTIMUS", string.Format("{0}{1} {2}", "INFO: ", DateTime.Now.ToString(), MSG), EventLogEntryType.Information);
        }

    }
}
