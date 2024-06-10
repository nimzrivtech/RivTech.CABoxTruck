using System;
using System.Collections;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Service.Exceptions
{
    public class RaterException : Exception
    {
        public IEnumerable<string> Errors { get; set; }

        public RaterException(string msg) : base(msg)
        {
            Errors = new[] { msg };
        }

        public RaterException(IEnumerable<string> errors)
        {
            Errors = errors;
        }
    }
}
