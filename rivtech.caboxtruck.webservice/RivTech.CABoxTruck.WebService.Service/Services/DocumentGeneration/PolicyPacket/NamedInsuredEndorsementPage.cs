using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class NamedInsuredEndorsementPage : CommonValuesPage
    {
        public NamedInsuredEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Named Insured Endst.docx";
            PageOrder = 1;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            FormValues.Add("EndorsementEffectiveDate", EndorsementEffectiveDate);
            FormValues.Add("NewInsuredName", ReportData.Risk?.NameAndAddress?.BusinessName);
        }
    }
}