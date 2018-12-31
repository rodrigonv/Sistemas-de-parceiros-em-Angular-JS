using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class cesta
    {

        public string codped { get; set; }


        public string codprod { get; set; }

        public string nomeprod { get; set; }

        public string codcli { get; set; }

        public double preco { get; set; }

        public string desconto { get; set; }

        public string estoque { get; set; }

        public double valortotal { get; set; }

        public int quantpedida { get; set; }

        public string quantfalta { get; set; }

        public int quantfaturada { get; set; }

        public string zcodpedgravado { get; set; }

        public string zzzzCdEnd { get; set; }

        public string zpromocao { get; set; }

        public string zzpcmargem { get; set; }

        public string TotalCesta { get; set; }

        public string TotalDescontoCesta { get; set; }

        public string QuantidadeTotalCesta { get; set; }

        public string codOperador { get; set; }

        public string codempresa { get; set; }

        public double precovenda { get; set; }

        public string datapedido { get; set; }

        public string precoview { get; set; }

        public string valortotalview { get; set; }

        public string percentualcomissao { get; set; }

        public double percentualcomissaocalc { get; set; }
        public double precomaximo { get; set; }
        public double precominio { get; set; }
        public double over { get; set; }
        public double totalcomissao { get; set; }
       
        public cesta()
        {
        }

        public cesta(string pcodped, string pcodprod, string pnomeprod, string pcodcli, string ppreco,
                     string pdesconto, string pestoque, string pvalortotal,
            string pquantpedida, string pquantfalta, string pquantfaturada,
            string pzcodpedgravado, string pzzzzCdEnd, string ppromocao, string ppcmargem, string pcodOperador, string ppercentualcomissao, string ppercentualcomissaocalc, string pcodempresa = "", string pprecovenda = "", string pdatapedido = "", string pprecoview = "", string pvalortotalview = "",
            string pQuantidadeTotalCesta = "", string pTotalCesta = "", string pTotalDescontoCesta = "", string pprecomaximo = "", string pprecominio = "", double pover = 0, double ptotalcomissao = 0)
        {
            codped = pcodped;
            codprod = pcodprod;
            nomeprod = pnomeprod;
            codcli = pcodcli;
            preco = Convert.ToDouble(ppreco);
            desconto = pdesconto;
            estoque = pestoque;
            valortotal = Convert.ToDouble(pvalortotal);
            quantpedida = Convert.ToInt32(pquantpedida);
            quantfalta = pquantfalta;
            quantfaturada = Convert.ToInt32(pquantfaturada);
            zcodpedgravado = pzcodpedgravado;
            zzzzCdEnd = pzzzzCdEnd;
            zpromocao = ppromocao;
            zzpcmargem = ppcmargem;
            codOperador = pcodOperador;
            codempresa = pcodempresa;
            precovenda = Convert.ToDouble(pprecovenda);
            datapedido = pdatapedido;
            precoview = pprecoview;
            valortotalview = pvalortotalview;
            QuantidadeTotalCesta = pQuantidadeTotalCesta;
            TotalCesta = pTotalCesta;
            TotalDescontoCesta = pTotalDescontoCesta;
            percentualcomissao = ppercentualcomissao;
            percentualcomissaocalc = preco == Convert.ToDouble(pprecominio) ? 0 : Convert.ToDouble(ppercentualcomissaocalc);
            precomaximo = Convert.ToDouble(pprecomaximo);
            precominio = Convert.ToDouble(pprecominio);
            over = Convert.ToDouble(pover);
            totalcomissao = Convert.ToDouble(ptotalcomissao);
        }
    }

    public class cestapost
    {
        public cestapost()
        {

        }
        public string codempresa { get; set; }
        public string codped { get; set; }
        public string cliente { get; set; }
        public string[] produ { get; set; }
        public string finaliza { get; set; }
        public string operador { get; set; }
        public string codend { get; set; }
        public string preco { get; set; }
        public string subsquant { get; set; }
        public string codforn { get; set; }
    }

    public class cestatotais
    {
        public string idpedido { get; set; }
        public double vrtotalpedido { get; set; }
        public double pesototal { get; set; }
        public double totalitens { get; set; }
        public string codproduto { get; set; }


    }
}