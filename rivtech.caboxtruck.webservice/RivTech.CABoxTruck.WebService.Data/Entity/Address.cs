using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Address : BaseEntity<Guid>, ICreatedData, IUpdatedData
    {
        public Address()
        {
            this.CreatedDate = DateTime.Now;
        }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ZipCodeExt { get; set; }
        public string County { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public bool IsMainGarage { get; set; }
        public bool IsGarageIndoor { get; set; }
        public bool IsGarageOutdoor { get; set; }
        public bool IsGarageFenced { get; set; }
        public bool IsGarageLighted { get; set; }
        public bool IsGarageWithSecurityGuard { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid UniqueId { get; set; }

        [NotMapped]
        public string StateCode { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Address;
            return other != null
                && StreetAddress1 == other.StreetAddress1
                && StreetAddress2 == other.StreetAddress2
                && City == other.City
                && State == other.State
                && ZipCode == other.ZipCode
                && ZipCodeExt == other.ZipCodeExt
                && County == other.County
                && Longitude == other.Longitude
                && Latitude == other.Latitude
                && IsMainGarage == other.IsMainGarage
                && IsGarageIndoor == other.IsGarageIndoor
                && IsGarageOutdoor == other.IsGarageOutdoor
                && IsGarageFenced == other.IsGarageFenced
                && IsGarageLighted == other.IsGarageLighted
                && IsGarageWithSecurityGuard == other.IsGarageWithSecurityGuard;
        }
    }
}
