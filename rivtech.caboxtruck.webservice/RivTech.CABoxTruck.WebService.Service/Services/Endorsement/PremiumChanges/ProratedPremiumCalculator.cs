using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.PremiumChanges
{
    public class ProratedPremiumCalculator
    {
        public decimal Calculate(decimal annualPremium, DateTime annualEffectiveDate, DateTime annualExpirationDate, DateTime endorsementEffectiveDate)
        {
            var annualDays = (annualExpirationDate - annualEffectiveDate).TotalDays;
            var premiumPerDay = annualPremium / (decimal)annualDays;

            var proratedDays = (endorsementEffectiveDate - annualEffectiveDate).TotalDays;
            var proratedPremium = (decimal)proratedDays * premiumPerDay;

            return proratedPremium;
        }

        public decimal Calculate(decimal annualPremium, DateTime annualEffectiveDate, DateTime annualExpirationDate, DateTime startDate, DateTime endDate)
        {
            var annualDays = (annualExpirationDate - annualEffectiveDate).TotalDays;
            var premiumPerDay = annualPremium / (decimal)annualDays;

            var proratedDays = (endDate - startDate).TotalDays;
            var proratedPremium = (decimal)proratedDays * premiumPerDay;

            return proratedPremium;
        }

        public decimal CalculateAdded(decimal annualPremium, DateTime annualEffectiveDate, DateTime annualExpirationDate, DateTime effectiveDate)
        {
            var annualDays = (annualExpirationDate - annualEffectiveDate).TotalDays;
            var premiumPerDay = annualPremium / (decimal)annualDays;

            var proratedDays = (annualExpirationDate - effectiveDate).TotalDays;
            var proratedPremium = (decimal)proratedDays * premiumPerDay;

            return proratedPremium;
        }
        public decimal CalculateRemoved(decimal annualPremium, DateTime annualEffectiveDate, DateTime annualExpirationDate, DateTime effectiveDate)
        {
            var annualDays = (annualExpirationDate - annualEffectiveDate).TotalDays;
            var premiumPerDay = annualPremium / (decimal)annualDays;

            var proratedDays = (effectiveDate - annualEffectiveDate).TotalDays;
            var proratedPremium = (decimal)proratedDays * premiumPerDay;

            return proratedPremium;
        }

    }
}
