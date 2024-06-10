using System.Linq;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class ReinstatementNoticePage : CommonValuesPage
    {
        public ReinstatementNoticePage(ReportData reportData) : base(reportData)
        {
            TemplateName = "ReinstatementNotice.docx";
            PageOrder = 1;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();

            var nameAddress = ReportData.Risk.NameAndAddress;
            var mailingAddress = ReportData.DataSets["Addresses"].FirstOrDefault(a => a.AddressTypeId == (short) AddressTypesEnum.Mailing)?.Address;
            var contact = nameAddress?.BusinessPrincipal;
            var contactAddress = mailingAddress?.StreetAddress1;
            var contactCityStateZip = $"{mailingAddress?.City} {mailingAddress?.State} {mailingAddress?.ZipCode}";

            if (ReportData.AIWithNotice != null)
            {
                contact = ReportData.AIWithNotice.Name;
                contactAddress = ReportData.AIWithNotice.Address;
                contactCityStateZip = $"{ReportData.AIWithNotice.City} {ReportData.AIWithNotice.State} {ReportData.AIWithNotice.ZipCode}";
            }

            FormValues.Add("Company1", CompanyName);
            FormValues.Add("AddressOne", "1900 L Don Dodson Drive");
            FormValues.Add("AddressTwo", "Bedford, TX 76021");

            FormValues.Add("Contact1", contact);
            FormValues.Add("Insured2", nameAddress?.BusinessName);
            FormValues.Add("InsuredAddressOne1", contactAddress);
            FormValues.Add("InsuredAddressTwo1", contactCityStateZip);

            FormValues.Add("Contact", contact);
            FormValues.Add("Insured1", nameAddress?.BusinessName);
            FormValues.Add("InsuredAddressOne", contactAddress);
            FormValues.Add("InsuredAddressTwo", contactCityStateZip);

            FormValues.Add("Company", CompanyName);
            FormValues.Add("Insured", nameAddress?.BusinessName);

            FormValues.Add("Contact2", contact);
            FormValues.Add("Insured3", nameAddress?.BusinessName);
            FormValues.Add("InsuredAddressOne2", contactAddress);
            FormValues.Add("InsuredAddressTwo2", contactCityStateZip);
        }
    }
}