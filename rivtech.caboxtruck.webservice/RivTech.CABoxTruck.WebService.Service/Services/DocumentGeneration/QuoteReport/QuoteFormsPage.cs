using System;
using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.SS.Formula.Functions;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.QuoteReport
{
    public class QuoteFormsPage : SinglePage, IPage
    {
        public string TemplateName => "Quote-Form.docx";
        public int PageOrder => 2;
        private List<string> _stateForms = new List<string> {
            "UninsuredMotoristCoverageBodilyInjury",
            "SplitBodilyInjuryUninsuredMotoristCoverage",
            "TexasChangesCancellationandNonRenewal",
            "TexasChanges",
            "TexasPersonalInjuryProtection",
            "TexasUMandUIMCoverage",
            "TexasSplitUninsuredandUnderinsuredMotoristCoverage",
            "NewJerseyUMandUIMCoverage",
            "NewJerseyChanges",
            "NewJerseyPersonalInjuryProtection",
            "NewJerseySplitUninsuredandUnderinsuredMotoristCoverage",
            "MissouriChanges",
            "MissouriUninsuredMotoristsCoverage",
            "MissouriUnderinsuredMotoristsCoverage",
            "GeorgiaChanges",
            "GeorgiaPolicyHolderNoticeRegardingUMCoverage",
            "GeorgiaUMCoverageReducedbyatFaultLiabilityLimits",
            "PennsylvaniaChanges",
            "PennsylvaniaBasicFirstPartyBenefits",
            "PennsylvaniaSplitUMCoverageLimitsNS",
            "PennsylvaniaSplitUIMCoverageLimitsNS",
            "PennsylvaniaUMCoverageLimitsNS",
            "PennsylvaniaUIMCoverageLimitsNS",
            "PennsylvaniaAddCombFirstPartyBenefitsEndorsement",
            "FloridaChanges",
            "FloridaPersonalInjuryProtection",
            "FloridaUninsuredMotoristsCoverageNS",
            "OregonChanges",
            "OregonPersonalInjuryProtection",
            "OregonUninsuredMotoristCoverage",
            "OhioChanges",
            "OhioUMandUIMCoverage",
            "IndianaChanges",
            "IndianaUninsuredMotoristCoverage",
            "IndianaUnderinsuredMotoristCoverage",
            "TennesseeCarolinaChanges",
            "TennesseeUninsuredMotoristCoverage",
            "ColoradoChanges",
            "ColoradoUMBICoverage",
            "ColoradoUMPDCoverage",
            "ColoradoMedicalPaymentCoverage",
            "VirginiaSplitUMCoverage",
            "VirginiaImportantNoticeUMCoverage",
            "IllinoisChanges",
            "IllinoisUMCoverage",
            "IllinoisUMPDCoverage",
            "IllinoisUIMCoverage",
            "ArizonaChanges",
            "ArizonaUIMCoverage",
            "ArizonaUMCoverage",
            "MississippiUninsuredMotoristCoverageNS",
        };
        protected RiskCoverageDTO Coverage
        {
            get
            {
                var riskOptions = ReportData.Risk.RiskCoverages;
                var optionId = riskOptions.Count == 1 ? 0 : (ReportData.Risk.Binding?.BindOptionId ?? 1) - 1;
                return riskOptions.Count == 0 ? new RiskCoverageDTO() : ReportData.Risk.RiskCoverages[optionId];
            }
        }

        public QuoteFormsPage(ReportData reportData) : base(reportData)
        {
        }

        public Form GetData()
        {
            var formData = new Form
            {
                PageOrder = PageOrder,
                TemplateName = $"documenttemplate/Quote/{TemplateName}",
                TableValues = GetTableValues()
            };
            return formData;
        }

        private Dictionary<string, List<Dictionary<string, string>>> GetTableValues()
        {
            var tableValues = new Dictionary<string, List<Dictionary<string, string>>>
            {
                {"Form_", GetFromRiskForms() }
            };

            return tableValues;
        }

        private List<Dictionary<string, string>> GetFromRiskForms()
        {
            var allForms = ReportData.DataSets["AllForms"].Cast<RiskFormDTO>().ToList();
            if (Coverage.GLBILimitId.HasValue && Coverage.GLBILimitId != LimitConstants.GLNoneCoverageId
                            && Coverage.GLBILimitId != LimitConstants.GLNoneCoverageIdv7)
            {
                var glForm = allForms.Find(x => x.FormId == "GeneralLiabilityEndorsement");
                if (ReportData.Risk.RiskForms.Any(x => x.FormId == "GeneralLiabilityEndorsement") == false)
                    ReportData.Risk.RiskForms.Add(glForm);
            }
            ReportData.Risk.RiskForms.ForEach(rf =>
            {
                if (rf.FormId == "MotorTruckCargoEndorsement")
                {
                    rf.IsSelected = Coverage.CargoLimitId > LimitConstants.CargoNoneCoverageId;
                }

                if (rf.FormId == "GeneralLiabilityEndorsement")
                {
                    rf.IsSelected = Coverage.GLBILimitId.HasValue && Coverage.GLBILimitId != LimitConstants.GLNoneCoverageId
                                    && Coverage.GLBILimitId != LimitConstants.GLNoneCoverageIdv7;
                }
            });

            var check = ReportData.Risk.RiskForms.Where(x => x.Form.FormNumber == "RPCA017");
            var forms = ReportData.Risk.RiskForms
                .Where(rf =>
                    rf?.IsSelected == true
                    && !rf.Form.IsSupplementalDocument &&
                    //rf.Form.FormNumber != "RPCAMANU" &&
                    !_stateForms.Any(x => x == rf.Form.Id) // Exclude State Forms 
                    || rf.Form.FormNumber == "RPCA009"
                    || rf.Form.FormNumber == "RPCA017"
                    || rf.Form.FormNumber == "RPCA018"
                    || rf.Form.FormNumber == "RPCA021"
                    || rf.Form.FormNumber == "RPCA028"
                    || rf.Form.FormNumber == "RPCA029"
                    || rf.Form.FormNumber == "RPCA030")
                .GroupBy(rf => rf.Form.FormNumber)
                .Select(rf => rf.First())
                .OrderBy(rf => rf.Form.SortOrder)
                .ToList();


            var formTable = new List<Dictionary<string, string>>();
            var manuscriptFormNumber = "RPCAMANU";
            
            foreach (var form in forms)
            {
                if (form.FormId == "SurplusLinesNotice") form.Form.FormNumber = ReportData.Risk.Binding.SurplusLIneNum;

                if (form.Form.Id == "ManuscriptEndorsement" && !formTable.Any(x => x.ContainsValue("RPCAMANU")))
                {
                    foreach (var manuscript in ReportData.Risk.RiskManuscripts.Where(m => m.IsSelected.GetValueOrDefault() || m.IsProrate))
                    {
                        var manuscriptForm = (FormDTO)ReportData.DataSets["ManuscriptForm"]?.FirstOrDefault();

                        if (manuscriptForm != null)
                        {
                            var quoteDate = ReportData.DataSets["QuoteDate"]?.FirstOrDefault();
                            var formVersion = manuscriptForm?.FormVersions?.Where(fv => quoteDate >= fv.EffectiveDate)?
                                    .OrderBy(fv => fv.EffectiveDate)?
                                    .FirstOrDefault() ??
                                    manuscriptForm?.FormVersions?
                                    .OrderByDescending(fv => fv.EffectiveDate)?
                                    .FirstOrDefault();

                            if (formVersion != null && (quoteDate != null || !ReportData.Risk.IsRaterVersionLocked))
                                manuscriptFormNumber = formVersion.FormNumber;
                        }

                        formTable.Add(new Dictionary<string, string>
                        {
                            {"Table_Form_Name", manuscript.Title},
                            {"Table_Form_Number", manuscriptFormNumber}
                        });
                    }
                }
                else
                {
                    formTable.Add(new Dictionary<string, string>
                    {
                        {"Table_Form_Name", form.Form.Name},
                        {"Table_Form_Number", form.Form.FormNumber}
                    });
                }
            }

            // State specific forms
            var umForm = ReportData.Risk.RiskForms
                .Where(rf =>
                    rf.IsSelected.GetValueOrDefault()
                    && !rf.Form.IsSupplementalDocument &&
                    _stateForms.Contains(rf.Form.Id))
                .GroupBy(rf => rf.Form.FormNumber)
                .Select(rf => rf.First())
                .OrderBy(rf => rf.Form.SortOrder).ThenBy(rf => rf.Form.CreatedDate)
                .ToList();

            if (umForm != null)
            {
                foreach (var form in umForm)
                {
                    formTable.Add(new Dictionary<string, string>
                    {
                        {"Table_Form_Name", form.Form.Name},
                        {"Table_Form_Number", form.Form.FormNumber}
                    });
                }
            }

            return formTable;
        }
    }

}