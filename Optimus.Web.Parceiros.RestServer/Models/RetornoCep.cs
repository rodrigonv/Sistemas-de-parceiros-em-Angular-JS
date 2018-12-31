using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class RetornoCep
    {
        public string Endereco { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Uf { get; set; }


        public string Cep { get; set; }

        public string CdmunicipioIbge { get; set; }
        public RetornoCep()
        {
        }
        public RetornoCep(string pendereco,
                       string pbairro, string pcidade,
                       string puf, string pcep, string pCdmunicipioIbge)
        {
            Endereco = pendereco;
            Bairro = pbairro;
            Cidade = pcidade;
            Uf = puf;
            Cep = pcep;
            CdmunicipioIbge = pCdmunicipioIbge;
        }

    }
}