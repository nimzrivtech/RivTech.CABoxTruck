using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class NameAndAddressDTO
    {
        public Guid? RiskDetailId { get; set; }
        public Guid? RiskId { get; set; }
        public string SubmissionNumber { get; set; } //Temporary generated from frontend, to be replaced by backend generation
        public string PolicyNumber { get; set; }
        public string BusinessName { get; set; }
        public string InsuredDBA { get; set; }
        public string BusinessAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CityOperations { get; set; }
        public string StateOperations { get; set; }
        public string ZipCodeOperations { get; set; }
        public string County { get; set; }
        public string BusinessPrincipal { get; set; }
        public string Email { get; set; }
        public string PhoneExt { get; set; }
        public List<EntityAddressDTO> Addresses { get; set; }
        public bool IsRenewed { get; set; }
        public bool IsRenewal { get; set; }
        public string PreviousPolicyNumber { get; set; }

        public List<FieldVersionDTO> FieldVersions { get; set; }

    }
}
