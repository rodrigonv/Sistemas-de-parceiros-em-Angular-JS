using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class cliente
    {
        public cliente()
        {
            this.Enderecos = new List<clienteendereco>();
            this.Contatos = new List<clientecontato>();
        }

        public int cdentidade { get; set; }
        public string tipopessoa { get; set; }
        public string nome { get; set; }
        public string fantasia { get; set; }
        public string sexo { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string IE { get; set; }
        public string IM { get; set; }
        public string cdentiresp { get; set; }
        public string identidade { get; set; }

        public int excluir { get; set; }

        public List<clienteendereco> Enderecos { get; set; }

        public List<clientecontato> Contatos { get; set; }



    }

    public class clientesearch
    {
        public string nome { get; set; }
        public string cpfcnpj { get; set; }
    }

    public class clienteendereco
    {
        public string cdendereco { get; set; }
        public string cdtipoendereco { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string cidade { get; set; }
        public string logradouro { get; set; }
        public string bairro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string cdmunicipioibge { get; set; }
        public string cdcontato { get; set; }
        public string nmenderecoweb { get; set; }
        public string cdenderecoweb { get; set; }
        public string referencia { get; set; }
        public string nmtipoendereco { get; set; }
        public string principal { get; set; }

        public int excluir { get; set; }
    }

    public class clientecontato
    {
        public clientecontato() {

            this.Telefones = new List<clientecontatotelefone>();
            this.Emails = new List<clientecontatoemail>();
        }
        public string cdcontato { get; set; }
        public string cdtipocontato { get; set; }
        public string stdefault { get; set; }
        public string nome { get; set; }
        public string apelido { get; set; }
        public string observacao { get; set; }
        public string nmtipocontato { get; set; }
        public int excluir { get; set; }
        public int cdtelefoneresi { get; set; }
        
        public string dddresi { get; set; }

        public string telresidencial { get; set; }

        public int cdtelefonecel { get; set; }
        public string dddcel { get; set; }
        public string telcelular { get; set; }

        public int cdemail { get; set; }

        public string email { get; set; }

        public int idx { get; set; }

        public List<clientecontatoemail> Emails { get; set; }
        public List<clientecontatotelefone> Telefones { get; set; }
    }

    public class clientecontatoemail
    {
        public string cdemail { get; set; }
        public string cdcontato { get; set; }
        public string cdtipoemail { get; set; }

        public string txemail { get; set; }
        public string nmtipoemail { get; set; }
        public int excluir { get; set; }

        public int idx { get; set; }
    }

    public class clientecontatotelefone
    {
        public string cdtelefone { get; set; }

        public string cdcontato { get; set; }
        public string cdtipotelefone { get; set; }

        public string ddd { get; set; }

        public string telefone { get; set; }
        public string nmtipotelefone { get; set; }
        public int excluir { get; set; }

        public int idx { get; set; }
    }

    public class ContatoRet 
    {
        public string nmfull { get; set; }
        public string cdentidade { get; set; }
        public string cdcontato { get; set; }
        public string nmcontato { get; set; }
        public string nmapelido { get; set; }
        public string stdefault { get; set; }
        public string txobservacao { get; set; }
        public string cdtipocontato { get; set; }
        public string nmtipocontato { get; set; } 
    }
}