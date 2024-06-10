using System;

namespace RivTech.CABoxTruck.WebService.Service.Exceptions
{
    public class ModelNotFoundException : Exception
    {
        public ModelNotFoundException(string msg = "Not found.") : base(msg)
        {
        }
    }
}
