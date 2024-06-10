using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class AddressDTO
    {
        public Guid Id { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string ZipCode { get; set; }
        public string ZipCodeExt { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string County { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public bool IsMainGarage { get; set; }
        public bool IsGarageIndoor { get; set; }
        public bool IsGarageOutdoor { get; set; }
        public bool IsGarageFenced { get; set; }
        public bool IsGarageLighted { get; set; }
        public bool IsGarageWithSecurityGuard { get; set; }
    }
}