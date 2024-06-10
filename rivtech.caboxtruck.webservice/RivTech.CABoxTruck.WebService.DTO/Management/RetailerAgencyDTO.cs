using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RetailerAgencyDTO
    {
        public Guid Id { get; set; }

        public Guid RetailerId { get; set; }
        public Guid AgencyId { get; set; }
    }
}