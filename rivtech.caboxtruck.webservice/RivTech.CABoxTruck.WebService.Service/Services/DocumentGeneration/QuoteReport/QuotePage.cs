using System;
using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.QuoteReport
{
    public class QuotePage : SinglePage, IPage
    {
        private decimal[] _optionTotals;
        public string TemplateName => "Quote.docx";
        public int PageOrder => 1;

        public QuotePage(ReportData reportData) : base(reportData)
        {
        }

        public Form GetData()
        {
            var formData = new Form
            {
                PageOrder = PageOrder,
                TemplateName = SetTemplate(),
                FormValues = GetFormValues(),
                TableValues = GetTableValues()
            };
            SetPremiumTotals(formData);
            return formData;
        }

        private string SetTemplate()
        {
            if (ReportData.Risk.RiskCoverages.Count == 1)
                return "documenttemplate/Quote/Quote1optn.docx";

            if (ReportData.Risk.RiskCoverages.Count == 2)
                return "documenttemplate/Quote/Quote2optn.docx";

            return $"documenttemplate/Quote/{TemplateName}";
        }

        private void SetPremiumTotals(Form formData)
        {
            for (var i = 0; i < ReportData.Risk.RiskCoverages.Count; i++)
            {
                var totalValue = ReportData.Risk.RiskCoverages[i].RiskCoveragePremium?.Premium > 0 ? $"${_optionTotals[i]:N2}" : "";
                formData.FormValues.Add($"PremiumTotalOpt{i + 1}", totalValue);
            }
        }

        private Dictionary<string, string> GetFormValues()
        {
            const string dateFormat = "MMMM d, yyyy";
            var agentName = ReportData.Risk.BrokerInfo?.Agent?.Entity?.FullName;
            var agencyName = ReportData.Risk.BrokerInfo?.Agency?.Entity?.CompanyName;
            var applicant = ReportData.Risk.NameAndAddress;
            var policyEffectiveDate = ReportData.Risk.BrokerInfo?.EffectiveDate?.ToString(dateFormat);
            var quoteExpirationDate = DateTime.Now.AddDays(30).ToString(dateFormat);
            var businessType = ReportData.DataSets["BusinessTypes"]
                .FirstOrDefault(bt => bt.Id == ReportData.Risk.BusinessDetails?.BusinessTypeId)?.Description;
            var policyContact = ReportData.DataSets["PolicyContacts"].FirstOrDefault(pc => pc.Position == "Product Underwriter" && pc.Id == ReportData.Risk.ProductUWUserId);
            var creditedOffice = ReportData.DataSets["CreditedOffice"]
                .FirstOrDefault(bt => bt.Id == ReportData.Risk.Binding?.CreditedOfficeId)?.Description;
            var formValues = new Dictionary<string, string>
            {
                {"QuoteDate", DateTime.Now.ToString(dateFormat)},
                {"AgentName", agentName},
                {"AgentName2", agentName},
                {"AgencyName", agencyName},
                {"AgencyName2", agencyName},
                {"ProposedInsured", ReportData.Risk.NameAndAddress?.BusinessName},
                {"PolicyEffectiveDate", policyEffectiveDate},
                {"PolicyEffectiveDate2", policyEffectiveDate},
                {"PolicyEffectiveDate3", policyEffectiveDate},
                {"UWName", policyContact?.Name},
                {"UWEmail", policyContact?.Email},
                {"QuoteNumber", ReportData.Risk.SubmissionNumber}, // TBC
                {"QuoteNumber2", ReportData.Risk.SubmissionNumber}, // TBC
                {"QuoteType", "New"}, // TBD (New/Renewal)
                {"Industry", businessType},
                {"QuoteDateTime", DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt")},
                {"QuoteDateTime2", DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt")},
                {"QuoteExpirationDate", quoteExpirationDate},
                {"QuoteExpirationDate2", quoteExpirationDate},
                {"PresentedBy", creditedOffice},

                //APPLICANT
                {"ApplicantName", applicant?.BusinessName},
                {"ApplicantName2", applicant?.BusinessName},
                {"ApplicantAddress1", applicant?.BusinessAddress},
                {"ApplicantAddress2", applicant?.BusinessAddress},
                {"ApplicantCity", applicant?.City},
                {"ApplicantState", applicant?.State},
                {"ApplicantZipCode", applicant?.ZipCode},

                {"DisclaimerText", GetDisclaimerText()},

                // Fees
                {"FeesNote", GetFeesNotes()},
            };

            return formValues;
        }

        private Dictionary<string, List<Dictionary<string, string>>> GetTableValues()
        {
            var tableValues = new Dictionary<string, List<Dictionary<string, string>>>
            {
                {"Premium", GetFromPremiums() },
                {"BindRequirement", GetFromBindRequirements() },
                {"QuoteCondition", GetFromQuoteConditions() },
                {"Coverage", GetFromCoverages() },
                {"PolPrem", GetFromPolicyPremiums() },
                {"PolFee", GetFromPolicyFees() },
                {"PolTotal", GetFromPolicyTotals() },
                {"PolPayFull", GetFromPolicyPaymentOptions() },
                {"PolPayInst", GetFromPolicyPaymentInstalls() },
                {"PolPayInstPrem", GetFromPolicyPaymentInstallPremiums() }
            };

            return tableValues;
        }

        private List<Dictionary<string, string>> GetFromPolicyPremiums()
        {
            var isRaterVersionSupport = ReportData.Risk.RaterVersionId >= 7;
            var premiumEntries = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> {{"Table_PolPremDesc", "Liability" } },
                new Dictionary<string, string> {{"Table_PolPremDesc", "Cargo" } },
                new Dictionary<string, string> {{"Table_PolPremDesc", "Physical Damage" } },
                new Dictionary<string, string> {{"Table_PolPremDesc", "Trailer Interchange" } },
                new Dictionary<string, string> {{"Table_PolPremDesc", "Non-Owned Trailer" } },
                new Dictionary<string, string> {{"Table_PolPremDesc", "Vehicle Level Total" } },
                new Dictionary<string, string> {{"Table_PolPremDesc", "General Liability" } },
            };

            var coverages = ReportData.Risk.RiskCoverages;
            for (var i = 0; i < 4; i++)
            {
                var bookmark = $"Table_PolPremOption{i + 1}";
                var liability = "";
                var cargo = "";
                var physicalDamage = "";
                var vehicleLevelTotal = "";
                var generalLiability = "";
                var trailerCoverage = "";
                var nonOwnedTrailerCoverage = "";

                if (HasPremiumForOption(i))
                {
                    liability = $"${coverages[i].RiskCoveragePremium?.AutoLiabilityPremium ?? 0:N2}";
                    cargo = $"${coverages[i].RiskCoveragePremium?.CargoPremium ?? 0:N2}";
                    physicalDamage = $"${coverages[i].RiskCoveragePremium?.PhysicalDamagePremium ?? 0:N2}";
                    vehicleLevelTotal = $"${coverages[i].RiskCoveragePremium?.VehicleLevelPremium ?? 0:N2}";
                    generalLiability = $"${coverages[i].RiskCoveragePremium?.GeneralLiabilityPremium ?? 0:N2}";
                    trailerCoverage = $"${coverages[i].RiskCoveragePremium?.TrailerInterchangeAL ?? 0:N2}";
                    nonOwnedTrailerCoverage = $"${coverages[i].RiskCoveragePremium?.NonOwnedTrailerPremium ?? 0:N2}";
                }

                var hasApd = coverages[0].CompDeductibleId > LimitConstants.ComprehensiveNoCoverageId
                             || coverages[0].FireDeductibleId > LimitConstants.ComprehensiveNoCoverageId
                             || coverages[0].CollDeductibleId > LimitConstants.CollisionNoCoverageId;

                premiumEntries[0].Add(bookmark, liability);
                premiumEntries[1].Add(bookmark, coverages[0].CargoLimitId == 53 ? "NA" : cargo);
                premiumEntries[2].Add(bookmark, !hasApd ? "NA" : physicalDamage);
                premiumEntries[3].Add(bookmark, coverages[0].TrailerInterchangeLimitId == LimitConstants.TrailerNoCoverageId ? "NA" : trailerCoverage);
                premiumEntries[4].Add(bookmark, coverages[0].NonOwnedTrailerLimitId == LimitConstants.NonOwnedTrailerNoCoverageId ? "NA" : nonOwnedTrailerCoverage);
                premiumEntries[5].Add(bookmark, vehicleLevelTotal);
                premiumEntries[6].Add(bookmark, coverages[0].GLBILimitId == 87 || coverages[0].GLBILimitId == 882 ? "NA" : generalLiability);
            }
            
            if (!isRaterVersionSupport)
                premiumEntries.RemoveRange(3, 2);

            premiumEntries.RemoveAll(HasNoCoveragePremium);
            return premiumEntries;
        }

        private QuoteTableEntry<string> CreateFeeValuesEntry(string description, string propertyName)
        {
            var fee = new QuoteTableEntry<string> {Description = description};
            string[] feeValues = {"","",""};
            var coverages = ReportData.Risk.RiskCoverages;
            for (var i = 0; i < 4; i++)
            {
                if (HasPremiumForOption(i))
                {
                    var sourceObject = coverages[i].RiskCoveragePremium;
                    var feeValue = sourceObject.GetType().GetProperty(propertyName)?.GetValue(sourceObject, null) ?? 0;

                    feeValues[i] = $"${feeValue:N2}"; //NEWCABT-3223: 5e. Include Cents
                }
            }
            fee.Option1Value = feeValues[0];
            fee.Option2Value = feeValues[1];
            fee.Option3Value = feeValues[2];

            return fee;
        }

        private List<Dictionary<string, string>> GetFromPolicyFees()
        {
            var fees = new List<QuoteTableEntry<string>>();

            var coverages = ReportData.Risk.RiskCoverages;

            if (coverages.Any(cov => (cov.RiskCoveragePremium?.AdditionalInsuredPremium ?? 0) > 0)) fees.Add(CreateFeeValuesEntry("Additional Insured Charge", "AdditionalInsuredPremium"));
            if (coverages.Any(cov => (cov.RiskCoveragePremium?.WaiverOfSubrogationPremium ?? 0) > 0)) fees.Add(CreateFeeValuesEntry("Waiver of Subrogation Charge", "WaiverOfSubrogationPremium"));
            if (coverages.Any(cov => (cov.RiskCoveragePremium?.PrimaryNonContributoryPremium ?? 0) > 0)) fees.Add(CreateFeeValuesEntry("Primary & Noncontributory Charge", "PrimaryNonContributoryPremium"));

            if (coverages.Any(cov => (cov.RiskCoveragePremium?.RiskMgrFeeAL ?? 0) > 0)) fees.Add(CreateFeeValuesEntry("Service Fee - AL", "RiskMgrFeeAL"));
            if (coverages.Any(cov => (cov.RiskCoveragePremium?.RiskMgrFeePD ?? 0) > 0)) fees.Add(CreateFeeValuesEntry("Service Fee - PD", "RiskMgrFeePD"));
            if (coverages.Any(cov => (cov.RiskCoveragePremium?.InstallmentFee ?? 0) > 0)) fees.Add(CreateFeeValuesEntry("Installment Fee", "TotalInstallmentFee"));

            return fees.Select(fee => new Dictionary<string, string>
                {
                    {"Table_PolFeeDesc", fee.Description}, 
                    {"Table_PolFeeOption1", fee.Option1Value}, 
                    {"Table_PolFeeOption2", fee.Option2Value}, 
                    {"Table_PolFeeOption3", fee.Option3Value},
                })
                .ToList();
        }

        private List<Dictionary<string, string>> GetFromPolicyTotals()
        {
            var tableEntries = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> {{"Table_PolTotalDesc", "Premium" } },
                new Dictionary<string, string> {{"Table_PolTotalDesc", "Fees" } },
                new Dictionary<string, string> {{"Table_PolTotalDesc", $"Taxes ({ReportData.DataSets["StateTax"].FirstOrDefault()})" } },
                new Dictionary<string, string> {{"Table_PolTotalDesc", $"Stamping Fee ({ReportData.DataSets["SFTax"].FirstOrDefault()})" } }
            };

            _optionTotals = new decimal[] {0, 0, 0};

            var coverages = ReportData.Risk.RiskCoverages;
            for (var i = 0; i < 4; i++)
            {
                var bookmark = $"Table_PolTotalOption{i + 1}";
                var premium = "";
                var fees = "";
                var taxes = "";
                var stampingFee = "";

                if (HasPremiumForOption(i))
                {
                    var totalAmountDue = 0m;
                    var premiumValue = coverages[i].RiskCoveragePremium?.Premium ?? 0;
                    premium = $"${premiumValue:N2}";

                    var feesValue = coverages[i].RiskCoveragePremium?.Fees ?? 0;
                    fees = $"${feesValue:N2}";

                    var taxDetail = ReportData.DataSets["TaxDetails"].FirstOrDefault(td => td.Id == coverages[i].RiskCoveragePremium?.TaxDetailsId);

                    var taxesValue = taxDetail?.TotalSLTax ?? 0;
                    taxes = $"${taxesValue:N2}";

                    var stampingFeeValue = taxDetail?.TotalStampingFee ?? 0;
                    stampingFee = $"${stampingFeeValue:N2}";

                    totalAmountDue = premiumValue + feesValue + taxesValue + stampingFeeValue;

                    var totalAmountDueInstallments = coverages[i].RiskCoveragePremium?.TotalAmountDueInstallment ?? 0;
                    _optionTotals[i] = totalAmountDueInstallments != 0 ? totalAmountDueInstallments : totalAmountDue;
                }

                tableEntries[0].Add(bookmark, premium);
                tableEntries[1].Add(bookmark, fees);
                tableEntries[2].Add(bookmark, taxes);
                tableEntries[3].Add(bookmark, stampingFee);
            }

            return tableEntries;
        }

        private List<Dictionary<string, string>> GetFromPolicyPaymentOptions()
        {
            var tableEntries = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> {{"Table_PolPayFullDesc", "Deposit" } },
            };

            var coverages = ReportData.Risk.RiskCoverages;
            for (var i = 0; i < 4; i++)
            {
                var bookmark = $"Table_PolPayFullOption{i + 1}";
                var fullPolicyAmount = "";
                if (HasPremiumForOption(i))
                {
                    fullPolicyAmount = $"${coverages[i].RiskCoveragePremium?.TotalAmounDueFull ?? 0:N2}";
                }
                tableEntries[0].Add(bookmark, fullPolicyAmount);
            }

            return tableEntries;
        }

        private List<Dictionary<string, string>> GetFromPolicyPaymentInstalls()
        {
            var tableEntries = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> {{"Table_PolPayInstDesc", "Deposit" } },
                new Dictionary<string, string> {{"Table_PolPayInstDesc", "# of Installments" } },
                new Dictionary<string, string> {{"Table_PolPayInstDesc", "Installment Total Due" } }
            };

            var coverages = ReportData.Risk.RiskCoverages;
            for (var i = 0; i < 4; i++)
            {
                var bookmark = $"Table_PolPayInstOption{i + 1}";
                var depositAmount = "";
                var numberOfInstallments = "";
                var installmentTotalDue = "";
                
                if (HasPremiumForOption(i))
                {
                    depositAmount = $"${coverages[i].RiskCoveragePremium.DepositAmount ?? 0:N2}";
                    numberOfInstallments = coverages[i].NumberOfInstallments?.ToString();
                    installmentTotalDue = $"${coverages[i].RiskCoveragePremium?.PerInstallment ?? 0:N2}";
                   
                }
                tableEntries[0].Add(bookmark, depositAmount);
                tableEntries[1].Add(bookmark, numberOfInstallments ?? "");
                tableEntries[2].Add(bookmark, installmentTotalDue);
            }

            return tableEntries;
        }

        private List<Dictionary<string, string>> GetFromPolicyPaymentInstallPremiums()
        {
            var tableEntries = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> {{"Table_PolPayInstPremDesc", "Premium" } },
                new Dictionary<string, string> {{"Table_PolPayInstPremDesc", "Fees" } },
                new Dictionary<string, string> {{"Table_PolPayInstPremDesc", "Taxes" } }
            };

            var coverages = ReportData.Risk.RiskCoverages;
            for (var i = 0; i < 4; i++)
            {
                var bookmark = $"Table_PolPayInstPremOpt{i + 1}";
                var premium = "";
                var fees = "";
                var taxes = "";
               
                if (HasPremiumForOption(i))
                {
                    premium = $"${coverages[i].RiskCoveragePremium?.PerInstallmentPremium ?? 0:N2}";
                    fees = $"${coverages[i].RiskCoveragePremium?.PerInstallmentFee ?? 0:N2}";
                    taxes = $"${coverages[i].RiskCoveragePremium?.PerInstallmentTax ?? 0:N2}";
                }
                tableEntries[0].Add(bookmark, premium);
                tableEntries[1].Add(bookmark, fees);
                tableEntries[2].Add(bookmark, taxes);
            }

            return tableEntries;
        }

        private List<Dictionary<string, string>> GetFromPremiums()
        {
            var premiumEntries = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "Table_PremiumItem", "Estimated Premium" } },
                new Dictionary<string, string> { { "Table_PremiumItem", "Liability Commission %" } },
                new Dictionary<string, string> { { "Table_PremiumItem", "Liability Commission $" } },
                new Dictionary<string, string> { { "Table_PremiumItem", "Physical Damage Commission %" } },
                new Dictionary<string, string> { { "Table_PremiumItem", "Physical Damage Commission $" } }
            };

            var coverages = ReportData.Risk.RiskCoverages;
            for (var i = 0; i < 4; i++)
            {
                var bookmark = $"Table_PremiumOption{i + 1}";
                var premium = "";
                var alCommission = "";
                var alCommissionAmount = "";
                var pdCommission = "";
                var pdCommissionAmount = "";

                if (HasPremiumForOption(i))
                {
                    var premiumValue = coverages[i].RiskCoveragePremium?.Premium ?? 0;
                    premium = $"${premiumValue:N2}";

                    var alCommissionValue = (string.IsNullOrEmpty(coverages[i].BrokerCommisionAL)) ? 0 : Convert.ToDecimal(coverages[i].BrokerCommisionAL);
                    alCommission = $"{alCommissionValue:0.00}%";
                    var alPremiumGroup = (coverages[i].RiskCoveragePremium?.AutoLiabilityPremium ?? 0)
                                         + (coverages[i].RiskCoveragePremium?.GeneralLiabilityPremium ?? 0)
                                         + (coverages[i].RiskCoveragePremium?.CargoPremium ?? 0)
                                         + (coverages[i].RiskCoveragePremium?.TrailerInterchangeAL ?? 0)
                                         + (coverages[i].RiskCoveragePremium?.NonOwnedTrailerPremium ?? 0);
                    alCommissionAmount = $"${alCommissionValue / 100 * alPremiumGroup:N2}";

                    var pdCommissionValue = string.IsNullOrEmpty(coverages[i].BrokerCommisionPD) ? 0 : Convert.ToDecimal(coverages[i].BrokerCommisionPD);
                    pdCommission = $"{pdCommissionValue:0.00}%";
                    var pdPremiumGroup = coverages[i].RiskCoveragePremium?.PhysicalDamagePremium ?? 0;
                    pdCommissionAmount = $"${pdCommissionValue / 100 * pdPremiumGroup:N2}";
                }

                premiumEntries[0].Add(bookmark, premium);
                premiumEntries[1].Add(bookmark, alCommission);
                premiumEntries[2].Add(bookmark, alCommissionAmount);
                premiumEntries[3].Add(bookmark, pdCommission);
                premiumEntries[4].Add(bookmark, pdCommissionAmount);
            }

            return premiumEntries;
        }

        private List<Dictionary<string, string>> GetFromBindRequirements()
        {
            var bindingRequirements = ReportData.Risk.BindingRequirements.Where(br => br.BindStatusId == BindStatus.Pending.Id);

            var bindRequirementItems = new List<string>();
            foreach (var requirement in bindingRequirements)
            {
                var bindRequirement = ReportData.DataSets["BindRequirementsOption"]
                    .FirstOrDefault(l => l.Id == requirement.BindRequirementsOptionId);
                bindRequirementItems.Add(bindRequirement?.Description == "Other" ? requirement.Describe : bindRequirement?.Description);
            }

            var tableValuesBindRequirements = new List<Dictionary<string, string>>();
            foreach (var item in bindRequirementItems.OrderBy(b => b))
            {
                tableValuesBindRequirements.Add(new Dictionary<string, string>
                {
                    { "Table_BindRequirementItem", $"{item}" }
                });
            }
            return tableValuesBindRequirements;
        }

        private List<Dictionary<string, string>> GetFromQuoteConditions()
        {
            var tableValuesQuoteConditions = new List<Dictionary<string, string>>();

            var quoteConditions = ReportData.Risk.QuoteConditions;

            var quoteConditionItems = new List<string>();
            foreach (var condition in quoteConditions)
            {
                var quoteCondition = ReportData.DataSets["QuoteConditionsOption"]
                    .FirstOrDefault(cc => cc.Id == condition.QuoteConditionsOptionId);

                if (quoteCondition?.Id == QuoteConditionOptionsConstants.MinimumEarned)
                {
                    //NEWCABT-3223: 5a. Minimum Earned
                    string description = quoteCondition?.Description;

                    int to = description.IndexOf("to") + 2;
                    int percent = description.IndexOf("%");
                    string placeholder = description[to..percent].Trim();

                    description = (ReportData.Risk.Binding.MinimumEarned % 1 == 0) ?
                        description.Replace(placeholder, $"{ReportData.Risk.Binding.MinimumEarned:N0}") :
                        description.Replace(placeholder, $"{ReportData.Risk.Binding.MinimumEarned:N1}");
                    quoteCondition.Description = description;
                }

                if (quoteCondition?.Id == QuoteConditionOptionsConstants.SymbolTen)
                {
                    if (ReportData.Risk.RiskCoverages[0].ALSymbolId == Symbol.Symbol10_2.Id)
                        quoteCondition.Description = quoteCondition.Description.Replace("2/8/9", "7/8/9");

                    if (ReportData.Risk.RiskCoverages[0].ALSymbolId == Symbol.Symbol10_1.Id)
                        quoteCondition.Description = quoteCondition.Description.Replace("7/8/9", "2/8/9");
                }

                quoteConditionItems.Add(quoteCondition?.Id == QuoteConditionOptionsConstants.Other ? condition.Describe : quoteCondition?.Description);
            }

            foreach (var item in quoteConditionItems.OrderBy(q => q))
            {
                tableValuesQuoteConditions.Add(new Dictionary<string, string>
                {
                    { "Table_QuoteConditionItem", $"{item}" }
                });
            }

            return tableValuesQuoteConditions;
        }

        private static string GetDisclaimerText()
        {
            // TBD source
            return "Disclaimer Text";
        }

        private string GetFeesNotes()
        {
            var state = ReportData.Risk.NameAndAddress?.State;
            switch(state)
            {
                case "FL":
                case "AL":
                    return "";
                default:
                    return "Note: Midterm vehicle transactions subject to $100 per vehicle Service fee for AL, $50 for APD";
            }
        }

        private List<Dictionary<string, string>> GetFromCoverages()
        {
            bool hasPollution = false;

            var coverageEntries = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> {{ "Table_CoverageItem", "Limit of Liability" }},
                //new Dictionary<string, string> {{ "Table_CoverageItem", "Property Damage" } },  // not MVP
                new Dictionary<string, string> {{ "Table_CoverageItem", "Medical Payments" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Personal Injury Protection" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Uninsured Motorists BI" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Underinsured Motorists BI" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Uninsured Motorists PD" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Underinsured Motorists PD" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Deductible Per Accident" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Motor Truck Cargo" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Refrigerated Cargo Limits" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "General Liability" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Vehicles with Physical Damage" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Collision Deductible" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Comprehensive Deductible" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Fire/Theft Deductible" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Number of Drivers" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Number of Vehicles" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Trailer Interchange" } },
                new Dictionary<string, string> {{ "Table_CoverageItem", "Non-Owned Trailer" } },
            };

            var coverages = ReportData.Risk.RiskCoverages;
            if (coverages.Any(c => c.PollutionCovLimitId != 879))
            {
                coverageEntries.Insert(15, new Dictionary<string, string> { { "Table_CoverageItem", "Pollution Coverage" } });
                hasPollution = true;
            }


                var isRaterVersionSupport = ReportData.Risk.RaterVersionId >=7;

            List<string> noCovList = new List<string> { "NONE", "NA", "No Coverage" };
            for (var i = 0; i < 4; i++)
            {
                var bookmarkOption = $"Table_CoverageOption{i + 1}";
                var bookmarkOptionSymbol = $"Table_CoverageOptionSymbol{i + 1}";
                
                if (HasPremiumForOption(i))
                {
                    int idx = 0;
                    var liabilityLimit = GetLimitsText(coverages[i].AutoBILimitId, "AutoBILimits") ?? "";
                    coverageEntries[idx].Add(bookmarkOption, liabilityLimit); idx++;
                    //coverageEntries[1].Add(bookmarkOption, liabilityLimit.Contains("/") ? GetLimitsText(coverages[i].AutoPDLimitId, "AutoPDLimits") : "");

                    coverageEntries[idx].Add(bookmarkOption, coverages[i].MedPayLimitId == 110 ? "NONE" : GetLimitsText(coverages[i].MedPayLimitId, "MedPayLimits")); idx++;
                    coverageEntries[idx].Add(bookmarkOption, coverages[i].PIPLimitId == 130 ? "NONE" : GetLimitsText(coverages[i].PIPLimitId, "PipLimits")); idx++;
                    coverageEntries[idx].Add(bookmarkOption, coverages[i].UMBILimitId == 388 ? "NONE" : GetLimitsText(coverages[i].UMBILimitId, "UMBILimits")); idx++;
                    coverageEntries[idx].Add(bookmarkOption, coverages[i].UIMBILimitId == 243 ? "NONE" : GetLimitsText(coverages[i].UIMBILimitId, "UIMBILimits")); idx++;
                    coverageEntries[idx].Add(bookmarkOption, coverages[i].UMPDLimitId == 509 ? "NONE" : GetLimitsText(coverages[i].UMPDLimitId, "UMPDLimits")); idx++;
                    coverageEntries[idx].Add(bookmarkOption, coverages[i].UIMPDLimitId == 298 ? "NONE" : GetLimitsText(coverages[i].UIMPDLimitId, "UIMPDLimits")); idx++;
                    coverageEntries[idx].Add(bookmarkOption, GetLimitsText(coverages[i].LiabilityDeductibleId, "LiabilityDeductibleLimits")); idx++;
                    coverageEntries[idx].Add(bookmarkOption, coverages[i].CargoLimitId == 53 ? "NONE" : GetLimitsText(coverages[i].CargoLimitId, "CargoLimits")); idx++;
                    coverageEntries[idx].Add(bookmarkOption, coverages[i].RefCargoLimitId == 157 ? "NONE" : GetLimitsText(coverages[i].RefCargoLimitId, "RefLimits")); idx++;
                    coverageEntries[idx].Add(bookmarkOption, coverages[i].GLBILimitId == 87 ? "NONE" : GetLimitsText(coverages[i].GLBILimitId, "GlLimits")); idx++;
                    coverageEntries[idx].Add(bookmarkOption, GetVehiclesWithPhysicalDamage(i)); idx++;
                    coverageEntries[idx].Add(bookmarkOption, coverages[i].CollDeductibleId == 65 ? "NONE" : GetLimitsText(coverages[i].CollDeductibleId, "CollisionLimits")); idx++;
                    coverageEntries[idx].Add(bookmarkOption, coverages[i].CompDeductibleId == 76 ? "NONE" : GetLimitsText(coverages[i].CompDeductibleId, "ComprehensiveLimits")); idx++;
                    coverageEntries[idx].Add(bookmarkOption, coverages[i].FireDeductibleId == 76 ? "NONE" : GetLimitsText(coverages[i].FireDeductibleId, "ComprehensiveLimits")); idx++;

                    if (hasPollution)
                    {
                        coverageEntries[idx].Add(bookmarkOption, coverages[i].PollutionCovLimitId == 879 ? "" : "Included"); idx++;
                    }
                    coverageEntries[idx].Add(bookmarkOption, GetDriversForOption(i)); idx++;
                    coverageEntries[idx].Add(bookmarkOption, GetVehiclesForOption(i)); idx++;


                    //Symbols
                    int symbolidx = 0;
                    coverageEntries[0].Add(bookmarkOptionSymbol, GetSymbolsText(coverages[i].ALSymbolId)); symbolidx++;
                    //coverageEntries[1].Add(bookmarkOptionSymbol, liabilityLimit.Contains("/") ? GetSymbolsText(coverages[0].PDSymbolId) : "");
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, noCovList.Contains(GetLimitsText(coverages[i].MedPayLimitId, "MedPayLimits")) ? "" : 
                        GetSymbolsText(coverages[0].MedPayLimitSymbolId)); symbolidx++;
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, noCovList.Contains(GetLimitsText(coverages[i].PIPLimitId, "PipLimits")) ? "" :
                        GetSymbolsText(coverages[0].PIPLimitSymbolId)); symbolidx++;
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, noCovList.Contains(GetLimitsText(coverages[i].UMBILimitId, "UMBILimits")) ? "" :
                        GetSymbolsText(coverages[0].UMBILimitSymbolId)); symbolidx++;
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, noCovList.Contains(GetLimitsText(coverages[i].UIMBILimitId, "UIMBILimits")) ? "" : 
                        GetSymbolsText(coverages[0].UIMBILimitSymbolId)); symbolidx++;
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, noCovList.Contains(GetLimitsText(coverages[i].UMPDLimitId, "UMPDLimits")) ? "" :
                        GetSymbolsText(coverages[0].UMPDLimitSymbolId)); symbolidx++;
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, noCovList.Contains(GetLimitsText(coverages[i].UIMPDLimitId, "UIMPDLimits")) ? "" :
                        GetSymbolsText(coverages[0].UIMPDLimitSymbolId)); symbolidx++;
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, noCovList.Contains(GetLimitsText(coverages[i].LiabilityDeductibleId, "LiabilityDeductibleLimits")) ? "" :
                        GetSymbolsText(coverages[0].LiabilityDeductibleSymbolId)); symbolidx++;
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, noCovList.Contains(GetLimitsText(coverages[i].CargoLimitId, "CargoLimits")) ? "" :
                        GetSymbolsText(coverages[i].CargoLimitSymbolId)); symbolidx++;
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, noCovList.Contains(GetLimitsText(coverages[i].RefCargoLimitId, "RefLimits")) ? "" :
                        GetSymbolsText(coverages[i].RefCargoLimitSymbolId)); symbolidx++;
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, GetSymbolsText(coverages[0].GLBILimitSymbolId)); symbolidx++;
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, ""); symbolidx++;
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, noCovList.Contains(GetLimitsText(coverages[i].CollDeductibleId, "CollisionLimits")) ? "" :
                        GetSymbolsText(coverages[0].CollDeductibleSymbolId)); symbolidx++;
                    var option1Symbol = coverages[0].CompDeductibleSymbolId != null ? GetSymbolsText(coverages[0].CompDeductibleSymbolId) 
                        : GetSymbolsText(coverages[0].FireDeductibleSymbolId);
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, coverages[i].CompDeductibleId != 76 && coverages[i].CompDeductibleId != null ? option1Symbol : ""); symbolidx++;
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, coverages[i].FireDeductibleId != 76 && coverages[i].FireDeductibleId != null ? option1Symbol : ""); symbolidx++;

                    if (hasPollution)
                    {
                     coverageEntries[symbolidx].Add(bookmarkOptionSymbol, ""); symbolidx++;
                    }
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, ""); symbolidx++;
                    coverageEntries[symbolidx].Add(bookmarkOptionSymbol, ""); symbolidx++;

                    if (isRaterVersionSupport)
                    {
                        //Limits
                        coverageEntries[idx].Add(bookmarkOption, coverages[i].TrailerInterchangeLimitId == 822 ? "NONE" : GetLimitsText(coverages[i].TrailerInterchangeLimitId, "TrailerInterchangeLimits")); idx++;
                        coverageEntries[idx].Add(bookmarkOption, coverages[i].NonOwnedTrailerLimitId == 839 ? "NONE" : GetLimitsText(coverages[i].NonOwnedTrailerLimitId, "NonOwnedTrailerLimits"));

                        //Symbols
                        coverageEntries[symbolidx].Add(bookmarkOptionSymbol, noCovList.Contains(GetLimitsText(coverages[i].TrailerInterchangeLimitId, "TrailerInterchangeLimits")) ? "" :
                            GetSymbolsText(coverages[0].TrailerInterchangeLimitSymbolId)); symbolidx++;

                        coverageEntries[symbolidx].Add(bookmarkOptionSymbol, noCovList.Contains(GetLimitsText(coverages[i].NonOwnedTrailerLimitId, "NonOwnedTrailerLimits")) ? "" :
                            GetSymbolsText(coverages[0].NonOwnedTrailerLimitSymbolId));
                    }
                }
                else
                {
                    WriteEmpty(coverageEntries, bookmarkOption, bookmarkOptionSymbol);
                }
            }

            if (!isRaterVersionSupport)
            {

                if (hasPollution)
                { 
                    coverageEntries.RemoveRange(18, 2); 
                }
                else 
                { 
                    coverageEntries.RemoveRange(17, 2); 
                }
                
            }
                
            

            coverageEntries.RemoveAll(HasNoCoverage);
            return coverageEntries;
        }

        private static bool HasNoCoverage(Dictionary<string, string> limit)
        {
            var option1Limit = limit.FirstOrDefault(f => f.Key == "Table_CoverageOption1").Value;
            var option2Limit = limit.FirstOrDefault(f => f.Key == "Table_CoverageOption2").Value;
            var option3Limit = limit.FirstOrDefault(f => f.Key == "Table_CoverageOption3").Value;

            return (option1Limit == "NA" && option2Limit == "NA" && option3Limit == "NA") ||
                (option1Limit == "NONE" && option2Limit == "NONE" && option3Limit == "NONE") ||
                (option1Limit == "No Coverage" && option2Limit == "No Coverage" && option3Limit == "No Coverage");
        }

        private static bool HasNoCoveragePremium(Dictionary<string, string> limit)
        {
            var option1Premium = limit.FirstOrDefault(f => f.Key == "Table_PolPremOption1").Value;
            var option2Premium = limit.FirstOrDefault(f => f.Key == "Table_PolPremOption2").Value;
            var option3Premium = limit.FirstOrDefault(f => f.Key == "Table_PolPremOption3").Value;

            return option1Premium == "NA" || option2Premium == "NA" || option3Premium == "NA";
        }

        private static void WriteEmpty(IReadOnlyList<Dictionary<string, string>> coverageEntries, string bookmarkOption, string bookmarkOptionSymbol)
        {
            for (var i = 0; i < 18; i++)
            {
                coverageEntries[i].Add(bookmarkOption, "");
                coverageEntries[i].Add(bookmarkOptionSymbol, "");
            }
        }

        private string GetLimitsText(int? id, string dataSetName)
        {
            if (id == null) return "NONE";

            var text = ReportData.DataSets[dataSetName].FirstOrDefault(d => d.Id == id)?.LimitDisplay;
            //DB text value has different white space encoding which breaks the display
            return Regex.Replace(text?.ToString() ?? "" , @"\s+", " ");
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

        private string GetVehiclesWithPhysicalDamage(int optionId)
        {
            const int comprehensiveNoCoverageId = 76;
            const int collisionNoCoverageId = 65;

            var option = (optionId + 1).ToString();

            var vehicleCount = ReportData.Risk.Vehicles
                .Where(v => v.Options != null && v.Options.Contains(option) && v.IsActive != false)
                .Count(v => {
                    if (option != "1")
                    {
                        var coverages = ReportData.Risk.RiskCoverages;

                        return (coverages[optionId].FireDeductibleId != null &&
                                coverages[optionId].FireDeductibleId != comprehensiveNoCoverageId) ||
                               (coverages[optionId].CompDeductibleId != null &&
                                coverages[optionId].CompDeductibleId != comprehensiveNoCoverageId) ||
                               (coverages[optionId].CollDeductibleId != null &&
                                coverages[optionId].CollDeductibleId != collisionNoCoverageId);
                    }
                    else
                    {
                        return (v.CoverageFireTheftDeductibleId != null && 
                                v.CoverageFireTheftDeductibleId != comprehensiveNoCoverageId) || 
                               (v.CoverageCollisionDeductibleId != null && 
                                v.CoverageCollisionDeductibleId != collisionNoCoverageId);
                    }
                }
            );
                
            return vehicleCount.ToString();
        }

        private string GetDriversForOption(int option)
        {
            var optionNumber = (option + 1).ToString();
            var drivers = ReportData.Risk.Drivers
                .Count(d => d.Options != null && d.Options.Contains(optionNumber) && d.IsActive != false);
            return drivers.ToString();
        }

        private string GetVehiclesForOption(int option)
        {
            var optionNumber = (option + 1).ToString();
            var vehicles = ReportData.Risk.Vehicles
                .Count(d => d.Options != null && d.Options.Contains(optionNumber) && d.IsActive != false);
            return vehicles.ToString();
        }

        private bool HasPremiumForOption(int optionNumber)
        {
            var coverages = ReportData.Risk.RiskCoverages;
            return optionNumber < coverages.Count && coverages[optionNumber].RiskCoveragePremium?.Premium > 0;
        }
    }

}