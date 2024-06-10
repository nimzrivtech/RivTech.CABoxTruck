using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class BusinessDetailsDTO
    {
        public Guid RiskDetailId { get; set; }
        public Guid EntityId { get; set; }
        public bool IsNewVenture { get; set; }
        public short? NewVenturePreviousExperienceId { get; set; }
        public short? YearEstablished { get; set; }
        public short? YearsInBusiness { get; set; }
        public short? FirstYearUnderCurrentManagement { get; set; }
        public short? YearsUnderCurrentManagement { get; set; }
        public short? BusinessTypeId { get; set; }
        public short? MainUseId { get; set; }
        public short? SecondaryBusinessGroupId { get; set; }
        public short? AccountCategoryId { get; set; }
        public short? MainGVWId { get; set; }
        public short? OrganizationTypeId { get; set; }
        public short? AccountCategoryUWId { get; set; }
        public string DescOperations { get; set; }
        public string FederalIDNumber { get; set; }
        public string NAICSCode { get; set; }
        public string PUCNumber { get; set; }
        public string SocialSecurityNumber { get; set; }
        public bool HasSubsidiaries { get; set; }
        public bool HasCompanies { get; set; }

        public List<EntitySubsidiaryDTO> Subsidiaries { get; set; }
        public List<EntitySubsidiaryDTO> Companies { get; set; }
    }

    public enum AddressTypesEnum
    {
        Billing = 1,
        Business = 2,
        Garaging = 3,
        Mailing = 4,
        Shipping = 5
    }
}
