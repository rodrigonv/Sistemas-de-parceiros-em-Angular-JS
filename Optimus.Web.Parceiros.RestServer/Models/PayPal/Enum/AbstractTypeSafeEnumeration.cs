using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Optimus.Web.Parceiros.Model.PayPal.Enum;

namespace Optimus.Web.Parceiros.Model.PayPal.Enum
{
    public abstract class AbstractTypeSafeEnumeration
    {
        protected readonly string name;
        protected readonly int value;

        protected AbstractTypeSafeEnumeration(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }
}