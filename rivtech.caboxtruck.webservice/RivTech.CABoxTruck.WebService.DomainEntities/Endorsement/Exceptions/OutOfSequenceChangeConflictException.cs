using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Exceptions
{
    public class OutOfSequenceChangeConflictException : OutOfSequenceException
    {
        public string oosType { get; set; }
        public IEnumerable<string> Errors { get; private set; }

        public OutOfSequenceChangeConflictException(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public OutOfSequenceChangeConflictException(string message, string type = null) : base(message)
        {
            this.oosType = type;
            Errors = new[] { message };
        }
    }
}
