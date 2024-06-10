using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.QuoteReport
{
    public class QuoteTableEntry<T>
    {
        public string Description { get; set; }

        public T Option1Value { get; set; }

        public T Option2Value { get; set; }

        public T Option3Value { get; set; }
    }
}
