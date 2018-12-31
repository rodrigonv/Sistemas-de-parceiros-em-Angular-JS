﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optimus.Web.Parceiros.Model.PayPal.Enum
{
    public class Ack : AbstractTypeSafeEnumeration
    {
        private static readonly Dictionary<string, Ack> ack = new Dictionary<string, Ack>();

        public static readonly Ack SUCCESS = new Ack("Success", 1);
        public static readonly Ack SUCCESS_WITH_WARNING = new Ack("SuccessWithWarning", 2);
        public static readonly Ack FAILURE = new Ack("Failure", 3);
        public static readonly Ack FAILURE_WITH_WARNING = new Ack("FailureWithWarning", 4);

        public Ack(string name, int value)
            : base(name, value)
        {
            ack["Success"] = SUCCESS;
            ack["SuccessWithWarning"] = SUCCESS_WITH_WARNING;
            ack["Failure"] = FAILURE;
            ack["FailureWithWarning"] = FAILURE_WITH_WARNING;
        }

        public static explicit operator Ack(string value)
        {
            Ack result;

            if (ack.TryGetValue(value, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException();
            }
        }
    }
}