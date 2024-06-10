using System.Linq;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class CancellationNoticePage : CommonValuesPage
    {
        public CancellationNoticePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "CancellationNotice.docx";
            PageOrder = 1;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var nameAddress = ReportData.Risk.NameAndAddress;
            var mailingAddress = ReportData.DataSets["Addresses"].FirstOrDefault(a => a.AddressTypeId == (short)AddressTypesEnum.Mailing)?.Address;

            var contact = nameAddress?.BusinessPrincipal;
            var contactAddress = mailingAddress?.StreetAddress1;
            var contactCityStateZip = $"{mailingAddress?.City} {mailingAddress?.State} {mailingAddress?.ZipCode}";

            if (ReportData.AIWithNotice != null)
            {
                contact = ReportData.AIWithNotice.Name;
                contactAddress = ReportData.AIWithNotice.Address;
                contactCityStateZip = $"{ReportData.AIWithNotice.City} {ReportData.AIWithNotice.State} {ReportData.AIWithNotice.ZipCode}";
            }

            FormValues.Add("ReportCompanyNameB", CompanyName);
            FormValues.Add("ReportCompanyAddressA", "1900 L Don Dodson Drive");
            FormValues.Add("ReportCompanyAddressB", "Bedford, TX 76021");

            FormValues.Add("Insured", nameAddress?.BusinessName);


            FormValues.Add("Contact1", contact);
            FormValues.Add("Insured2", nameAddress?.BusinessName);
            FormValues.Add("InsuredAddressOne1", contactAddress);
            FormValues.Add("InsuredAddressTwo1", contactCityStateZip);

            FormValues.Add("Contact", contact);
            FormValues.Add("Insured1", nameAddress?.BusinessName);
            FormValues.Add("InsuredAddressOne", contactAddress);
            FormValues.Add("InsuredAddressTwo", contactCityStateZip);

            FormValues.Add("ReportCompanyName", CompanyName);
            
            FormValues.Add("Insured3", nameAddress?.BusinessName);
            FormValues.Add("Contact2", contact);
            FormValues.Add("InsuredAddressOne2", contactAddress);
            FormValues.Add("InsuredAddressTwo2", contactCityStateZip);
           
            var effectiveDate = ReportData.Endorsement?.EffectiveDate.ToString("dddd, MMMM d, yyyy");
            FormValues.Add("CancellationDate", effectiveDate);

            var (reasonTextLine1, reasonTextLine2) = GetCancellationReasonText();
            FormValues.Add("Body", string.Format(reasonTextLine1, effectiveDate, ReportData.Risk?.PolicyNumber));

            if (!string.IsNullOrEmpty(reasonTextLine2))
            {
                FormValues.Add("Body2", reasonTextLine2);
            }
        }


        private (string, string) GetCancellationReasonText()
        {
            var reasonTextLine1 = "";
            var reasonTextLine2 = "";
            switch (ReportData.Endorsement?.CancellationReasonId)
            {
                case "1": // Flat Cancel
                    reasonTextLine1 = @"IT IS HEREBY AGREED AND UNDERSTOOD THAT COVERAGE IS CANCELED EFFECTIVE 12:01 AM {0} FOR POLICY NUMBER {1}.";
                    break;
                case "2": // Driver Requirements Violation
                    reasonTextLine1 = @"IT IS HEREBY AGREED AND UNDERSTOOD THAT COVERAGE IS CANCELED EFFECTIVE 12:01 AM {0}. POLICY NUMBER {1} IS TERMINATED BECAUSE OF INSURED’S FAILURE TO ABIDE BY THE POLICY TERMS AND CONDITIONS REGARDING ADDITION AND APPROVAL OF INSURED DRIVERS.";
                    break;
                case "3": // Finance Company Non-Pay
                case "7": // Non-Payment of Premium
                    reasonTextLine1 = "IT IS HEREBY AGREED AND UNDERSTOOD THAT COVERAGE IS CANCELED EFFECTIVE 12:01 AM {0}. POLICY NUMBER {1} IS TERMINATED BECAUSE OF INSURED’S FAILURE TO PAY AN AMOUNT DUE ON TIME PER THE COMPANY’S INSURANCE POLICY.";
                    reasonTextLine2 = "SHOULD UNPAID AMOUNT BE PAID AFTER THE CANCELLATION DATE, THIS POLICY WILL NOT BE REINSTATED.";
                    break;
                case "4": // Increased Hazard or Material Change
                    reasonTextLine1 = @"IT IS HEREBY AGREED AND UNDERSTOOD THAT COVERAGE IS CANCELED EFFECTIVE 12:01 AM {0}. POLICY NUMBER {1} IS TERMINATED BECAUSE OF: INCREASED HAZARD OR MATERIAL CHANGE.";
                    break;
                case "5": // Non-Cooperation
                    reasonTextLine1 = @"IT IS HEREBY AGREED AND UNDERSTOOD THAT COVERAGE IS CANCELED EFFECTIVE 12:01 AM {0}. POLICY NUMBER {1} IS TERMINATED BECAUSE OF INSURED’S FAILURE TO ABIDE BY THE POLICY TERMS AND CONDITIONS.";
                    break;
                case "6": // Non-Payment of Deductible
                    reasonTextLine1 = "IT IS HEREBY AGREED AND UNDERSTOOD THAT COVERAGE IS CANCELED EFFECTIVE 12:01 AM {0}. POLICY NUMBER {1} IS TERMINATED BECAUSE OF INSURED’S FAILURE TO PAY A CLAIM DEDUCTIBLE AMOUNT DUE ON TIME.";
                    reasonTextLine2 = "SHOULD UNPAID AMOUNT BE PAID AFTER THE CANCELLATION DATE, THIS POLICY WILL NOT BE REINSTATED.";
                    break;
                case "8": // Supporting Liability Policy Canceled
                    reasonTextLine1 = @"IT IS HEREBY AGREED AND UNDERSTOOD THAT COVERAGE IS CANCELED EFFECTIVE 12:01 AM {0}. POLICY NUMBER {1} IS TERMINATED BECAUSE OF: SUPPORTING LIABILITY POLICY CANCELED.";
                    break;
                case "9": // Insured Request
                    reasonTextLine1 = @"IT IS HEREBY AGREED AND UNDERSTOOD THAT COVERAGE IS CANCELED EFFECTIVE 12:01 AM {0} FOR POLICY NUMBER {1}.";
                    break;
            }

            return (reasonTextLine1, reasonTextLine2);
        }
    }
}