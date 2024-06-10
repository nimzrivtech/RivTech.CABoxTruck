using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Exceptions
{
    public class OutOfSequenceException : Exception
    {
        public OutOfSequenceException()
        {
        }

        public OutOfSequenceException(string message) : base(message)
        {
        }
    }
}
