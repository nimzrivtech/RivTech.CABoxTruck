using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class CarrierDTO
    {
        public Int32 CarrierID { get; set; }
        public string CompanyName { get; set; }
        public Int32 BusinessAddressID { get; set; }
        public Int32 MailingAddressID { get; set; }
        public Int32 GaragingAddressID { get; set; }
        public string WorkPhone { get; set; }
        public string WorkFax { get; set; }
        public string WorkEmailAddress { get; set; }
        public Int16 BusinessTypeID { get; set; }
        public string DBA { get; set; }
        public string Website { get; set; }
        public Int16 IsActive { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}

