using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class Pedido
    {
        public Pedido()
        {
            this.Itens = new List<PedidoItem>();
            this.Status = new List<PedidoStatus>();
        }
        public string cdpedoptimus { get; set; }
        public string cdpedweb { get; set; }
        public string vrfrete { get; set; }
        public string vrtotal { get; set; }
        public string vrdesconto { get; set; }
        public string vrliquido { get; set; }
        public string codobjcorreio { get; set; }
        public string formpagamento { get; set; }
        public string parcelas { get; set; }
        public string statuspedido { get; set; }
        public string dtpedido { get; set; }

        public string urlnota { get; set; }

        public string statusrastreio { get; set; }
        public List<PedidoItem> Itens { get; set; }
        public List<PedidoStatus> Status { get; set; }
    }

    public class PedidoItem
    {
        public string cdpedoptimus { get; set; }

        public int quantidade { get; set; }

        public string vrunitario { get; set; }

        public string cdproduto { get; set; }

        public string nmproprodduto { get; set; }

        public string ean { get; set; }

        public string vrtotal { get; set; }
    }

    public class PedidoStatus
    {

        public string cdpedoptimus { get; set; }
        public string cdpedweb { get; set; }
        public string nmstatus { get; set; }
        public string cdstatuspedido { get; set; }

        public string statussite { get; set; }

        public string datastatus { get; set; }
    }

    public class PedidoConsulta
    {
        public string cdentifilial { get; set; }
        public string cdpedido { get; set; }
        public string nmentidade { get; set; }
        public string cdstatpedido { get; set; }
        public string quantidade { get; set; }
        public double valortotal { get; set; }
        public string nmstatus { get; set; }
        public string dataped { get; set; }
        public string cpfcnpj { get; set; }
        public string statusnfe { get; set; }
        public string flalteravel { get; set; }
        public string cdnotafiscalsaida { get; set; }

        public string epai { get; set; }
        public string cdpai { get; set; }
        public string nmpai { get; set; }

        public double comissao { get; set; }
        public double over { get; set; }
        public double totalcomissao { get; set; }

        //public string cdstatuspedido { get; set; }
    }

    public class PedidoUpdateFrete
    {
        public string cdpedido { get; set; }
        public double vrfrete { get; set; }
        public string txtipofrete { get; set; }
        public string tokenaxado { get; set; }

        public string cdenderecoentregra { get; set; }
    }
    public class PedidoPaypal
    {
        public string cdproduto { get; set; }
        public string codped { get; set; }
        public string nmproduto { get; set; }
        public double preco { get; set; }
        public double quantidade { get; set; }
        public double frete { get; set; }

        public string txfrete { get; set; }

        public string cdenderecoentrega { get; set; }

        public string cdcliente { get; set; }

        public double comissao { get; set; }
        public double totalcomissao { get; set; }
        public double over { get; set; }
        
    }

    public class PedidoPesquisa
    {

        public string codigopedido { get; set; }
        public string datainicio { get; set; }
        public string datafim { get; set; }
        public string cpfcnpj { get; set; }
        public string statuspedido { get; set; }
        public string cdforn { get; set; }

        public string nome { get; set; }

        public string perfil { get; set; }

        public string cdpai { get; set; }

    }
    public class RetornoGenericoValue
    {
        public string Nome { get; set; }
        public string Valor { get; set; }

        public RetornoGenericoValue(string pNome, string pValor)
        {
            this.Nome = pNome;
            this.Valor = pValor;
        }
        public RetornoGenericoValue()
        {

        }

    }
}