using Newtonsoft.Json;
using System;

namespace RivTech.CABoxTruck.WebService.Common.ReportTemplates
{
    public class VehicleReportTemplate
    {
        [JsonProperty("VIN")]
        public string VIN { get; set; }
        [JsonProperty("Year")]
        public string Year { get; set; }
        [JsonProperty("Make")]
        public string Make { get; set; }
        [JsonProperty("Model")]
        public string Model { get; set; }
        [JsonProperty("State")]
        public string RegisteredState { get; set; }
        [JsonProperty("TIV (stated Amount)")]
        public string StatedAmount { get; set; }
        [JsonProperty("Vehicle Type")]
        public string VehicleType { get; set; }
        [JsonProperty("Vehicle Description")]
        public string Description { get; set; }
        [JsonProperty("Vehicle Business Class")]
        public string BusinessClass { get; set; }
        [JsonProperty("Gross Vehicle Weight (lbs)")]
        public string GrossVehicleWeight { get; set; }
        [JsonProperty("Vehicle Radius")]
        public string Radius { get; set; }
        [JsonProperty("Comprehensive / Fire & Theft (Yes or No)")]
        public string IsCoverageFireTheft { get; set; }
        [JsonProperty("Collision (Yes or No)")]
        public string IsCoverageCollision { get; set; }
        [JsonProperty("Comprehensive / Fire & Theft Deductible")]
        public string CoverageFireTheft { get; set; }
        [JsonProperty("Collision Deductible")]
        public string CoverageCollisionDeductible { get; set; }
        [JsonProperty("Cargo ( Yes or No)")]
        public string HasCoverageCargo { get; set; }
        [JsonProperty("Refrigeration (Yes or No)")]
        public string HasCoverageRefrigeration { get; set; }
    }
}
