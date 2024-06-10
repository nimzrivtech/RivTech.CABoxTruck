using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class SLNumberDTO
    {
        public int Id { get; set; }
        public short CreditedOfficeId { get; set; }
        public string StateCode { get; set; }
        public string LicenseNumber { get; set; }
        public bool IsActive { get; set; }
        public string ProducerSLNumber { get; set; }
        public string SLBroker { get; set; }
        public string SLBrokerAddressLineOne { get; set; }
        public string SLBrokerAddressCityStateZip { get; set; }
        public string ProducerName { get; set; }
    }
}
