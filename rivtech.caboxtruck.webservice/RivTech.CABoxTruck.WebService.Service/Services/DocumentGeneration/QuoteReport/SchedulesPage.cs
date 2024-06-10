using System;
using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using System.Text.RegularExpressions;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.QuoteReport
{
    public class SchedulesPage : SinglePage, IPage
    {
        public string TemplateName => "Schedules.docx";
        public int PageOrder => 2;

        public SchedulesPage(ReportData reportData) : base(reportData)
        {
        }

        public Form GetData()
        {
            var formData = new Form
            {
                PageOrder = PageOrder,
                TemplateName = $"documenttemplate/Quote/{TemplateName}",
                FormValues = GetFormValues(),
                TableValues = GetTableValues()
            };
            return formData;
        }

        private Dictionary<string, string> GetFormValues()
        {
            var formValues = new Dictionary<string, string>
            {
                {"QuoteNumber", ReportData.Risk.SubmissionNumber},
                {"QuoteDateTime", DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt")},
                {"ApplicantName", ReportData.Risk.NameAndAddress?.BusinessName},
                {"PolicyEffectiveDate", ReportData.Risk.BrokerInfo?.EffectiveDate?.ToString("MMMM dd, yyyy")},
                {"OptionNumber", ReportData.Id}
            };
            return formValues;
        }

        private Dictionary<string, List<Dictionary<string, string>>> GetTableValues()
        {
            var tableValues = new Dictionary<string, List<Dictionary<string, string>>>
            {
                {"Vehicle", GetFromVehicles() },
                {"Driver", GetFromDrivers() },
                {"Location", GetFromLocations() }
            };

            return tableValues;
        }

        private List<Dictionary<string, string>> GetFromVehicles()
        {
            var option = int.Parse(ReportData.Id) - 1;
            var vehicleFactors = ReportData.Risk.VehicleFactors.Where(vf => vf.OptionId == int.Parse(ReportData.Id)).ToList();
            var vehicles = ReportData.Risk.Vehicles
                .Where(v => v.Options != null && v.Options.Contains(ReportData.Id) && v.IsActive != false && v.IsValidated != false)
                .OrderBy(v => v.VIN)
                .ToList();
            var vehiclesWithPDCount = ReportData.Risk.Vehicles
                .Where(v => v.Options != null && v.Options.Contains(ReportData.Id) && v.IsActive != false && v.IsValidated != false &&
                       HasPhysicalDamage(v, option))
                .OrderBy(v => v.VIN)
                .ToList().Count;
            var coverage = ReportData.Risk.RiskCoverages[option];
            var serviceFeeAL = coverage?.RiskCoveragePremium?.RiskMgrFeeAL;
            var serviceFeePD = coverage?.RiskCoveragePremium?.RiskMgrFeePD;

            var tableValues = new List<Dictionary<string, string>>();

            var riskFeeValue = ((serviceFeeAL ?? 0) > 0 && vehicles.Count > 0) ? (serviceFeeAL / vehicles.Count) : 0;
            foreach (var vehicle in vehicles)
            {
                var pdRiskFeeValue = HasPhysicalDamage(vehicle, option) && ((serviceFeePD ?? 0) > 0 && vehicles.Count > 0) ? (serviceFeePD / vehiclesWithPDCount) : 0;

                var fromVehicleFactors = vehicleFactors?.Where(vf => Guid.Parse(vf?.VehicleId) == vehicle.Id)?.FirstOrDefault();
                var compAndFTPremium = (fromVehicleFactors?.CompPremium ?? 0) + (fromVehicleFactors?.FireTheftPremium ?? 0); // only either of the two will have a value.

                tableValues.Add(
                    new Dictionary<string, string>
                    {
                        {"Table_VehicleVin", $"{vehicle.VIN}"},
                        {"Table_VehiclePremLiab", $"${fromVehicleFactors?.TotalALPremium ?? 0:N2}"},
                        {"Table_VehiclePremComp", $"${compAndFTPremium:N2}"},
                        {"Table_VehiclePremColl", $"${fromVehicleFactors?.CollisionPremium ?? 0:N2}"},
                        //{"Table_VehicleSymbol", GetSymbolsText(coverage.ALSymbolId)}, //NEWCABT-3223: 5d. Remove Vehicle Symbol 
                        {"Table_VehicleMgmtFee", $"${riskFeeValue + pdRiskFeeValue:N2}"}
                    }
                );
            }

            return tableValues;
        }

        private List<Dictionary<string, string>> GetFromDrivers()
        {
            var drivers = ReportData.Risk.Drivers
                .Where(d => d.Options != null && d.Options.Contains(ReportData.Id) && d.IsActive != false && d.IsValidated != false)
                .OrderBy(d => d.LicenseNumber);

            return drivers.Select(driver => new Dictionary<string, string>
                {
                    {"Table_DriverLastName", driver.LastName},
                    {"Table_DriverFirstName", driver.FirstName},
                    {"Table_DriverState", driver.State},
                    {"Table_DriverLicense", driver.LicenseNumber},
                    {"Table_DriverMvr", driver.IsMvr.GetValueOrDefault() ? "Yes" : "No"},
                })
                .ToList();
        }

        private List<Dictionary<string, string>> GetFromLocations()
        {
            var locations = ReportData.DataSets["Addresses"].Where(a => a.AddressTypeId == 3);
            var tableValues = new List<Dictionary<string, string>>();
            var i = 1;
            foreach (var location in locations)
            {
                tableValues.Add(new Dictionary<string, string>
                {
                    { "Table_LocationNumber", i.ToString() },
                    { "Table_LocationAddress", location.Address.StreetAddress1 },
                    { "Table_LocationCity", location.Address.City },
                    { "Table_LocationState", location.Address.State },
                    { "Table_LocationZip", location.Address.ZipCode },
                });
                i++;
            }
            return tableValues;
        }

        private string GetSymbolsText(int? id)
        {
            string symbolText = ReportData.DataSets["Symbols"].FirstOrDefault(d => d.Id == id)?.Description ?? "";

            if (symbolText.Contains(")") || symbolText.Contains("-"))
            {
                string pattern = @"^\d+";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(symbolText);

                if (match.Success)
                    return match.Value;
            }

            return symbolText;
        }

        private bool HasPhysicalDamage(VehicleDTO vehicle, int optionId)
        {
            var option = (optionId + 1).ToString();
            var coverages = ReportData.Risk.RiskCoverages;

            if (option != "1")
            {
                return (coverages[optionId].CompDeductibleId != null &&
                        coverages[optionId].CompDeductibleId != LimitConstants.ComprehensiveNoCoverageId)
                    || (coverages[optionId].FireDeductibleId != null &&
                        coverages[optionId].FireDeductibleId != LimitConstants.ComprehensiveNoCoverageId)
                    || (coverages[optionId].CollDeductibleId != null &&
                        coverages[optionId].CollDeductibleId != LimitConstants.CollisionNoCoverageId);
            }
            else
            {
                return (vehicle.CoverageFireTheftDeductibleId != null &&
                        vehicle.CoverageFireTheftDeductibleId != LimitConstants.ComprehensiveNoCoverageId)
                    || (vehicle.CoverageCollisionDeductibleId != null &&
                        vehicle.CoverageCollisionDeductibleId != LimitConstants.CollisionNoCoverageId);
            }
        }
    }

}