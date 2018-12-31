using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class NFESaidaReturnNaoProcessada
    {

        public void ProcessarRetornoSefazManual(string DiretorioEnviar, string PastaLogCliente, ObjEmail _objemailadm, string datasource, string schema, string pastaLogWs, string cdentifilial, string cnpjCliente)
        {
            PastaLogCliente = string.Format(@"{0}\{1}_{2}_{3}", pastaLogWs, schema, cdentifilial, cnpjCliente);
            
            DiretorioEnviar = DiretorioEnviar + "\\naoprocessadossefaz";

            if (!System.IO.Directory.Exists(DiretorioEnviar )) 
            {
                System.IO.Directory.CreateDirectory(DiretorioEnviar);
                System.IO.Directory.CreateDirectory(DiretorioEnviar + "\\" + "Processado");
            }

            System.IO.DirectoryInfo dirInfo = new DirectoryInfo(DiretorioEnviar);
            System.IO.FileInfo[] ArquivosRetorno = dirInfo.EnumerateFiles("*.xml", SearchOption.TopDirectoryOnly).AsParallel().ToArray();
            ObjEmail objemailadm = _objemailadm;
            string xmlAutorizacao = string.Empty;
            Util.DadosRecClass oDadosRec = new Util.DadosRecClass();
            List<Util.DadosRecClass> lstArqRetProc = new List<Util.DadosRecClass>();

            try
            {
                foreach (FileInfo fi in ArquivosRetorno.ToList())
                {
                    xmlAutorizacao = Util.LerArquivo(fi.FullName);

                    xmlAutorizacao = xmlAutorizacao.Replace("\r", "").Replace("\n", "");

                    oDadosRec = Util.ReciboProtNFe(xmlAutorizacao, fi.FullName, fi.Name);

                    lstArqRetProc.Add(oDadosRec);
                }

                if (lstArqRetProc.Count > 0)
                {
                    Log.For(this, PastaLogCliente).Info("INICIO----------------------------------------------------------------------------");
                    Log.For(this, PastaLogCliente).Info("XML NAO RETORNADOS PELA SEFAZ");
                    Log.For(this, PastaLogCliente).Info(lstArqRetProc.Count.ToString() + "ARQUIVOS ENCONTRADOS");

                    NFEHelper nh = new NFEHelper();

                    foreach (var item in lstArqRetProc)
                    {
                        //atualiza no banco pela chave
                        Log.For(this, PastaLogCliente).Info("CHAVE:" + item.chNFe + "SCHEMA:" + schema);

                        nh.NotaFiscalAtualizarNaoProcessada(item.chNFe, item.xmlCompleto, item.xmlAutorizado, Convert.ToDateTime(item.dhRecbto), item.nProt, item.xMotivo, datasource, schema);

                        System.IO.File.Move(item.CaminhoArquivo, DiretorioEnviar + "\\" + "Processado" + "\\" + item.NomeArquivo + DateTime.Now.ToString("ddMMyyyyhhmmss"));
                    }
                    Log.For(this, PastaLogCliente).Info("FIM----------------------------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                Log.For(this, PastaLogCliente).Error("XML NAO RETORNADOS PELA SEFAZ - ERRO : ");
                Log.For(this, PastaLogCliente).Error(ex.ToString());
                Log.For(this, PastaLogCliente).Error("FIM ERRO XML NAO RETORNADOS PELA SEFAZ");
                objemailadm.SUBJECT = "Erro carregar xml não processado schema:" + schema;
                objemailadm.BODY = ex.ToString();
                Util.EnviaEmail(objemailadm, "opterro@optimuserp.com.br");
            }
        }
    }
}
