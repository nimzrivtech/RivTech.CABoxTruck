using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Vehicles
{
    public class RiskDetailVehicle : ICreatedData, IUpdatedData
    {
        protected RiskDetailVehicle() { }


        public RiskDetailVehicle(
            Guid riskDetailId,
            Guid vehicleId
        )
        {
            RiskDetailId = riskDetailId;
            VehicleId = vehicleId;
            CreatedDate = DateTime.Now;
        }

        public RiskDetailVehicle(
            RiskDetail riskDetail,
            Vehicle vehicle
        )
        {
            RiskDetailId = riskDetail.Id;
            RiskDetail = riskDetail;
            VehicleId = vehicle.Id;
            Vehicle = vehicle;
            CreatedDate = DateTime.Now;
        }

        public Guid RiskDetailId { get; set; }
        public Guid VehicleId { get; set; }
        public bool IsSkipped { get; set; } = false;

        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public RiskDetail RiskDetail { get; set; }
        public Vehicle Vehicle { get; set; }

        public RiskDetailVehicle WithNewVehicle(Vehicle newVehicle)
        {
            return new RiskDetailVehicle
            {
                RiskDetail = RiskDetail,
                RiskDetailId = RiskDetailId,
                Vehicle = newVehicle,
                VehicleId = newVehicle.Id
            };
        }

        /// <summary>
        /// Returns new instance with new RiskDetail.
        /// </summary>
        public RiskDetailVehicle WithNewRiskDetail(RiskDetail newRiskDetail)
        {
            var clone = (RiskDetailVehicle) this.MemberwiseClone();
            clone.RiskDetail = newRiskDetail;
            clone.RiskDetailId = newRiskDetail.Id;
            return clone;
        }

        public void ReplaceVehicle(Vehicle newVehicle)
        {
            VehicleId = newVehicle.Id;
            Vehicle = Vehicle;
        }
    }
}
