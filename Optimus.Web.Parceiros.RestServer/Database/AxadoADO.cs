using Optimus.Web.Parceiros.RestServer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Database
{
    public class AxadoADO
    {


        public static AxadoCotacoes RetornaTabelaCotacao(string cdcesta, string cepcliente, string datasource, string schema, out string pjsonretorno)
        {
            cestatotais lstcesta = CestaADO.RetornaTotaisCesta(cdcesta, datasource, schema);
            AxadoPost apv = new AxadoPost();
            apv.cep_destino = cepcliente;
            apv.cep_origem = ConfigurationManager.AppSettings.Get("AXADO_CEPORIGEM");
            apv.valor_notafiscal = lstcesta.vrtotalpedido.ToString("N2");
            //apv.prazo_adicional = "";
            //apv.preco_adicional = "";
            AxadoPostVolume apvv = new AxadoPostVolume();
            apvv.altura = "1";
            apvv.comprimento = "1";
            apvv.largura = "1";
            apvv.peso = lstcesta.pesototal.ToString("N2");
            apvv.preco = lstcesta.vrtotalpedido.ToString("N2");
            apvv.quantidade = lstcesta.totalitens.ToString();
            apvv.sku = "PED" + lstcesta.idpedido;

            //logparametro = "Peso: " + pPesoTotal.ToString("N2") + " valor total: " + pVrTotalPedido.ToString("N2");

            apv.volumes.Add(apvv);

            string jsoonenvio = string.Empty;
            string jsonRetorno = string.Empty;

            AxadoCotacoes axc = Axado.GetTabelaFrete(apv, out jsoonenvio, out jsonRetorno);

            pjsonretorno = jsonRetorno;

            //logparametro = logparametro + " JSON: " + jsoonenvio;

            return axc;
        }

        public static AxadoCotacoes RetornaTabelaCotacao(string pCepCliente, double pVrTotalPedido, double pPesoTotal, int pTotalItens, string pIdPedido, out string logparametro, out string jsonRetorno)
        {
            AxadoPost apv = new AxadoPost();
            apv.cep_destino = pCepCliente;
            apv.cep_origem = ConfigurationManager.AppSettings.Get("AXADO_CEPORIGEM");
            apv.valor_notafiscal = pVrTotalPedido.ToString("N2");
            //apv.prazo_adicional = "";
            //apv.preco_adicional = "";
            AxadoPostVolume apvv = new AxadoPostVolume();
            apvv.altura = "1";
            apvv.comprimento = "1";
            apvv.largura = "1";
            apvv.peso = pPesoTotal.ToString("N2");
            apvv.preco = pVrTotalPedido.ToString("N2");
            apvv.quantidade = pTotalItens.ToString();
            apvv.sku = "PED" + pIdPedido;

            logparametro = "Peso: " + pPesoTotal.ToString("N2") + " valor total: " + pVrTotalPedido.ToString("N2");

            apv.volumes.Add(apvv);

            string jsoonenvio = string.Empty;

            AxadoCotacoes axc = Axado.GetTabelaFrete(apv, out jsoonenvio, out jsonRetorno);

            logparametro = logparametro + " JSON: " + jsoonenvio;

            return axc;
        }
    }
}