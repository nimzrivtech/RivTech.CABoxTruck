using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Validators
{
    public class OutOfSequenceValidationException : Exception
    {
        public OutOfSequenceValidationException(string message) : base(message)
        {
        }
    }
}
