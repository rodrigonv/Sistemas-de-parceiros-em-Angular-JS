using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class ObjEmail
    {

        public ObjEmail() 
        {
            CC = new List<string>();
        }

        public string SMTP { get; set; }
        public int PORTA { get; set; }
        public bool ENABLESSL { get; set; }
        public string USUARIO { get; set; }
        public string SENHA { get; set; }
        public string TO { get; set; }
        public string SUBJECT { get; set; }
        public string BODY { get; set; }
        public List<string> CC { get; set; }
    }
}
