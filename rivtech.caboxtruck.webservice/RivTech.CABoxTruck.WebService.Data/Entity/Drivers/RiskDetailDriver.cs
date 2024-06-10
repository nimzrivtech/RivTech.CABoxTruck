using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Drivers
{
    public class RiskDetailDriver : ICreatedData, IUpdatedData
    {
        protected RiskDetailDriver() { }

        public RiskDetailDriver(
            Guid riskDetailId,
            Guid driverId
        )
        {
            RiskDetailId = riskDetailId;
            DriverId = driverId;
        }

        public RiskDetailDriver(
            RiskDetail riskDetail,
            Driver driver
        )
        {
            RiskDetail = riskDetail;
            RiskDetailId = riskDetail.Id;
            Driver = driver;
            DriverId = driver.Id;
        }

        public Guid RiskDetailId { get; set; }
        public Guid DriverId { get; set; }
        public bool IsSkipped { get; set; } = false;

        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public RiskDetail RiskDetail { get; internal set; }
        public Driver Driver { get; internal set; }

        public RiskDetailDriver WithNewRiskDetail(RiskDetail newRiskDetail)
        {
            var clone = (RiskDetailDriver)this.MemberwiseClone();
            clone.RiskDetail = newRiskDetail;
            clone.RiskDetailId = newRiskDetail.Id;
            return clone;
        }
    }
}
