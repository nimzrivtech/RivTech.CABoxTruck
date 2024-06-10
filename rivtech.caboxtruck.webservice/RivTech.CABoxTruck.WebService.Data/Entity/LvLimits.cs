using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class LvLimits : BaseEntity<int>, IIsActive
    {
        public const int CollisionNoCoverageId = 65;
        public const int ComprehensiveNoCoverageId = 76;
        public const int GLNoneCoverageId = 87;
        public const int CargoNoneCoverageId = 53;
        public const int RefNoneCoverageId = 157;

        public LvLimits()
        {
            this.IsActive = true;
        }

        public string LimitDisplay { get; set; }
        public string LimitValue { get; set; }
        public string SingleLimit { get; set; }
        public string PerPerson { get; set; }
        public string PerAccident { get; set; }
        public string StateCode { get; set; }
        public string LimitType { get; set; }
        public string RaterVersionId { get; set; }
        public bool IsActive { get; set; }
        public string Deductible { get; set; }
        public string Description { get; set; }
    }
}
