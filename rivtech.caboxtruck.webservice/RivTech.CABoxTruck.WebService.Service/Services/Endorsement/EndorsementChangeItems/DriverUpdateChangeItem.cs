using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class DriverUpdateChangeItem : IEndorsementChangeItem
    {

        public  string Type => nameof(DriverUpdateChangeItem);

        public  string DisplayText => "";

        public Guid? Id { get; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? BirthDate { get; set; }
        public short? YrsDrivingExp { get; set; }
        public short? YrsCommDrivingExp { get; set; }
        public string State { get; set; }
        public string StateFull { get; set; }
        public string LicenseNumber { get; set; }
        public bool? IsMvr { get; set; }
        public string YearPoint { get; set; }
        public string AgeFactor { get; set; }
        public string TotalFactor { get; set; }
        public bool? IsOutOfState { get; set; }
        public DateTime? MvrDate { get; set; }
        public DateTime? HireDate { get; set; }
        public bool? isExcludeKO { get; set; }
        public string koReason { get; set; }
        public string Options { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsValidated { get; set; }
        public bool? IsValidatedKO { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public bool HasChanges()
        {
            return true; // TODO: checking if has changes
        }
    }
}
