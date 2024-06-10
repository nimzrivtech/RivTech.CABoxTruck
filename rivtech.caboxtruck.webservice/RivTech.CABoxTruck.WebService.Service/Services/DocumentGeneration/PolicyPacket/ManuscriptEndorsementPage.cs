using System.Linq;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class ManuscriptEndorsementPage : CommonValuesPage
    {
        public ManuscriptEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Manuscript Endorsement.docx";
            PageOrder = 33;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var manuscriptForm = (FormDTO)ReportData.DataSets["ManuscriptForm"]?.FirstOrDefault();

            if(manuscriptForm != null)
            {
                var quoteDate = ReportData.DataSets["QuoteDate"]?.FirstOrDefault();
                var formVersion = manuscriptForm?.FormVersions?.Where(fv => quoteDate >= fv.EffectiveDate)?
                        .OrderBy(fv => fv.EffectiveDate)?
                        .FirstOrDefault() ??
                        manuscriptForm?.FormVersions?
                        .OrderByDescending(fv => fv.EffectiveDate)?
                        .FirstOrDefault();

                if (formVersion != null && (quoteDate != null || !ReportData.Risk.IsRaterVersionLocked))
                    TemplateName = formVersion.FilePath;
            }

            var manuscript = ReportData.Risk.RiskManuscripts.FirstOrDefault(m => m.Id == ReportData.Id);
            var effectiveDate = PolicyEffectiveDate;
            var premium = (IsEndorsement ? manuscript?.ProratedPremium : manuscript?.Premium) ?? 0;
            if (manuscript?.IsProrate == true)
                effectiveDate = manuscript.EffectiveDate.ToString(DateFormat);
            base.ReplaceFormValues("EffectiveDate", effectiveDate);
            FormValues.Add("Premium", premium.ToCurrency());
            FormValues.Add("Description", manuscript?.Description);
        }
    }
}