using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class PotentialChangeEndorsementPage : CommonValuesPage
    {
        public PotentialChangeEndorsementPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Potential Change Endorsement.docx";
            PageOrder = 1;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            FormValues.Add("Description", ReportData.Endorsement?.Description);
            FormValues.Add("EndorsementNumber", $"{ReportData.Endorsement?.EndorsementNumber}");
            FormValues.Add("EndorsementEffectiveDate", $"{ReportData.Endorsement?.EffectiveDate:MM/dd/yyyy}");

            SetPremiumChangeTable();
        }

        private void SetPremiumChangeTable()
        {
            try
            {
                bool hasPremiumChanges = ReportData.Endorsement.PremiumChange.TotalPremiumChangeList
                                            .Where(x => x.Description == "Total Premium")
                                            .Select(x => x.EndorsementPendingChange.PremiumDifference)
                                            .FirstOrDefault() != 0;
                var table = new List<Dictionary<string, string>>();
                if (ReportData.Endorsement != null && hasPremiumChanges)
                {
                    var premiumChanges = new List<EndorsementPremiumChangeDTO>();

                    if (ReportData.Endorsement == null) return;

                    premiumChanges.AddRange(ReportData.Endorsement.PremiumChange.VehicleChangeList);
                    premiumChanges.AddRange(ReportData.Endorsement.PremiumChange.PolicyChangeList);
                    premiumChanges.AddRange(ReportData.Endorsement.PremiumChange.TotalPremiumChangeList);
                    premiumChanges.AddRange(ReportData.Endorsement.PremiumChange.FeesChangeList);

                    var headerTexts = new[] { "Vehicle", "Policy", "Total Premium", "Fees (Fully Earned)" };

                    foreach (var premiumChange in premiumChanges)
                    {
                        var isBold = headerTexts.Contains(premiumChange.Description);
                        table.Add(new Dictionary<string, string>
                        {
                            {"Table_Change_Coverage", Format(premiumChange.Description, isBold)},
                            {"Table_Change_AnnualPremium", Format($"{premiumChange.CurrentPremium:N2}", isBold)},
                            {"Table_Change_PremiumBefore", Format($"{premiumChange.EndorsementPendingChange.PremiumBeforeEndorsement:N2}", isBold)},
                            {"Table_Change_PremiumAfter", Format($"{premiumChange.EndorsementPendingChange.PremiumAfterEndorsement:N2}", isBold)},
                            {"Table_Change_PremiumDifference", Format($"{premiumChange.EndorsementPendingChange.PremiumDifference:N2}", isBold)}
                        });
                    }
                }

                TableValues = new Dictionary<string, List<Dictionary<string, string>>>
                {
                    {"Change_", table}
                };
            }
            catch (Exception) { }

        }

        private static string Format(string text, bool isBold)
        {
            var plainText = Regex.Replace(text, "<.*?>", "");

            if (decimal.TryParse(plainText, out _))
            {
                plainText = $"${plainText}";
                plainText = plainText.Replace("$-", "-$");
            }

            if (text.Contains("pad-25")) return Spacer(5) + plainText;
            if (text.Contains("pad-50")) return Spacer(10) + plainText;

            return isBold ? $"<b>{plainText}</b>" : plainText;

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
    }
}