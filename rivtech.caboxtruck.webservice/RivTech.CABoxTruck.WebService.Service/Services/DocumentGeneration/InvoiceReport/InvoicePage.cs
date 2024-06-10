using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.DTO.Enums;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class InvoicePage : SinglePage, IPacketPage
    {
        public string TemplateName
        {
            get
            {
                var payType = IsFinanced ? "Financed" : "NonFinanced";
                switch (ReportData.Billing?.InvoiceType)
                {
                    case InvoiceType.Deposit:
                    case InvoiceType.Installment:
                        return $"Invoice ({payType}).docx";
                    case InvoiceType.Endorsement:
                        return GetEndorsementItemCount() > 5 ? $"Invoice ({payType}Next) Endorsement.docx" : $"Invoice ({payType}) Endorsement.docx";
                    case InvoiceType.Cancellation:
                        return $"Final Audit ({payType}).docx";
                    case null:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return $"Invoice ({payType}).docx";
            }
        }

        public int PageOrder => 1;

        private readonly string[] _feeTypes = { "Fee", "Transaction Fee" };

        public bool IsFinanced => ReportData.Risk?.Binding.PaymentTypesId == 3;

        private Dictionary<string, string> _formValues = new Dictionary<string, string>();

        protected Dictionary<string, List<Dictionary<string, string>>> TableValues { get; set; }

        private readonly List<VehiclePremiumChange> _premiumChanges;

        public InvoicePage(ReportData reportData) : base(reportData)
        {
            _premiumChanges = GetVehiclePremiumChanges();
        }

        public Form GetData()
        {
            TableValues = new Dictionary<string, List<Dictionary<string, string>>>();
            GetFormValues();
            SetBillTo();
            SetBroker();
            var formData = new Form
            {
                PageOrder = PageOrder,
                TemplateName = $"documenttemplate/Invoice/{TemplateName}",
                FormValues = _formValues,
                TableValues = TableValues
            };

            return formData;
        }

        private void GetFormValues()
        {
            var billing = ReportData.Billing;
            var nameAddress = ReportData.Risk.NameAndAddress;
            var billingAddress = ReportData.DataSets["Addresses"].FirstOrDefault(a => a.AddressTypeId == (short)AddressTypesEnum.Billing)?.Address;
            var brokerAddress = ReportData.DataSets["BrokerAddresses"]?.FirstOrDefault(a => a.AddressTypeId == (short)AddressTypesEnum.Billing)?.Address;
            var brokerInfo = ReportData.Risk.BrokerInfo;
            const string dateFormat = "MM/dd/yyyy";
            var insuredName = nameAddress?.BusinessName ?? "";

            _formValues = new Dictionary<string, string>
            {
                {"ReportCompanyName", "Texas Insurance Company"},
                {"LOB", IsFinanced ? "Financed" : "Non Financed"},

                {"PolicyNumber", ReportData.Risk.PolicyNumber},
                {"DueDate", billing?.DueDate.ToString(dateFormat)},
                {"DueDateB", billing?.DueDate.ToString(dateFormat)},
                {"InvoiceNumber", billing?.InvoiceNumber},

                {"Stub_InvoiceDate", billing?.InvoiceDate.ToString(dateFormat)},
                {"Stub_PolicyNumber", ReportData.Risk.PolicyNumber},
                {"Stub_InvoiceNumber", billing?.InvoiceNumber},

                {"Stub_DueDate", billing?.DueDate.ToString(dateFormat)},

                { "PaymentPortalLink", ReportData.DataSets["CommonValues"].FirstOrDefault(a => a.Id == "PaymentPortalLink")?.Value}
            };

            if (!string.IsNullOrEmpty(billingAddress?.StreetAddress2))
            {
                _formValues.Add("BillTo_AddressB", $" {billingAddress.StreetAddress2},");
                _formValues.Add("BillTo_AddressB2", $" {billingAddress.StreetAddress2},");
                _formValues.Add("BillTo_AddressB3", $" {billingAddress.StreetAddress2},");
                _formValues.Add("Stub_BillTo_AddressB", $" {billingAddress.StreetAddress2},");
            }

            if (!string.IsNullOrEmpty(brokerAddress?.StreetAddress2))
            {
                _formValues.Add("Broker_AddressB", $" {brokerAddress.StreetAddress2},");
            }

            var dueToRivington = billing?.TotalAmountDue ?? 0;
            if (IsFinanced) dueToRivington = SetFinancedValues();
            
            switch (ReportData.Billing?.InvoiceType)
            {
                case InvoiceType.Endorsement:
                    SetEndorsementValues();
                    break;
                case InvoiceType.Cancellation:
                    dueToRivington = SetCancellationValues();
                    break;
                default:
                    SetDepositOrInstallmentValues();
                    break;
            }

            _formValues.Add("TotalDue", dueToRivington.ToCurrency());
            _formValues.Add("Stub_TotalDue", dueToRivington.ToCurrency());
            _formValues.Add("InsuredName", insuredName);

        }

        private void SetBillTo()
        {
            var nameAddress = ReportData.Risk.NameAndAddress;
            //var businessName = nameAddress?.BusinessName ?? "";
            //var contactName = nameAddress?.InsuredDBA ?? "";
            //var email = nameAddress?.Email ?? "";

            var brokerAddress = ReportData.DataSets["BrokerAddresses"]?.FirstOrDefault(a => a.AddressTypeId == (short)AddressTypesEnum.Business)?.Address;
            var brokerInfo = ReportData.Risk.BrokerInfo;

            // Default to Broker Info
            var businessName = brokerInfo?.Agency?.Entity?.CompanyName ?? "";
            var email = brokerInfo?.Agent?.Entity?.WorkEmailAddress ?? "";
            var contactName = "";

            var address = brokerAddress?.StreetAddress1;
            var streetAddress2 = brokerAddress?.StreetAddress2;
            var cityStateZip = $"{brokerAddress?.City}, {brokerAddress?.StateCode} {brokerAddress?.ZipCode}";

            var billingContact = ReportData.DataSets["RiskPolicyContacts"].Cast<RiskPolicyContactDTO>().Where(x => x.TypeId == (short)AddressTypesEnum.Billing).FirstOrDefault();
            var nameAddressBilling = nameAddress.Addresses.FirstOrDefault(x => x.AddressTypeId == (short)AddressTypesEnum.Billing);
            if (IsFinanced == false && billingContact != null)
            {
                businessName = nameAddress?.BusinessName ?? "";
                contactName = $"{billingContact.FullName}";
                address = nameAddressBilling.Address.StreetAddress1;
                streetAddress2 = "";
                cityStateZip = $"{nameAddressBilling.Address.City}, {nameAddressBilling.Address.State} {nameAddressBilling.Address.ZipCode}";
                email = billingContact.Email;
            }

            var table = new List<Dictionary<string, string>> {new Dictionary<string, string> {{"Table_BillTo_Details", businessName}}};
            var bill2Table = new List<Dictionary<string, string>> {new Dictionary<string, string> {{"Table_BillTo2_Details", businessName}}};
            var bill3Table = new List<Dictionary<string, string>> {new Dictionary<string, string> {{"Table_BillTo3_Details", businessName}}};
            var stubTable = new List<Dictionary<string, string>> {new Dictionary<string, string> {{"Table_StubBillTo_Details", businessName}}};

            if (!string.IsNullOrEmpty(contactName))
            {
                table.Add(new Dictionary<string, string> {{"Table_BillTo_Details", contactName}});
                bill2Table.Add(new Dictionary<string, string> {{"Table_BillTo2_Details", contactName}});
                bill3Table.Add(new Dictionary<string, string> {{"Table_BillTo3_Details", contactName}});
                stubTable.Add(new Dictionary<string, string> {{"Table_StubBillTo_Details", contactName}});
            }

            if (!string.IsNullOrEmpty(streetAddress2))
            {
                address += $", {streetAddress2}";
            }

            if (!string.IsNullOrEmpty(address))
            {
                table.Add(new Dictionary<string, string> {{"Table_BillTo_Details", address}});
                bill2Table.Add(new Dictionary<string, string> {{"Table_BillTo2_Details", address}});
                bill3Table.Add(new Dictionary<string, string> {{"Table_BillTo3_Details", address}});
                stubTable.Add(new Dictionary<string, string> {{"Table_StubBillTo_Details", address}});
            }

            table.Add(new Dictionary<string, string> {{"Table_BillTo_Details", cityStateZip}});
            bill2Table.Add(new Dictionary<string, string> {{"Table_BillTo2_Details", cityStateZip}});
            bill3Table.Add(new Dictionary<string, string> {{"Table_BillTo3_Details", cityStateZip}});
            stubTable.Add(new Dictionary<string, string> {{"Table_StubBillTo_Details", cityStateZip}});

            if (!string.IsNullOrEmpty(email))
            {
                table.Add(new Dictionary<string, string> {{"Table_BillTo_Details", email}});
            }

            TableValues.Add("BillTo_", table);
            TableValues.Add("BillTo2_", bill2Table);
            TableValues.Add("BillTo3_", bill3Table);
            TableValues.Add("StubBillTo_", stubTable);
        }

        private void SetBroker()
        {
            var brokerAddress = ReportData.DataSets["BrokerAddresses"]?.FirstOrDefault(a => a.AddressTypeId == (short) AddressTypesEnum.Business)?.Address;
            var brokerInfo = ReportData.Risk.BrokerInfo;

            var companyName = brokerInfo?.Agency?.Entity?.CompanyName ?? "";
            var agentEmail = brokerInfo?.Agent?.Entity?.WorkEmailAddress ?? "";

            //if (ReportData.Risk?.BrokerInfo?.IsBrokeredAccount == true)
            //{
            //    companyName = brokerInfo?.SubAgency?.Entity?.CompanyName;
            //    agentEmail = brokerInfo?.SubAgent?.Entity?.WorkEmailAddress;
            //}

            var table = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> {{"Table_Broker_Details", companyName}}
            };

            var address = "";
            if (!string.IsNullOrEmpty(brokerAddress?.StreetAddress1))
            {
                address = brokerAddress.StreetAddress1;
                if (!string.IsNullOrEmpty(brokerAddress.StreetAddress2))
                {
                    address += $", {brokerAddress.StreetAddress2}";
                }
            }

            if (!string.IsNullOrEmpty(address))
            {
                table.Add(new Dictionary<string, string> {{"Table_Broker_Details", address}});
            }

            if (!string.IsNullOrEmpty(brokerAddress?.City))
            {
                var cityStateZip = $"{brokerAddress.City}, {brokerAddress.StateCode} {brokerAddress.ZipCode}";
                table.Add(new Dictionary<string, string> { { "Table_Broker_Details", cityStateZip } });
            }

            if (!string.IsNullOrEmpty(brokerInfo?.Agent?.Entity?.WorkEmailAddress))
            {
                table.Add(new Dictionary<string, string> {{"Table_Broker_Details", agentEmail } });
            }

            TableValues.Add("Broker_", table);
        }

        private decimal GetInvoicePremium()
        {
            return ReportData.Billing?.InvoiceDetails.Where(detail => detail.AmountSubType?.AmountType?.Description == "Premium").Sum(detail => detail.InvoicedAmount) ?? 0;
        }

        private void SetEndorsementValues()
        {
            SetDepositOrInstallmentValues();

            var endorsedVins = GetEndorsedVins();
            var table = new List<Dictionary<string, string>>();
            var adjustments = new List<dynamic>();
            foreach (var change in _premiumChanges)
            {
                var endorsedVin = endorsedVins.FirstOrDefault(ev => ev.Vin == change.Vin);

                // Other VINs adjustment, don't display
                if (endorsedVin == null && !string.IsNullOrEmpty(change.Vin))
                {
                    adjustments.Add(new { ChangeType = "Adjustments", Amount = change.Amount });
                    continue;
                }

                // if is adjustment
                if (endorsedVin != null && endorsedVin.IsAdjustment)
                {
                    adjustments.Add(new { ChangeType = "Adjustments", Amount = change.Amount });
                    continue;
                }

                // Display policy level premium adjustment (eg. GL)
                var changeType = endorsedVin != null ? endorsedVin.ChangeType : change.ChangeType; 

                table.Add(new Dictionary<string, string>
                {
                    {"Table_PremiumDetails_Description", $"{changeType} {change.Vin}"},
                    {"Table_PremiumDetails_Amount", change.Amount.ToCurrency()}
                });
            }

            if (adjustments.Count > 0)
            {
                table.Add(new Dictionary<string, string>
                {
                    {"Table_PremiumDetails_Description", "Adjustments"},
                    {"Table_PremiumDetails_Amount", adjustments.Sum(x => (decimal)x.Amount).ToCurrency() }
                });
            }

            TableValues.Add("PremiumDetails_", table);
        }

        private int GetEndorsementItemCount()
        {
            var endorsementItemCount = 0;
            var endorsedVins = GetEndorsedVins();
            foreach (var change in _premiumChanges)
            {
                var endorsedVin = endorsedVins.FirstOrDefault(ev => ev.Vin == change.Vin);
                if (endorsedVin == null && !string.IsNullOrEmpty(change.Vin)) continue;
                endorsementItemCount++;
            }
            return endorsementItemCount;
        }

        private List<dynamic> GetEndorsedVins()
        {
            var endorsedVins = new List<dynamic>();
            if (ReportData.Billing?.PolicyChanges == null) return endorsedVins;

            var policyChanges = JsonConvert.DeserializeObject<List<string>>(ReportData.Billing?.PolicyChanges);
            
            foreach (var change in policyChanges.Where(change => change.Contains("VIN")))
            {
                try
                {
                    string changeType;
                    string vin;
                    bool isAdjustmentOnly = false;
                    if (change.Contains("Added")) changeType = "ADD";
                    else if (change.Contains("Removed")) changeType = "REMOVE";
                    else changeType = "Adjustment";

                    var startMarker = change.IndexOf("VIN ", StringComparison.Ordinal);
                    if (startMarker < 0) continue;

                    var vinPart = change.Substring(startMarker + 4);
                    if (string.IsNullOrEmpty(vinPart)) continue;

                    if (vinPart.Substring(0, 1) == "'")
                    {
                        vinPart = vinPart.Substring(1);
                        vin = vinPart.Substring(0, vinPart.IndexOf("'", StringComparison.Ordinal));
                    }
                    else
                    {
                        isAdjustmentOnly = true;
                        vin = vinPart.Substring(0, vinPart.IndexOf(" ", StringComparison.Ordinal));
                    }

                    endorsedVins.Add(new { ChangeType = changeType, Vin = vin, IsAdjustment = isAdjustmentOnly });
                }
                catch { }
            }

            return endorsedVins;
        }

        private List<VehiclePremiumChange> GetVehiclePremiumChanges()
        {
            var premiumChanges = new List<VehiclePremiumChange>();

            var vehicleSubTypeIds = new[] { BillingConstants.VehicleALP, BillingConstants.VehiclePDP, BillingConstants.VehicleCP };
            foreach (var detail in ReportData.Billing.InvoiceDetails)
            {
                if (detail.AmountSubType?.AmountType?.Description == "Premium" && !vehicleSubTypeIds.Contains(detail.AmountSubTypeId))
                {
                    premiumChanges.Add(new VehiclePremiumChange { ChangeType = detail.AmountSubType?.Description, Amount = detail.InvoicedAmount });
                }
            }

            var vehiclePremiumChanges = ReportData.DataSets["VehiclePremiumChanges"].Cast<VehiclePremiumChange>().ToList();
            if (vehiclePremiumChanges.Any()) premiumChanges.AddRange(vehiclePremiumChanges.Where(vp => vp.Amount != 0));

            return premiumChanges;
        }

        private decimal GetInvoiceTax()
        {
            var taxAmountTypes = new[] {"Tax", "Transaction Tax"};
            return ReportData.Billing?.InvoiceDetails
                .Where(detail => taxAmountTypes.Contains(detail.AmountSubType?.AmountType?.Description))
                .Sum(detail => detail.InvoicedAmount) ?? 0;
        }

        private decimal GetInvoiceFees()
        {
            if (ReportData.Billing?.InvoiceDetails == null) return 0;

            TableValues = new Dictionary<string, List<Dictionary<string, string>>>();
            var feesTable = new List<Dictionary<string, string>>();

            var invoiceFees = ReportData.Billing?.InvoiceDetails.Where(i => _feeTypes.Contains(i.AmountSubType?.AmountType?.Description)).ToList();
            if (!invoiceFees.Any())
            {
                invoiceFees.Add(new InvoiceDetailsDTO
                {
                    AmountSubType = new AmountSubTypeDTO {Id = "NF", Description = "No fees"}, InvoicedAmount = 0
                });
            }

            decimal fees = 0;
            foreach (var detail in invoiceFees)
            {
                fees += detail.InvoicedAmount;
                feesTable.Add(new Dictionary<string, string>
                {
                    {"Table_Fees_FeeType", GetFeeText(detail.AmountSubType)},
                    {"Table_Fees_Amount", detail.InvoicedAmount.ToCurrency()}
                });
            }

            TableValues.Add("Fees_", feesTable);
            return fees;
        }

        private decimal SetCancellationValues()
        {
            var billing = ReportData.Billing;

            var premium = ReportData.Billing?.FinalPremium ?? 0;
            var taxes = ReportData.Billing?.FinalTaxes ?? 0;
            var fees = ReportData.Billing?.FinalFees ?? 0;

            var total = premium + taxes+ fees;

            _formValues.Add("FinalPremium", premium.ToCurrency());
            _formValues.Add("FinalTaxes", taxes.ToCurrency());
            _formValues.Add("FinalFees", fees.ToCurrency());
            _formValues.Add("TotalPolicyCost", total.ToCurrency());
            _formValues.Add("TotalPaid", billing?.TotalPaid.ToCurrency());

            GetInvoiceFees();

            decimal dueToRivington;
            if (!IsFinanced)
            {
                dueToRivington = total - (billing?.TotalPaid ?? 0);
                _formValues.Add("TotalDueText", dueToRivington < 0 ? "For Refund" : "to Rivington");
                return dueToRivington;
            }

            var netPolicyCost = total + (ReportData.Billing?.BrokerCommission ?? 0);
            _formValues.Add("GrossPolicyCost", total.ToCurrency());
            _formValues.Add("NetPolicyCost", netPolicyCost.ToCurrency());

            dueToRivington = netPolicyCost - (billing?.TotalPaid ?? 0);
            _formValues.Add("TotalDueText", dueToRivington < 0 ? "For Refund" : "to Rivington");

            return dueToRivington;
        }

        private static string GetFeeText(AmountSubTypeDTO amountSubType)
        {
            return amountSubType?.Id switch
            {
                "ALSF" => "Service Fee - AL",
                "PDSF" => "Service Fee - PD",
                _ => amountSubType?.Description ?? ""
            };
        }

        private void SetDepositOrInstallmentValues()
        {
            var billing = ReportData.Billing;
            var premium = GetInvoicePremium();
            var balanceForward = (billing?.TotalAmountDue ?? 0) - (billing?.CurrentAmountInvoiced ?? 0);

            _formValues.Add("InvoiceType", IsFinanced ? "Bound Premium" : ReportData.Billing?.InvoiceType == InvoiceType.Installment ? "Installment Premium" : "Deposit Premium");
            _formValues.Add("PremiumAmount", premium.ToCurrency());
            _formValues.Add("BalanceForward", balanceForward.ToCurrency());
            _formValues.Add("CurrentInstallmentDue", billing?.CurrentAmountInvoiced.ToCurrency());
            _formValues.Add("Premium", premium.ToCurrency());
            _formValues.Add("Taxes", GetInvoiceTax().ToCurrency());
            _formValues.Add("Fees", GetInvoiceFees().ToCurrency());
        }

        private decimal SetFinancedValues()
        {
            var premium = GetInvoicePremium();
            var brokerCommission = Math.Abs(ReportData.Billing?.BrokerCommission ?? 0);

            brokerCommission = premium != 0 ? brokerCommission : 0;

            var premiumDue = 0m;
            var totalAmountInvoiced = 0m;
            if (premium >= 0)
            {
                premiumDue = premium - brokerCommission;
                totalAmountInvoiced = (ReportData.Billing?.CurrentAmountInvoiced ?? 0) - brokerCommission;
            }
            else
            {
                premiumDue = premium - -brokerCommission;
                totalAmountInvoiced = (ReportData.Billing?.CurrentAmountInvoiced ?? 0) - -brokerCommission;
            }
            
            _formValues.Add("FinancedPremium", premium.ToCurrency());
            _formValues.Add("BrokerCommission", (-brokerCommission).ToCurrency());
            _formValues.Add("PremiumDue", premiumDue.ToCurrency());

            return totalAmountInvoiced;
        }

        public bool isAttached => true;
    }
}