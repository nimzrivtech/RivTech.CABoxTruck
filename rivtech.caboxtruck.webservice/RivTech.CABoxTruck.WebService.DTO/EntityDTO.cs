using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class EntityDTO
    {
        public Guid Id { get; set; }
        public bool IsIndividual { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string FullName { get { return $"{FirstName?.Trim()} {LastName?.Trim()}".Trim(); } }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string WorkPhone { get; set; }
        public string WorkPhoneExtension { get; set; }
        public string WorkFax { get; set; }
        public string PersonalEmailAddress { get; set; }
        public string WorkEmailAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string DriverLicenseNumber { get; set; }
        public string DriverLicenseState { get; set; }
        public DateTime DriverLicenseExpiration { get; set; }
        public byte? GenderID { get; set; }
        public byte? MaritalStatusID { get; set; }
        public byte? BusinessTypeID { get; set; }
        public string DBA { get; set; }
        public int YearEstablished { get; set; }
        public string FederalIDNumber { get; set; }
        public byte? NAICSCodeID { get; set; }
        public byte? AdditionalNAICSCodeID { get; set; }
        public string ICCMCDocketNumber { get; set; }
        public string USDOTNumber { get; set; }
        public string PUCNumber { get; set; }
        public string SocialSecurityNumber { get; set; }
        public bool HasSubsidiaries { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public BusinessTypeDTO BusinessType { get; set; }
        public ICollection<EntityAddressDTO> EntityAddresses { get; set; }

        public bool IsActive { get; set; }
    }
}
