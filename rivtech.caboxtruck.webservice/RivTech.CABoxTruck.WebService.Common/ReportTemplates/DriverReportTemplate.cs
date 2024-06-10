using DocumentFormat.OpenXml.VariantTypes;
using Newtonsoft.Json;
using System;

namespace RivTech.CABoxTruck.WebService.Common.ReportTemplates
{
    public class DriverReportTemplate
    {
        [JsonProperty("First Name")]
        public string FirstName { get; set; }
        [JsonProperty("Middle Name")]
        public string MiddleName { get; set; }
        [JsonProperty("Last Name")]
        public string LastName { get; set; }
        [JsonProperty("DL Number")]
        public string LicenseNumber { get; set; }
        [JsonProperty("License State")]
        public string State { get; set; }
        [JsonProperty("DOB")]
        public string BirthDate { get; set; }
        [JsonProperty("Years Driving Exp")]
        public string YrsDrivingExp { get; set; }
        [JsonProperty("Years Commercial Driving Exp")]
        public string YrsCommDrivingExp { get; set; }
    }
}
