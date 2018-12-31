using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Text;

namespace Optimus.Web.Parceiros.Model.PayPal.Nvp
{
    public class NVP : NameValueCollection
    {
        public NVP() : base() { }

        public bool GetBool(int index)
        {
            return GetBool(GetKey(index));
        }
        public bool GetBool(string name)
        {
            string value = Get(name);
            return value != null && (value.Equals("1") || value.Equals("true"));
        }

        public double GetDouble(int index)
        {
            return GetDouble(GetKey(index));
        }
        public double GetDouble(string name)
        {
            string value = Get(name);
            return value == null ? 0 : double.Parse(value, CultureInfo.InvariantCulture);
        }
        public int GetInt(int index)
        {
            return GetInt(GetKey(index));
        }

        public int GetInt(string name)
        {
            string value = Get(name);
            return value == null ? 0 : int.Parse(value);
        }

        public static explicit operator NVP(string queryString)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(queryString);
            NVP nvp = new NVP();

            for (int i = 0, t = nvc.Count ; i < t ; ++i)
            {
                nvp.Set(nvc.GetKey(i), nvc[i]);
            }
            return nvp;
        }

        public void Set(string name, bool value)
        {
            Set(name, value ? "true" : "false");        
        }
        public void Set(string name, double value)
        {
            Set(name, value.ToString(CultureInfo.InvariantCulture));
        }

        public void Set(string name, int value)
        {
            Set(name, value.ToString());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            Console.WriteLine("----------------------<ToString>-----------------------");
            for (int i = 0, t = this.Count; i < t ; i++)
            {
                if (i != 0)
                {
                    sb.Append("&");
                }
                Console.WriteLine(GetKey(i) + " = " + this[i]);

                sb.Append(GetKey(i));
                sb.Append("=");
                sb.Append(HttpUtility.UrlEncode(this[i]));
            }
            Console.WriteLine(sb.ToString());
            Console.WriteLine("----------------------<ToString>-----------------------");
            return sb.ToString();
        }

    }
}