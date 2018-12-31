using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Optimus.Web.Parceiros.Model.PayPal.Enum;

namespace Optimus.Web.Parceiros.Model.PayPal.ExpressCheckout
{
    public class GetExpressCheckoutDetailsOperation : ExpressCheckoutApi.Operation
    {
        public GetExpressCheckoutDetailsOperation(ExpressCheckoutApi ec, string token)
            : base(ec)
        {
            RequestNVP.Method = "GetExpressCheckoutDetails";
            Token = token;
        }
    }
}
