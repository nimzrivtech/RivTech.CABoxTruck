using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Exceptions
{
    public class BillingException : Exception
    {
        public BillingException(string msg) : base(msg)
        {
        }
    }
}
