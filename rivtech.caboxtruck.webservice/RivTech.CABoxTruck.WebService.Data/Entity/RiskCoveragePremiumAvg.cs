using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RiskCoveragePremiumAvg : BaseEntity<Guid>
    {
        public RiskCoveragePremiumAvg()
        {
            this.EffectiveDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now.AddYears(5);
            this.AddProcessDate = DateTime.Now;
        }
        public Guid RiskCoveragePremiumId { get; set; }
        public decimal AvgAlVehicleTotalPremium { get; set; }
        public decimal AvgAlVehiclePowerPremium { get; set; }
        public decimal AvgAlVehicleTrailerOtherPremium { get; set; }

        public decimal AvgApdVehicleTotalPremium { get; set; }
        public decimal AvgApdVehiclePowerPremium { get; set; }
        public decimal AvgApdVehicleTrailerOtherPremium { get; set; }

        public decimal AvgMtcRefVehicleTotalPremium { get; set; }
        public decimal AvgMtcRefVehiclePowerPremium { get; set; }
        public decimal AvgMtcRefVehicleTrailerOtherPremium { get; set; }

        public decimal AvgTotalVehiclePremium { get; set; }
        public decimal AvgTotalVehiclePowerPremium { get; set; }
        public decimal AvgTotalVehicleTrailerOtherPremium { get; set; }

        public decimal NumberOfVehicles { get; set; }
        public decimal NumberOfPowerUnits { get; set; }
        public decimal NumberOfTrailerOthers { get; set; }

        public decimal AlVehicleTotalPremium { get; set; }
        public decimal AlVehiclePowerPremium { get; set; }
        public decimal AlVehicleTrailerOtherPremium { get; set; }

        public decimal ApdVehicleTotalPremium { get; set; }
        public decimal ApdVehiclePowerPremium { get; set; }
        public decimal ApdVehicleTrailerOtherPremium { get; set; }

        public decimal MtcRefVehicleTotalPremium { get; set; }
        public decimal MtcRefVehiclePowerPremium { get; set; }
        public decimal MtcRefVehicleTrailerOtherPremium { get; set; }

        public decimal TotalVehiclePremium { get; set; }
        public decimal TotalVehiclePowerPremium { get; set; }
        public decimal TotalVehicleTrailerOtherPremium { get; set; }
        public decimal TotalAIPremium { get; set; }
        public decimal TotalManuPremium { get; set; }

        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime AddProcessDate { get; set; }
        public Int64 AddedBy { get; set; }
        public DateTime? RemoveProcessDate { get; set; }
        public Int64? RemovedBy { get; set; }

        public void CalculateAveragePremiums()
        {
            AvgAlVehicleTotalPremium = AlVehicleTotalPremium != 0 && NumberOfVehicles > 0 ? AlVehicleTotalPremium / NumberOfVehicles : 0;
            AvgAlVehiclePowerPremium = AlVehiclePowerPremium != 0 && NumberOfPowerUnits > 0 ? AlVehiclePowerPremium / NumberOfPowerUnits : 0;
            AvgAlVehicleTrailerOtherPremium = AlVehicleTrailerOtherPremium != 0 && NumberOfTrailerOthers > 0 ? AlVehicleTrailerOtherPremium / NumberOfTrailerOthers : 0;

            AvgApdVehicleTotalPremium = ApdVehicleTotalPremium != 0 && NumberOfVehicles > 0 ? ApdVehicleTotalPremium / NumberOfVehicles : 0;
            AvgApdVehiclePowerPremium = ApdVehiclePowerPremium != 0 && NumberOfPowerUnits > 0 ? ApdVehiclePowerPremium / NumberOfPowerUnits : 0;
            AvgApdVehicleTrailerOtherPremium = ApdVehicleTrailerOtherPremium != 0 && NumberOfTrailerOthers > 0 ? ApdVehicleTrailerOtherPremium / NumberOfTrailerOthers : 0;

            AvgMtcRefVehicleTotalPremium = MtcRefVehicleTotalPremium != 0 && NumberOfVehicles > 0 ? MtcRefVehicleTotalPremium / NumberOfVehicles : 0;
            AvgMtcRefVehiclePowerPremium = MtcRefVehiclePowerPremium != 0 && NumberOfPowerUnits > 0 ? MtcRefVehiclePowerPremium / NumberOfPowerUnits : 0;
            AvgMtcRefVehicleTrailerOtherPremium = MtcRefVehicleTrailerOtherPremium != 0 && NumberOfTrailerOthers > 0 ? MtcRefVehicleTrailerOtherPremium / NumberOfTrailerOthers : 0;

            AvgTotalVehiclePremium = TotalVehiclePremium != 0 && NumberOfVehicles > 0 ? TotalVehiclePremium / NumberOfVehicles : 0;
            AvgTotalVehiclePowerPremium = TotalVehiclePowerPremium != 0 && NumberOfPowerUnits > 0 ? TotalVehiclePowerPremium / NumberOfPowerUnits : 0;
            AvgTotalVehicleTrailerOtherPremium = TotalVehicleTrailerOtherPremium > 0 && NumberOfTrailerOthers > 0 ? TotalVehicleTrailerOtherPremium / NumberOfTrailerOthers : 0;
        }
    }
}
