using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Util
{
    public class Formatar
    {
        public static string FormataValorDecimal(string valor)
        {
            double v = 0;
            string ret = string.Empty;
            try
            {
                v = Convert.ToDouble(valor);
                ret ="R$ " + Convert.ToDouble(valor).ToString("N2");
            }
            catch
            {
                ret = "R$ 0,00";
            }
            return ret;
        }
    }
}