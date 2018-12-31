using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Models
{
    public class Usuario
    {
        /*
              PROCEDURE SP_USUARIO
    ( P_EMAIL IN VARCHAR2,
      P_IDCEL IN VARCHAR2,
      P_CDENTIFILIAL IN NUMBER,
      P_SO IN VARCHAR2,
      P_SENHA IN VARCHAR2,
      P_CDAPP IN NUMBER
)*/

        public Usuario()
        {
            this.cliente = new cliente();
        }
        public string cdentidade { get; set; }
        public string email { get; set; }
        public string idcel { get; set; }
        public string cdentifilial { get; set; }
        public string so { get; set; }

        public string senha { get; set; }

        public string cdapp { get; set; }
        //public DateTime ultimologin { get; set; }

        public string nome { get; set; }
        //public string sobrenome { get; set; }

        public string status { get; set; }
        public cliente cliente { get; set; }
    }

    public class UsuarioPost
    {
        public string nomeuser { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }
        public string cdforn { get; set; }
        public string cdusuariooptimus { get; set; }
        public int cdfuncionario { get; set; }
    }

    public class UsuarioRetorno
    {
        public int cdfuncionario { get; set; }
        public string cdentidadepai { get; set; }
        public string nmfuncionario { get; set; }
        public string cdusuariooptimus { get; set; }
        public string perfil { get; set; }

        public bool Logado { get; set; }

        public string dtcadastro { get; set; }

        public string dsemail { get; set; }

        public string senha { get; set; }
    }
}