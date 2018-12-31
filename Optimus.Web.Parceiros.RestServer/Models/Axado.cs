using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class Axado
    {
        private static string apiUrl = "http://api.axado.com.br";
        private static string Token { get { return ConfigurationManager.AppSettings.Get("TOKEN_AXADO"); } }
        public static AxadoCotacoes GetTabelaFrete(AxadoPost ap, out string JSONEnvio, out string JSONRetorno)
        {
            AxadoCotacoes retorno = new AxadoCotacoes();

            string Json = JsonConvert.SerializeObject(ap, Formatting.None);
            JSONEnvio = Json;
            var request = (HttpWebRequest)WebRequest.Create(apiUrl + "/v2/consulta/?token=" + Token);
            request.Timeout = 600000;
            request.Method = "POST";
            request.ContentType = "application/json";
            using (var requestStream = request.GetRequestStream())
            {
                var bytes = Encoding.UTF8.GetBytes(Json);
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }
            var result = String.Empty;
            using (
                var response = (HttpWebResponse)request.GetResponse()
                )
            using (
                var stream = response.GetResponseStream()
                )
            using (var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            JSONRetorno = result;
            retorno = Newtonsoft.Json.JsonConvert.DeserializeObject<AxadoCotacoes>(result);
            return retorno;

        }
    }
  
    public class AxadoResponse
    {
        public AxadoResponse()
        {
            this.cotacoes = new AxadoCotacoes();
        }
        public string consulta_token { get; set; }
        public AxadoCotacoes cotacoes { get; set; }
    }
   
    public class AxadoCotacao
    {
        /*
         <cotacao_codigo>2</cotacao_codigo>
         <cotacao_custo>21,00</cotacao_custo>
         <cotacao_prazo>1</cotacao_prazo>
         <cotacao_preco>25,19</cotacao_preco>
         <servico_logo>/media/uploads/transportadoras/logos/correios_sedex.png</servico_logo>
         <servico_metaname>correios-sedex</servico_metaname>
         <servico_nome>Correios - SEDEX</servico_nome>
         <servico_transporte>1</servico_transporte>
         <transportadora_metaname>correios</transportadora_metaname>

         */

        public string cotacao_codigo { get; set; }
        public string cotacao_custo { get; set; }
        public string cotacao_prazo { get; set; }
        public string cotacao_preco { get; set; }
        public string servico_logo { get; set; }
        public string servico_metaname { get; set; }
        public string servico_nome { get; set; }
        public string servico_transporte { get; set; }
        public string transportadora_metaname { get; set; }


    }
    public class AxadoCotacoes
    {
        public string consulta_token { get; set; }
        public List<AxadoCotacao> Cotacoes { get; set; }

        public string CotacoesRetorno()
        {
            StringBuilder retorno = new StringBuilder();
            retorno.AppendLine("------------------------------------------------------------------------------------------------------------");
            retorno.AppendLine("consulta_token: " + this.consulta_token);
            foreach (var i in Cotacoes)
            {
                retorno.AppendLine("cotacao_codigo: " + i.cotacao_codigo);
                retorno.AppendLine("cotacao_custo: " + i.cotacao_custo);
                retorno.AppendLine("cotacao_prazo: " + i.cotacao_prazo);
                retorno.AppendLine("cotacao_preco: " + i.cotacao_preco);
                retorno.AppendLine("servico_metaname: " + i.servico_metaname);
                retorno.AppendLine("servico_nome: " + i.servico_nome);
                retorno.AppendLine("servico_transporte: " + i.servico_transporte);
                retorno.AppendLine("transportadora_metaname: " + i.transportadora_metaname);
            }
            retorno.AppendLine("------------------------------------------------------------------------------------------------------------");
            return retorno.ToString();
        }
    }

    public class AxadoPost
    {
        public AxadoPost()
        {
            this.volumes = new List<AxadoPostVolume>();
        }
        public string cep_origem { get; set; }
        public string cep_destino { get; set; }
        public string valor_notafiscal { get; set; }
        public string prazo_adicional { get; set; }
        public string preco_adicional { get; set; }
        public List<AxadoPostVolume> volumes { get; set; }
    }
    public class AxadoPostVolume
    {
        public string sku { get; set; }
        public string quantidade { get; set; }
        public string preco { get; set; }
        public string altura { get; set; }
        public string comprimento { get; set; }
        public string largura { get; set; }
        public string peso { get; set; }
    }
    public class AxadoPostRest
    {
        public string CepCliente { get; set; }
        public double VrTotalPedido { get; set; }
        public double PesoTotal { get; set; }
        public int TotalItens { get; set; }
        public string IdPedido { get; set; }
    }
}