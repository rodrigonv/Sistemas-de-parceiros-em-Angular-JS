using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Optimus.Web.Parceiros.Model.PayPal.Enum;

namespace Optimus.Web.Parceiros.Model.PayPal.ExpressCheckout
{
    public class DoExpressCheckoutPaymentOperation : ExpressCheckoutApi.OperationWithPaymentRequest
    {
        public DoExpressCheckoutPaymentOperation(ExpressCheckoutApi ec, string token, string payerid, PaymentAction action)
            : base(ec)
        {
            RequestNVP.Method = "DoExpressCheckoutPayment";
            Token = token;
            PayerId = payerid;

            PaymentRequest(0).Action = action;
        }

        public string PayerId
        {
            get { return RequestNVP.Get("PAYERID"); }
            set { RequestNVP.Set("PAYERID", value); }
        }

        public string GiftMessage
        {
            get { return RequestNVP.Get("GIFTMESSAGE"); }
            set { RequestNVP.Set("GIFTMESSAGE", value); }
        }

        public bool GiftReceiptEnable
        {
            get { return RequestNVP.GetBool("GIFTRECEIPTENABLE"); }
            set { RequestNVP.Set("GIFTRECEIPTENABLE", value); }
        }

        public double GiftWrapAmount
        {
            get { return RequestNVP.GetDouble("GIFTWRAPAMOUNT"); }
            set { RequestNVP.Set("GIFTWRAPAMOUNT", value); }
        }

        public string GiftWrapName
        {
            get { return RequestNVP.Get("GIFTWRAPNAME"); }
            set { RequestNVP.Set("GIFTWRAPNAME", value); }
        }

        public bool ReturnFmfDetails
        {
            get { return RequestNVP.GetBool("RETURNFMFDETAILS"); }
            set
            {
                RequestNVP.Set("RETURNFMFDETAILS", value ? "1" : "0");
            }
        }
    }
}