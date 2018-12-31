using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class pedidopagamento
    {


        public string codped { get; set; }

        public string valorpago { get; set; }

        public string formapagamento { get; set; }

        public string dataemissao { get; set; }

        public string datapagamento { get; set; }

        public string valortroco { get; set; }

        public string pago { get; set; }

        public string statusped { get; set; }

        public string alteravel { get; set; }

        public string parcelas { get; set; }

        public string cdformapag { get; set; }


        public string cdpag { get; set; }

        public string nrtitulo { get; set; }
        public pedidopagamento()
        {
        }
        public pedidopagamento(string pcodped,
            string pvalorpago, string pformapagamento, string pdataemissao, string pdatapagamento,
            string pvalortroco, string ppago, string pstatusped, string palteravel, string pparcelas,
            string pcdformapag, string pcdpag, string pnrtitulo
            )
        {
            codped = pcodped;
            valorpago = pvalorpago;
            formapagamento = pformapagamento;
            dataemissao = pdataemissao;
            datapagamento = pdatapagamento;
            valortroco = pvalortroco;
            pago = ppago;
            statusped = pstatusped;
            alteravel = palteravel;
            parcelas = pparcelas;
            cdpag = pcdpag;
            cdformapag = pcdformapag;
            nrtitulo = pnrtitulo;

        }
    }
}