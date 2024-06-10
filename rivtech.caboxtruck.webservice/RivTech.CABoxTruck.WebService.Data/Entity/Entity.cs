using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Entity : BaseEntity<Guid>, ICreatedData, IUpdatedData
    {
        public Entity()
        {
            CreatedDate = DateTime.Now;
            IsActive = true;
        }
        public bool IsIndividual { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string FullName { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string WorkPhone { get; set; }
        public string WorkPhoneExtension { get; set; }
        public string WorkFax { get; set; }
        public string PersonalEmailAddress { get; set; }
        public string WorkEmailAddress { get; set; }
        public string BusinessPrincipal { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public string DriverLicenseNumber { get; set; }
        public string DriverLicenseState { get; set; }
        public DateTime? DriverLicenseExpiration { get; set; }
        public short? GenderID { get; set; }
        public short? MaritalStatusID { get; set; }
        public short? BusinessTypeID { get; set; }
        public string DBA { get; set; }
        public short? YearEstablished { get; set; }
        public string FederalIDNumber { get; set; }
        public string NAICSCode { get; set; }
        public short? AdditionalNAICSCodeID { get; set; }
        public string ICCMCDocketNumber { get; set; }
        public string USDOTNumber { get; set; }
        public string McNumber { get; set; }
        public string PUCNumber { get; set; }
        public string SocialSecurityNumber { get; set; }
        public bool HasSubsidiaries { get; set; }
        public bool HasCompanies { get; set; }
        public string DescOperations { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public Insured Insured { get; set; }
        public List<EntityAddress> EntityAddresses { get; set; }

        public Entity CloneWithNewId()
        {
            var clone = (Entity)MemberwiseClone(); 
            clone.Id = Guid.Empty;
            return clone;
        }
    }
}
