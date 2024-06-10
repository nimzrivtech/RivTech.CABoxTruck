using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class DriverExclusionPage : CommonValuesPage
    {
        public DriverExclusionPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Driver Exclusion.docx";
            PageOrder = 21;
            FormId = "DriverExclusion";
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var driverEntities = new List<Dictionary<string, string>>();
            foreach (var driver in GetExcludedDrivers())
            {
                driverEntities.Add(new Dictionary<string, string>
                    {{"Table_Names_Item", $"{driver.FirstName} {driver.LastName}, {driver.LicenseNumber}"}});
            }

            TableValues = new Dictionary<string, List<Dictionary<string, string>>>
            {
                {"Names_", driverEntities}
            };
        }

        private List<FormDriverDTO> GetExcludedDrivers()
        {
            List<FormDriverDTO> excludedDrivers;
            var effectiveDate = PolicyEffectiveDate;

            var formOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.FormId == "DriverExclusion")?.Other;

            if (formOtherContent != null)
            {
                var formValues = JsonConvert.DeserializeObject<FormDriverExclusionDTO>(formOtherContent);
                excludedDrivers = formValues.Drivers;
                effectiveDate = formValues.UpdatedDate?.ToString(DateFormat);
            }
            else
            {
                var optionId = (ReportData.Risk.Binding?.BindOptionId ?? 1).ToString();
                var drivers = ReportData.Risk.Drivers.Where(d => d.IsActive != false && (d.Options == null || !d.Options.Contains(optionId)));
                excludedDrivers = drivers.Select(d => new FormDriverDTO {FirstName = d.FirstName, LastName = d.LastName, LicenseNumber = d.LicenseNumber}).ToList();
            }

            FormValues.Add("EndorsementEffectiveDate", effectiveDate);
            base.ReplaceFormValues("EffectiveDate", effectiveDate);

            return excludedDrivers;
        }
    }
}