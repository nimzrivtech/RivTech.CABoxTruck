using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class BusinessAutoPolicyDeclarationsPage : CommonValuesPage
    {
        private dynamic _slBroker;
        private dynamic _slInfo;
        private bool _isVehiclesHasPD;
        private BindingDTO _bind;
        private List<int> _UMBISingleLimits = new List<int> { 314, 318, 319, 320, 321, 322 };
        private List<int> _UMBISplitLimits = new List<int> { 310, 311, 312, 313, 315, 316, 317 };
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
            "SouthCarolinaChanges",
            "SouthCarolinaSplitUMCoverage",
            "SouthCarolinaSplitUIMCoverage",
            "SouthCarolinaUMCoverage",
            "SouthCarolinaUIMCoverage"
        };

        public BusinessAutoPolicyDeclarationsPage(ReportData reportData) : base(reportData)
        {
            FormId = "BusinessAutoPolicyDeclarations";
            Initialize();
            PageOrder = 3;
        }

        private void Initialize()
        {
            _bind = ReportData.Risk.Binding;
            var creditedOfficeId = _bind?.CreditedOfficeId ?? 3;
            var creditedOffice = ReportData.DataSets["CreditedOffice"]
                .FirstOrDefault(bt => bt.Id == creditedOfficeId)?.Description;
            _slBroker = ReportData.DataSets["SLBrokers"].FirstOrDefault(d => d.Id == creditedOffice);
            _slInfo = ReportData.DataSets["SLInfo"].FirstOrDefault();

            TableValues = new Dictionary<string, List<Dictionary<string, string>>>();
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();
            //Replace with the correct policy effective date
            FormValues.Remove("EffectiveDate");
            FormValues.Add("EffectiveDate", PolicyEffectiveDate);

            FormValues.Add("ReportCompanyName", CompanyName.ToUpper());
            FormValues.Add("ReportTitleCompanyName", CompanyName.ToUpper());

            FormValues.Add("BusinessDescription", ReportData.DataSets["MainUse"].FirstOrDefault(m => m.Id == ReportData.Risk?.BusinessDetails?.MainUseId)?.Description);

            FormValues.Add("CollisionDeductible", GetLimitsText(Coverage.CollDeductibleId, "CollisionLimits"));

            FormValues.Add("ComprehensiveDeductible", GetLimitsText(Coverage.CompDeductibleId ?? Coverage.FireDeductibleId, "ComprehensiveLimits"));
            FormValues.Add("NoOfInsuredUnits", ReportData.Risk.Vehicles.Count.ToString());

            SetAutoLiability();
            SetGeneralLiability();
            SetAutoPhysicalDamage();
            SetMotorTruckCargo();

            if(ReportData.Risk.RaterVersionId >= 7)
                SetTrailerCoverage();

            SetPolicyTotals();
            SetSurplusLines();

            SetVehicleAutoLiability();
            SetVehicleAutoPhysicalDamage();
            SetVehicleMotoTruckCargo();

            SetDrivers();
            SetGaragingAddress();
            SetForms();

            SetDecPageVersion();
        }

        private void SetDecPageVersion()
        {
            var hasApd = (Coverage.CompDeductibleId > LimitConstants.ComprehensiveNoCoverageId
                         || Coverage.FireDeductibleId > LimitConstants.ComprehensiveNoCoverageId
                         || Coverage.CollDeductibleId > LimitConstants.CollisionNoCoverageId)
                         && _isVehiclesHasPD;
                         //|| ComprehensivePremium > 0 || CollisionPremium > 0;
            var hasMtc = Coverage.CargoLimitId > LimitConstants.CargoNoneCoverageId || CargoPremium > 0;
            var hasTrailer = HasTRC || HasNonOwnedTRC || TrailerInterchangePremium > 0 || NonOwnedPremium > 0;
            var formVersion = base.SetFormVersion();
            var pathVersion = formVersion != null ? $"FormsV{formVersion?.Version}/" : "";
            var path = IsEndorsement ? $"{pathVersion}Dec Pages/" : $"{pathVersion}Dec Pages/Bind/";
            var hasGL = HasGL || GeneralLiabilityPremium > 0;

            if (hasApd)
            {
                if (hasMtc && hasGL && hasTrailer)
                    TemplateName = $"{path}BT Policy Declarations w AL_APD_MTC_GL_TRC.docx";
                else if (hasMtc && hasGL)
                    TemplateName = $"{path}BT Policy Declarations w AL_APD_MTC_GL.docx";
                else if (hasMtc && hasTrailer)
                    TemplateName = $"{path}BT Policy Declarations w AL_APD_MTC_TRC.docx";
                else if (hasMtc)
                    TemplateName = $"{path}BT Policy Declarations w AL_APD_MTC.docx";
                else if (hasGL && hasTrailer)
                    TemplateName = $"{path}BT Policy Declarations w AL_APD_GL_TRC.docx";
                else if (hasGL)
                    TemplateName = $"{path}BT Policy Declarations w AL_APD_GL.docx";
                else if (hasTrailer)
                    TemplateName = $"{path}BT Policy Declarations w AL_APD_TRC.docx";
                else
                    TemplateName = $"{path}BT Policy Declarations w AL_APD.docx";
            }
            else if (hasMtc)
            {
                if (hasGL && hasTrailer)
                    TemplateName = $"{path}BT Policy Declarations w AL_MTC_GL_TRC.docx";
                else if (hasGL)
                    TemplateName = $"{path}BT Policy Declarations w AL_MTC_GL.docx";
                else if (hasTrailer)
                    TemplateName = $"{path}BT Policy Declarations w AL_MTC_TRC.docx";
                else
                    TemplateName = $"{path}BT Policy Declarations w AL_MTC.docx";
            }
            else if (hasGL)
            {
                if (hasTrailer)
                    TemplateName = $"{path}BT Policy Declarations w AL_GL_TRC.docx";
                else
                    TemplateName = $"{path}BT Policy Declarations w AL_GL.docx";
            }
            else if (hasTrailer)
                TemplateName = $"{path}BT Policy Declarations w AL_TRC.docx";
            else
                TemplateName = $"{path}BT Policy Declarations w AL.docx";
        }

        private void SetAutoLiability()
        {
            var alItems = new[] {
                new
                {
                    Name = "Bodily Injury and Property\nDamage Liability",
                    Symbol = GetSymbolsText(Coverage.ALSymbolId),
                    Limit = GetLimitsText(Coverage.AutoBILimitId, "AutoBILimits"),
                    Premium = $"${AutoLiabilityPremium:N2}"
                }
            }.ToList();

            var pipLimit = GetLimitsText(Coverage.PIPLimitId, "PipLimits");
            if (!string.IsNullOrEmpty(pipLimit) && pipLimit != "NONE")
            {
                alItems.Add(new
                    {
                        Name = "Personal Injury Protection\n(Or Equivalent No Fault Coverage)\nfor First Party Drivers & Passengers",
                        Symbol = GetSymbolsText(Coverage1.PIPLimitSymbolId),
                        Limit = pipLimit,
                        Premium = "Included in BI"
                });
            }

            var uninsuredLimit = GetLimitsText(Coverage.UMBILimitId, "UMBILimits");
            if (!string.IsNullOrEmpty(uninsuredLimit) && uninsuredLimit != "NONE")
            {
                alItems.Add(new
                {
                    Name = "Uninsured Motorist BI",
                    Symbol = GetSymbolsText(Coverage1.UMBILimitSymbolId),
                    Limit = uninsuredLimit,
                    Premium = "Included in BI"
                });
            }

            var underinsuredLimit = GetLimitsText(Coverage.UIMBILimitId, "UIMBILimits");
            if (!string.IsNullOrEmpty(underinsuredLimit) && underinsuredLimit != "NONE")
            {
                alItems.Add(new
                {
                    Name = "Underinsured Motorist BI",
                    Symbol = GetSymbolsText(Coverage1.UIMBILimitSymbolId),
                    Limit = underinsuredLimit,
                    Premium = "Included in BI"
                });
            }

            var uninsuredPdLimit = GetLimitsText(Coverage.UMPDLimitId, "UMPDLimits");
            if (!string.IsNullOrEmpty(uninsuredPdLimit) && uninsuredPdLimit != "NONE")
            {
                alItems.Add(new
                {
                    Name = "Uninsured Motorist PD",
                    Symbol = GetSymbolsText(Coverage1.UMPDLimitSymbolId),
                    Limit = uninsuredPdLimit,
                    Premium = "Included in BI"
                });
            }

            var underinsuredPdLimit = GetLimitsText(Coverage.UIMPDLimitId, "UIMPDLimits");
            if (!string.IsNullOrEmpty(underinsuredPdLimit) && underinsuredPdLimit != "NONE")
            {
                alItems.Add(new
                {
                    Name = "Underinsured Motorist PD",
                    Symbol = GetSymbolsText(Coverage1.UIMPDLimitSymbolId),
                    Limit = underinsuredPdLimit,
                    Premium = "Included in BI"
                });
            }

            var deductibleLimit = GetLimitsText(Coverage.LiabilityDeductibleId, "LiabilityDeductibleLimits");
            if (!string.IsNullOrEmpty(deductibleLimit) && deductibleLimit != "$0")
            {
                alItems.Add(new
                {
                    Name = "Liability Deductible",
                    Symbol = "",
                    Limit = deductibleLimit,
                    Premium = "Included in BI"
                });
            }

            var medicalPaymentsLimit = GetLimitsText(Coverage.MedPayLimitId, "MedPayLimits");
            if (!string.IsNullOrEmpty(medicalPaymentsLimit) && medicalPaymentsLimit != "NONE")
            {
                alItems.Add(new
                {
                    Name = "Medical Payments",
                    Symbol = GetSymbolsText(Coverage1.MedPayLimitSymbolId),
                    Limit = medicalPaymentsLimit,
                    Premium = "Included in BI"
                });
            }


            var apdTable = new List<Dictionary<string, string>>();
            foreach (var item in alItems)
            {
                apdTable.Add(new Dictionary<string, string>
                {
                    {"Table_AL_Name", item.Name},
                    {"Table_AL_Symbol", item.Symbol},
                    {"Table_AL_Limit", item.Limit},
                    {"Table_AL_Premium", item.Premium}
                });
            }
            TableValues.Add("AL_", apdTable);
        }

        private void SetAutoPhysicalDamage()
        {
            var apdItems = new List<dynamic>();

            var apdTable = new List<Dictionary<string, string>>();

            if (Coverage.CompDeductibleId > LimitConstants.ComprehensiveNoCoverageId)
            {
                apdItems.Add(new
                {
                    Coverage = "Comprehensive",
                    Symbol = GetSymbolsText(Coverage1.CompDeductibleSymbolId ?? Coverage1.FireDeductibleSymbolId),
                    Deductible = Coverage.CompDeductibleId == LimitConstants.ComprehensiveNoCoverageId ? GetLimitsText(Coverage.CompDeductibleId, "ComprehensiveLimits") : "See Schedule",
                    Premium = ComprehensivePremium.ToCurrency()
                });
            }

            if (Coverage.FireDeductibleId > LimitConstants.ComprehensiveNoCoverageId)
            {
                apdItems.Add(new
                {
                    Coverage = "Fire / Theft",
                    Symbol = GetSymbolsText(Coverage1.FireDeductibleSymbolId ?? Coverage1.CompDeductibleSymbolId),
                    Deductible = Coverage.FireDeductibleId == LimitConstants.ComprehensiveNoCoverageId ? GetLimitsText(Coverage.CompDeductibleId, "ComprehensiveLimits") : "See Schedule",
                    Premium = ComprehensivePremium.ToCurrency()
                });
            }

            if ((Coverage.CompDeductibleId == null || Coverage.CompDeductibleId == LimitConstants.ComprehensiveNoCoverageId)
                && (Coverage.FireDeductibleId == null || Coverage.FireDeductibleId == LimitConstants.ComprehensiveNoCoverageId))
            {
                if (ComprehensivePremium > 0)
                {
                    var coverageName = (Coverage.CompDeductibleId == null &&
                                        Coverage.FireDeductibleId != null || Coverage.FireDeductibleId == LimitConstants.ComprehensiveNoCoverageId)
                                        ? "Fire / Theft" : "Comprehensive";
                    coverageName = (Coverage.FireDeductibleId == null &&
                                        Coverage.CompDeductibleId != null || Coverage.CompDeductibleId == LimitConstants.ComprehensiveNoCoverageId)
                                        ? "Comprehensive" : "Fire / Theft";

                    apdItems.Add(new
                    {
                        Coverage = coverageName,
                        Symbol = GetSymbolsText(Coverage1.CompDeductibleSymbolId ?? Coverage1.FireDeductibleSymbolId),
                        Deductible = Coverage.CompDeductibleId == LimitConstants.ComprehensiveNoCoverageId ||
                                     Coverage.FireDeductibleId == LimitConstants.ComprehensiveNoCoverageId ?
                                     GetLimitsText(coverageName == "Comprehensive" ?
                                                Coverage.CompDeductibleId :
                                                Coverage.FireDeductibleId, "ComprehensiveLimits") : "See Schedule",
                        Premium = ComprehensivePremium.ToCurrency()
                    });
                }
            }

            if (Coverage.CollDeductibleId > LimitConstants.CollisionNoCoverageId || CollisionPremium > 0)
            {
                apdItems.Add(new
                {
                    Coverage = "Collision",
                    Symbol = GetSymbolsText(Coverage1.CollDeductibleSymbolId),
                    Deductible = Coverage.CollDeductibleId == LimitConstants.CollisionNoCoverageId ? GetLimitsText(Coverage.CollDeductibleId, "CollisionLimits") : "See Schedule",
                    Premium = CollisionPremium.ToCurrency()
                });
            }

            foreach (var item in apdItems)
            {
                apdTable.Add(new Dictionary<string, string>
                {
                    {"Table_APD_Coverage", item.Coverage},
                    {"Table_APD_Symbol", item.Symbol},
                    {"Table_APD_Deductible", item.Deductible},
                    {"Table_APD_Premium", item.Premium}
                });
            }
            TableValues.Add("APD_", apdTable);
        }

        private void SetMotorTruckCargo()
        {
            var mtcItems = new List<dynamic>();
            if (Coverage.CargoLimitId > LimitConstants.CargoNoneCoverageId || CargoPremium > 0)
            {
                mtcItems.Add(new
                {
                    Coverage = "Motor Truck Cargo",
                    Limit = "$" + GetUpperLimitsText(Coverage.CargoLimitId, "CargoLimits"),
                    Premium = CargoPremium.ToCurrency()
                });
            }
            if (Coverage.RefCargoLimitId > LimitConstants.RefNoneCoverageId)
            {
                mtcItems.Add(new
                {
                    Coverage = "Refrigeration",
                    Limit = "$" + GetUpperLimitsText(Coverage.RefCargoLimitId, "RefLimits"),
                    Premium = RefrigerationPremium.ToCurrency()
                });
            }

            var mtcTable = new List<Dictionary<string, string>>();
            foreach (var item in mtcItems)
            {
                mtcTable.Add(new Dictionary<string, string>
                {
                    {"Table_MTC_Coverage", item.Coverage},
                    {"Table_MTC_Limit", item.Limit},
                    {"Table_MTC_Premium", item.Premium}
                });
            }
            TableValues.Add("MTC_", mtcTable);
        }

        private void SetGeneralLiability()
        {
            var glItems = new[] {new {Name = "", Limit = "", Premium = ""}}.ToList();
            glItems.RemoveAt(0);

            if (HasGL || GeneralLiabilityPremium > 0)
            {
                FormValues.Add("GLPremium", GeneralLiabilityPremium.ToCurrency());
                glItems.Add(new
                {
                    Name = "BI Occurrence Limit",
                    Limit = $"${GetUpperLimitsText(Coverage.GLBILimitId, "GlLimits")}",
                    Premium = ""
                });
            }

            var glFormOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f =>
                f.IsSelected.GetValueOrDefault() && f.FormId == "GeneralLiabilityEndorsement")?.Other;
            if (glFormOtherContent != null)
            {
                var glInputs = JsonConvert.DeserializeObject<GeneralLiabilityDTO>(glFormOtherContent);
                glItems.Add(new
                {
                    Name = "Personal & Advertising\n(Any One Person / Organization)",
                    Limit = $"${glInputs?.InjuryLimit ?? 0:N0}",
                    Premium = ""
                });
                glItems.Add(new
                {
                    Name = "Medical Expenses\n(One Person)",
                    Limit = $"${glInputs?.MedicalExpenses ?? 0:N0}",
                    Premium = ""
                });
                glItems.Add(new
                {
                    Name = "Damage to premises rented to you\n(any one premises)",
                    Limit = $"${glInputs?.DamagesRentedToYou ?? 0:N0}",
                    Premium = ""
                });
                glItems.Add(new
                {
                    Name = "Products / Completed Operations\nAggregate Limit",
                    Limit = $"${glInputs?.AggregateLimit ?? 0:N0}",
                    Premium = ""
                });
            }

            if (HasGL)
            {
                glItems.Add(new
                {
                    Name = "General Aggregate Limit",
                    Limit = $"${GetLowerLimitsText(Coverage.GLBILimitId, "GlLimits")}",
                    Premium = ""
                });
            }

            var apdTable = new List<Dictionary<string, string>>();
            foreach (var item in glItems)
            {
                apdTable.Add(new Dictionary<string, string>
                {
                    {"Table_GL_Name", item.Name},
                    {"Table_GL_Limit", item.Limit},
                    {"Table_GL_Premium", item.Premium}
                });
            }

            TableValues.Add("GL_", apdTable);
        }

        private void SetTrailerCoverage()
        {
            var trcItems = new List<dynamic>();
            var trcTable = new List<Dictionary<string, string>>();
            
            if (HasTRC || TrailerInterchangePremium > 0)
            {
                var limitValue = ParseTrailerLimitValue(GetLimitsValue(Coverage.TrailerInterchangeLimitId, "TrailerInterchangeLimits"));
                var limitValueNone = GetLimitsValue(Coverage.TrailerInterchangeLimitId, "TrailerInterchangeLimits");
                trcItems.Add(new
                {
                    Coverage = "Trailer Interchange",
                    Limit = HasTRC ? $"${limitValue.Item1}" : limitValueNone,
                    Deductible = HasTRC ? $"${limitValue.Item2}" : limitValueNone,
                    Premium = TrailerInterchangePremium.ToCurrency()
                });
            }

            if (HasNonOwnedTRC || NonOwnedPremium > 0)
            {
                var limitValue = ParseTrailerLimitValue(GetLimitsValue(Coverage.NonOwnedTrailerLimitId, "NonOwnedTrailerLimits"));
                var limitValueNone = GetLimitsValue(Coverage.NonOwnedTrailerLimitId, "NonOwnedTrailerLimits");
                trcItems.Add(new
                {
                    Coverage = "Non-Owned Trailer",
                    Limit = HasNonOwnedTRC ? $"${limitValue.Item1}" : limitValueNone,
                    Deductible = HasNonOwnedTRC ? $"${limitValue.Item2}" : limitValueNone,
                    Premium = NonOwnedPremium.ToCurrency()
                });
            }

            foreach (var item in trcItems)
            {
                trcTable.Add(new Dictionary<string, string>
                {
                    {"Table_TRC_Coverage", item.Coverage},
                    {"Table_TRC_Limit", item.Limit},
                    {"Table_TRC_Deductible", item.Deductible},
                    {"Table_TRC_Premium", item.Premium}
                });
            }

            TableValues.Add("TRC_", trcTable);
        }

        private void SetPolicyTotals()
        {
            var additionalPremium = (Coverage.RiskCoveragePremium?.AdditionalInsuredPremium ?? 0) +
                                    (Coverage.RiskCoveragePremium?.WaiverOfSubrogationPremium ?? 0) +
                                    (Coverage.RiskCoveragePremium?.PrimaryNonContributoryPremium ?? 0);
            var manuscriptALPremium = ManuscriptPremiumAL;
            var manuscriptPDPremium = ManuscriptPremiumPD;

            var totalItems = new[] {
                new {Name = "Auto Liability Premium", Value = $"${(AutoLiabilityPremium):N2}"}
            }.ToList();

            var additionalPremiumFullyEarned = GetProratedPremium("Policy", "Additional Premium", additionalPremium);
            if (additionalPremiumFullyEarned > 0)
            {
                totalItems.Add(new {Name = $"{Spacer(5)}Additional Premium (Fully Earned)", Value = ""}); // additionalPremiumFullyEarned.ToCurrency()
            }

            var additionalInsuredPremium = GetProratedPremium("Policy", "Additional Insured", Coverage.RiskCoveragePremium?.AdditionalInsuredPremium ?? 0);
            if (additionalInsuredPremium > 0)
            {
                totalItems.Add(new {Name = $"{Spacer(10)}Additional Insured", Value = additionalInsuredPremium.ToCurrency()});
            }

            var waiverOfSubrogationPremium = GetProratedPremium("Policy", "Waiver of Subrogation", Coverage.RiskCoveragePremium?.WaiverOfSubrogationPremium ?? 0);
            if (waiverOfSubrogationPremium > 0)
            {
                totalItems.Add(new { Name = $"{Spacer(10)}Waiver of Subrogation", Value = waiverOfSubrogationPremium.ToCurrency()});
            }

            var primaryNonContributoryPremium = GetProratedPremium("Policy", "Primary & Non-Contributory", Coverage.RiskCoveragePremium?.PrimaryNonContributoryPremium ?? 0);
            if (primaryNonContributoryPremium > 0)
            {
                totalItems.Add(new { Name = $"{Spacer(10)}Primary & Non-Contributory", Value = primaryNonContributoryPremium.ToCurrency() });
            }

            if (ManuscriptPremiumAL > 0 || manuscriptALPremium != 0) totalItems.Add(new { Name = $"{Spacer(5)}Manuscript Premium", Value = manuscriptALPremium.ToCurrency() });

            if (PhysicalDamagePremium > 0 || PhysicalDamagePremium != 0)
            {
                totalItems.Add(new { Name = "Physical Damage Premium", Value = PhysicalDamagePremium.ToCurrency()});
            }

            var hiredPhysdamOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.IsSelected.GetValueOrDefault() && f.FormId == "HiredPhysicalDamage")?.Other;
            if (hiredPhysdamOtherContent != null)
            {
                var hiredPhysdamPremium = Coverage.RiskCoveragePremium?.HiredPhysicalDamage ?? 0;
                totalItems.Add(new { Name = $"{Spacer(5)}Hired Physical Damage", Value = hiredPhysdamPremium.ToCurrency() });
            }

            if(ReportData.Risk.RaterVersionId < 7)
            {
                var trailerOtherContent = ReportData.Risk.RiskForms.FirstOrDefault(f => f.IsSelected.GetValueOrDefault() && f.FormId == "TrailerInterchangeCoverage")?.Other;
                if (trailerOtherContent != null)
                {
                    var premiumsSum = Coverage.RiskCoveragePremium?.TrailerInterchange ?? 0;
                    totalItems.Add(new { Name = $"{Spacer(5)}Trailer Interchange", Value = premiumsSum.ToCurrency() });
                }
            }

            if (ManuscriptPremiumPD > 0 || manuscriptPDPremium != 0) totalItems.Add(new { Name = $"{Spacer(5)}Manuscript Premium", Value = ManuscriptPremiumPD.ToCurrency() });

            if (HasGL || GeneralLiabilityPremium > 0) totalItems.Add(new {Name = "General Liability Premium", Value = GeneralLiabilityPremium.ToCurrency()});

            if (HasCargo || CargoPremium > 0) totalItems.Add(new {Name = "Motor Truck Cargo Premium", Value = CargoPremium.ToCurrency()});

            if(ReportData.Risk.RaterVersionId >= 7)
            {
                if (HasTRC || TrailerInterchangePremium > 0)
                    totalItems.Add(new { Name = "Trailer Interchange Premium", Value = TrailerInterchangePremium.ToCurrency() });

                if (HasNonOwnedTRC || NonOwnedPremium > 0) 
                    totalItems.Add(new { Name = "Non-Owned Trailer Premium", Value = NonOwnedPremium.ToCurrency() }); 
            }

            totalItems.Add(new { Name = "<b>Total Premium</b>", Value = $"${Premium:N2}" });

            var taxDetail = ReportData.DataSets["TaxDetails"].FirstOrDefault(td => td.Id == Coverage.RiskCoveragePremium?.TaxDetailsId);
            var surplusLinesTax = GetProratedPremium("Total", "Surplus Lines", taxDetail?.TotalSLTax ?? 0);

            // Hardcode for OR. Currently the only State with Fire Tax
            // TODO: Future enhancement to support other states with FireTax
            if (PolicyState == "OR")
            {
                decimal onePctOfTax = surplusLinesTax / 2.3m;
                decimal totalPremTaxandFees = onePctOfTax * 100m;
                var orFireTax = totalPremTaxandFees * 0.003m;
                totalItems.Add(new { Name = $"{Spacer(5)}Surplus Lines Tax", Value = $"${surplusLinesTax - orFireTax:N2}" });
                totalItems.Add(new { Name = $"{Spacer(5)}OR Fire Marshal Tax", Value = $"${orFireTax:N2}" });
            }
            else if (PolicyState == "MS")
            {
                decimal onePctOfTax = surplusLinesTax / 7.0m;
                decimal totalPremTaxandFees = onePctOfTax * 100m;
                var msWindpoolFee = totalPremTaxandFees * 0.03m;
                totalItems.Add(new { Name = $"{Spacer(5)}Surplus Lines Tax", Value = $"${surplusLinesTax - msWindpoolFee:N2}" });
                totalItems.Add(new { Name = $"{Spacer(5)}MS Windpool Fee", Value = $"${msWindpoolFee:N2}" });
            }
            else
            {
                totalItems.Add(new { Name = $"{Spacer(5)}Surplus Lines Tax", Value = $"${surplusLinesTax:N2}" });
            }

            var stampingFee = GetProratedPremium("Total", "Stamping Fee", taxDetail?.TotalStampingFee ?? 0);
            totalItems.Add(new { Name = $"{Spacer(5)}Stamping Fee", Value = $"${stampingFee ?? 0:N2}" });

            decimal? totalFees = (Coverage.RiskCoveragePremium?.RiskMgrFeeAL ?? 0) + (Coverage.RiskCoveragePremium?.RiskMgrFeePD ?? 0);

            var fullyEarnedFees = GetProratedPremium("Fee", "Fees", totalFees ?? 0);
            totalItems.Add(new { Name = "Fees (Fully Earned)", Value = "" }); // $"${fullyEarnedFees:N2}"

            var alFee = GetProratedPremium("Fee", "AL", Coverage.RiskCoveragePremium?.RiskMgrFeeAL ?? 0);
            totalItems.Add(new { Name = $"{Spacer(5)}Service Fee - AL", Value = $"${alFee:N2}" });

            var pdFee = GetProratedPremium("Fee", "PD", Coverage.RiskCoveragePremium?.RiskMgrFeePD ?? 0);
            if (pdFee > 0)
            {
                totalItems.Add(new {Name = $"{Spacer(5)}Service Fee - PD", Value = pdFee.ToCurrency()});
            }

            //if (ReportData.Risk.Binding?.PaymentTypesId == 2)
            //    totalItems.Add(new { Name = $"{Spacer(5)}Installment Fee", Value = $"${0:N0}" });
            //if (ReportData.Risk.Binding?.PaymentTypesId == 2) 
            //    totalItems.Add(new { Name = $"{Spacer(5)}Installment Fee", Value = $"${Coverage.RiskCoveragePremium?.TotalInstallmentFee ?? 0:N0}" });

            var apdTable = new List<Dictionary<string, string>>();
            foreach (var item in totalItems)
            {
                apdTable.Add(new Dictionary<string, string>
                {
                    {"Table_Total_Name", item.Name},
                    {"Table_Total_Value", item.Value}
                });
            }
            TableValues.Add("Total_", apdTable);

            var (depositAmount, numberOfInstallments, perInstallment) = InstallmentDetails;
            var paymentMessage = ReportData.Risk.Binding?.PaymentTypesId == 2 
                ? $"${depositAmount:N2} due at inception and {numberOfInstallments} installments of ${perInstallment:N2} each. Installments are due 30 days thereafter prior to the due date." 
                : $"${Coverage.RiskCoveragePremium?.TotalAmounDueFull ?? 0:N2} at inception.";

            FormValues.Add("PaymentDetails", paymentMessage);
        }

        private void SetSurplusLines()
        {
            FormValues.Add("SLBrokerName", _slInfo?.SLBroker);
            FormValues.Add("SLBrokerAddress", $"{_slInfo?.SLBrokerAddressLineOne}, {_slInfo?.SLBrokerAddressCityStateZip}");
            FormValues.Add("SLState", _bind?.SLState);
            FormValues.Add("SLNumber", _bind?.SurplusLIneNum == null ? _slInfo?.LicenseNumber : _bind?.SurplusLIneNum);
            FormValues.Add("SLProducerName", _slInfo?.ProducerName);
            FormValues.Add("SLProducerNumber", _bind?.ProducerSLNumber == null ? _slInfo?.ProducerSLNumber : _bind?.ProducerSLNumber);
        }

        private void SetVehicleAutoLiability()
        {
            var vehicleEntities = new List<Dictionary<string, string>>();
            foreach (var vehicle in Vehicles)
            {
                var effectiveDate = vehicle.EffectiveDate != null ? vehicle.EffectiveDate?.ToString(DateFormat) : PolicyEffectiveDate;
                var expirationDate = vehicle.ExpirationDate != null ? vehicle.ExpirationDate?.ToString(DateFormat) : PolicyExpirationDate;

                var vehicleProperties = new Dictionary<string, string>
                {
                    {"Table_ALVehicle_Make", vehicle.Make},
                    {"Table_ALVehicle_Model", vehicle.Model},
                    {"Table_ALVehicle_Year", vehicle.Year?.ToString()},
                    {"Table_ALVehicle_VIN", vehicle.VIN},
                    {"Table_ALVehicle_Effective",  effectiveDate},
                    {"Table_ALVehicle_Expiration", expirationDate}
                };

                if (!IsEndorsement)
                {
                    vehicleProperties.Add("Table_ALVehicle_ALPremium", vehicle.AutoLiabilityPremium.ToCurrency(0));
                    vehicleProperties.Add("Table_ALVehicle_PDPremium", vehicle.PhysicalDamagePremium.ToCurrency(0));
                }

                vehicleEntities.Add(vehicleProperties);
            }
            TableValues.Add("ALVehicle_", vehicleEntities);
        }

        private void SetVehicleAutoPhysicalDamage()
        {
            var vehicleEntities = new List<Dictionary<string, string>>();
            _isVehiclesHasPD = false;
            foreach (var vehicle in Vehicles)
            {
                var effectiveDate = vehicle.EffectiveDate != null ? vehicle.EffectiveDate?.ToString(DateFormat) : PolicyEffectiveDate;
                var expirationDate = vehicle.ExpirationDate != null ? vehicle.ExpirationDate?.ToString(DateFormat) : PolicyExpirationDate;

                var compDeduct = GetLimitsText(vehicle.CoverageFireTheftDeductibleId, "ComprehensiveLimits");
                var collDeduct = GetLimitsText(vehicle.CoverageCollisionDeductibleId, "CollisionLimits");

                var vehicleProperties = new Dictionary<string, string>
                {
                    {"Table_PDVehicle_Make", vehicle.Make},
                    {"Table_PDVehicle_VIN", vehicle.VIN},
                    {"Table_PDVehicle_Amount", $"${vehicle.StatedAmount:N0}"},
                    {"Table_PDVehicle_CompDed", compDeduct},
                    {"Table_PDVehicle_CollDed", collDeduct},
                    {"Table_PDVehicle_Effective",  effectiveDate},
                    {"Table_PDVehicle_Expiration", expirationDate},
                };

                if (!IsEndorsement)
                {
                    vehicleProperties.Add("Table_PDVehicle_CompPremium", vehicle.ComprehensivePremium.ToCurrency(0));
                    vehicleProperties.Add("Table_PDVehicle_CollPremium", vehicle.CollisionPremium.ToCurrency(0));
                }

                if (vehicle.ComprehensivePremium > 0 || vehicle.CollisionPremium > 0)
                {
                    vehicleEntities.Add(vehicleProperties);
                    if (!string.IsNullOrEmpty(compDeduct) || !string.IsNullOrEmpty(collDeduct))
                        _isVehiclesHasPD = true;
                }
            }
            TableValues.Add("PDVehicle_", vehicleEntities);
        }

        private void SetVehicleMotoTruckCargo()
        {
            var vehicleEntities = new List<Dictionary<string, string>>();
            foreach (var vehicle in Vehicles)
            {
                var effectiveDate = vehicle.EffectiveDate != null ? vehicle.EffectiveDate?.ToString(DateFormat) : PolicyEffectiveDate;
                var expirationDate = vehicle.ExpirationDate != null ? vehicle.ExpirationDate?.ToString(DateFormat) : PolicyExpirationDate;
                var cargoDeduct = vehicle.HasCoverageCargo == true ? GetDeductibleText(Coverage.CargoLimitId, "CargoLimits") : "";
                var refDeduct = vehicle.HasCoverageRefrigeration == true ? GetDeductibleText(Coverage.RefCargoLimitId, "RefLimits") : "";
                var cargoLimit = vehicle.HasCoverageCargo == true ? $"${GetUpperLimitsText(Coverage.CargoLimitId, "CargoLimits")}" : "";
                var refLimit = vehicle.HasCoverageRefrigeration == true ? $"${GetUpperLimitsText(Coverage.RefCargoLimitId, "RefLimits")}" : "";
                var vehicleProperties = new Dictionary<string, string>
                {
                    {"Table_MTCVehicle_VIN", vehicle.VIN},
                    {"Table_MTCVehicle_CargoDeduct", cargoDeduct},
                    {"Table_MTCVehicle_RefDeduct", refDeduct},
                    {"Table_MTCVehicle_CargoLimit", cargoLimit},
                    {"Table_MTCVehicle_RefLimit", refLimit},
                    {"Table_MTCVehicle_Effective", effectiveDate},
                    {"Table_MTCVehicle_Expiration", expirationDate}
                };
                if (!IsEndorsement)
                {
                    vehicleProperties.Add("Table_MTCVehicle_Premium", vehicle.CargoPremium.ToCurrency(0));
                    vehicleProperties.Add("Table_MTCVehicle_CargoPremium", vehicle.CargoPremium.ToCurrency(0));
                    vehicleProperties.Add("Table_MTCVehicle_RefPremium", vehicle.RefrigerationPremium.ToCurrency(0));
                }
                
                vehicleEntities.Add(vehicleProperties);
            }
            TableValues.Add("MTCVehicle_", vehicleEntities);
        }

        private void SetDrivers()
        {
            var driverEntities = new List<Dictionary<string, string>>();
            var drivers = ReportData.Risk.RiskDetailDrivers
                .Where(d => d.Options != null && d.Options.Contains(OptionId))
                .OrderBy(d => d.LicenseNumber);

            if (IsEndorsement)
            {
                drivers = ReportData.Risk.RiskDetailDrivers
                .OrderBy(d => d.LicenseNumber);
            }

            foreach (var driver in drivers)
            {
                var effectiveDate = driver.EffectiveDate != null ? driver.EffectiveDate?.ToString(DateFormat) : PolicyEffectiveDate;
                var expirationDate = driver.ExpirationDate != null ? driver.ExpirationDate?.ToString(DateFormat) : PolicyExpirationDate;
                driverEntities.Add(new Dictionary<string, string>
                {
                    {"Table_Driver_LastName", driver.LastName},
                    {"Table_Driver_FirstName", driver.FirstName},
                    {"Table_Driver_Age", driver.Age},
                    {"Table_Driver_License", driver.LicenseNumber},
                    {"Table_Driver_State", driver.State},
                    {"Table_Driver_Effective", effectiveDate},
                    {"Table_Driver_Expiration", expirationDate},
                });
            }
            TableValues.Add("Driver_", driverEntities);
        }

        private void SetGaragingAddress()
        {
            var garagingEntities = new List<Dictionary<string, string>>();
            var addresses = ReportData.DataSets["Addresses"].Where(a => a.AddressTypeId == 3);
            var i = 1;
            foreach (var address in addresses)
            {
                garagingEntities.Add(new Dictionary<string, string>
                {
                    {"Table_Garage_Number", i.ToString()},
                    {"Table_Garage_Address", address.Address.StreetAddress1},
                    {"Table_Garage_City", address.Address.City},
                    {"Table_Garage_State", address.Address.State},
                    {"Table_Garage_Zip", address.Address.ZipCode}
                });
                i++;
            }
            TableValues.Add("Garage_", garagingEntities);
        }

        private void SetForms()
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

            var forms = ReportData.Risk.RiskForms
                .Where(rf =>
                    rf.IsSelected.GetValueOrDefault() 
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
                if (form.FormId == "SurplusLinesNotice") form.Form.FormNumber = _bind.SurplusLIneNum;

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

            TableValues.Add("Form_", formTable);
        }

        private static string Spacer(int count)
        {
            var whiteSpaces = "";
            for (var i = 0; i < count; i++)
            {
                whiteSpaces += " ";
            }
            return whiteSpaces;
        }

        public decimal Premium => GetProratedPremium("Total", "Total", Coverage.RiskCoveragePremium?.Premium ?? 0);

        private decimal AutoLiabilityPremium
        {
            get
            {
                var premium = GetProratedPremium("Vehicle", "Auto Liability", Coverage.RiskCoveragePremium?.AutoLiabilityPremium ?? 0);
                if (!IsEndorsement) return premium;
                var additionalPremium = (Coverage.RiskCoveragePremium?.AdditionalInsuredPremium ?? 0) +
                        (Coverage.RiskCoveragePremium?.WaiverOfSubrogationPremium ?? 0) +
                        (Coverage.RiskCoveragePremium?.PrimaryNonContributoryPremium ?? 0);
                var manuscriptALPremium = ManuscriptPremiumAL;
                return premium + additionalPremium + manuscriptALPremium;
            }
        }

        private decimal PhysicalDamagePremium
        {
            get
            {
                var premium = GetProratedPremium("Vehicle", "Physical Damage", Coverage.RiskCoveragePremium?.PhysicalDamagePremium ?? 0);
                if (!IsEndorsement) return premium;
                return premium + (Coverage.RiskCoveragePremium?.HiredPhysicalDamage ?? 0)
                                  + (Coverage.RiskCoveragePremium?.TrailerInterchange ?? 0) 
                                  + ManuscriptPremiumPD;
            }
        }

        private decimal GeneralLiabilityPremium => GetProratedPremium("Policy", "General Liability", Coverage.RiskCoveragePremium?.GeneralLiabilityPremium ?? 0);
        private decimal TrailerInterchangePremium => GetProratedPremium("Policy", "Trailer Interchange Premium", Coverage.RiskCoveragePremium?.TrailerInterchangeAL ?? 0);
        private decimal NonOwnedPremium => GetProratedPremium("Policy", "Non-Owned Trailer Premium", Coverage.RiskCoveragePremium?.NonOwnedTrailerPremium ?? 0);
        private decimal CargoPremium => GetProratedPremium("Vehicle", "Motor Truck Cargo", Coverage.RiskCoveragePremium?.CargoPremium ?? 0);
        private decimal ManuscriptPremiumAL => IsEndorsement ? (Coverage.RiskCoveragePremium?.ProratedALManuscriptPremium ?? Coverage.RiskCoveragePremium?.ALManuscriptPremium) ?? 0
            : ReportData.Risk.RiskManuscripts.Where(m => m.IsSelected.GetValueOrDefault() && m.PremiumType == "AL").Sum(m => m.Premium);
        private decimal ManuscriptPremiumPD => IsEndorsement ? (Coverage.RiskCoveragePremium?.ProratedPDManuscriptPremium ?? Coverage.RiskCoveragePremium?.PDManuscriptPremium) ?? 0
            : ReportData.Risk.RiskManuscripts.Where(m => m.IsSelected.GetValueOrDefault() && m.PremiumType == "PD").Sum(m => m.Premium);

        private decimal GetProratedPremium(string type, string description, decimal annualValue)
        {
            if (ReportData.Endorsement?.PremiumChange != null)
            {
                var changeList = new List<EndorsementPremiumChangeDTO>();
                switch (type)
                {
                    case "Vehicle":
                        changeList = ReportData.Endorsement?.PremiumChange.VehicleChangeList;
                        break;
                    case "Policy":
                        changeList = ReportData.Endorsement?.PremiumChange.PolicyChangeList;
                        break;
                    case "Total":
                        changeList = ReportData.Endorsement?.PremiumChange.TotalPremiumChangeList;
                        break;
                    case "Fee":
                        changeList = ReportData.Endorsement?.PremiumChange.FeesChangeList;
                        break;
                }

                var endorsementChange = changeList.FirstOrDefault(cl => cl.Description.Contains(description));
                return endorsementChange?.EndorsementPendingChange?.PremiumAfterEndorsement ?? 0;
            }
            return annualValue;
        }

        private (decimal depositAmount, int numberOfInstallments, decimal perInstallment) InstallmentDetails
        {
            get
            {
                decimal depositAmount;
                var numberOfInstallments = 0;
                decimal perInstallment;
                if (IsEndorsement)
                {
                    var installments = ReportData.DataSets["InstallmentSchedule"].Cast<InstallmentAndInvoiceDTO>().ToList();
                    depositAmount = installments.FirstOrDefault(i => i.InstallmentType == "Deposit")?.Premium ?? 0;
                    var futureInstallments = installments.Where(i => i.Status == "Future").ToList();
                    numberOfInstallments = futureInstallments.Count - 1;
                    perInstallment = futureInstallments.Skip(1).FirstOrDefault()?.TotalBilled ?? 0;
                }
                else
                {
                    depositAmount = Coverage.RiskCoveragePremium?.DepositAmount ?? 0;
                    numberOfInstallments = Coverage.NumberOfInstallments.GetValueOrDefault();
                    perInstallment = Coverage.RiskCoveragePremium?.PerInstallment ?? 0;
                }

                return (depositAmount, numberOfInstallments, perInstallment);

            }
        }

        private string SetAppliedCompanyNameByState(dynamic surplusLinesBroker, string state)
        {
            string slBrokerCompanyName = string.Empty;

            if (surplusLinesBroker?.Id == CreditedOffice.Applied.Description)
            {
                switch (state)
                {
                    case "CA":
                    case "FL":
                    case "MA":
                    case "NY":
                    case "TX":
                    case "WA":
                        slBrokerCompanyName = "Vale Insurance Partners";
                        break;
                    default:
                        slBrokerCompanyName = "Rivington Partners";
                        break;
                }
            }
            else
            {
                slBrokerCompanyName = surplusLinesBroker?.Description;
            }

            return slBrokerCompanyName;
        }

        private Tuple<string, string> ParseTrailerLimitValue(string trailerLimitValue = "")
        {
            string pattern = @"(\d+) WITH (\d+)";

            MatchCollection matches = Regex.Matches(trailerLimitValue, pattern);
            var matched = matches.FirstOrDefault();

            if (matched != null)
            {
                var value1 = matched?.Groups[1]?.Value;
                var value2 = matched?.Groups[2]?.Value;

                if (value1 != null)
                    value1 = int.Parse(matched.Groups[1].Value).ToString("N0");

                if (value2 != null)
                    value2 = int.Parse(matched.Groups[2].Value).ToString("N0");

                return Tuple.Create(value1 ?? "0", value2 ?? "0");
            }

            return Tuple.Create("", "");
        }
    }
}