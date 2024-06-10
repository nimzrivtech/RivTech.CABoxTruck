using System;

namespace RivTech.CABoxTruck.WebService.DTO.Report
{
    public class SubmissionQuoteReportDTO
    {
        public string PolicyNumber { get; set; }
        public string SubmissionNumber { get; set; }
        public string BusinessType { get; set; }
        public string Program { get; set; }
        public string Insured { get; set; }
        public string InsuredAddress { get; set; }
        public string InsuredState { get; set; }
        public string InsuredZip { get; set; }
        public string ExposureGaragingState { get; set; }
        public string SubmissionType { get; set; }
        public string SubmissionStatus { get; set; }
        public string ExpiringPolicy { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime InceptionDate { get; set; }
        public string InceptionTBD { get; set; }

        public string LastNoteAdded { get; set; }
        public string AURepresentative { get; set; }
        public string AUPSR { get; set; }
        public string ProductUnderwriter { get; set; }
        public string InternalUnderwriter { get; set; }
        public string AssistantUnderwriter { get; set; }
        public string AutoLiabilityLimit { get; set; }
        public string AutoLiabilityDeductible { get; set; }
        public string PIPLimit { get; set; }
        public string AutoMedicalPaymentLimit { get; set; }
        public string CollisionDeductible { get; set; }
        public string CompFTDeductible { get; set; }
        public string MotorTruckCargoLimit { get; set; }
        public string UMBILimit { get; set; }
        public string UMPDLimit { get; set; }
        public string UIMBILimit { get; set; }
        public string UIMPDLimit { get; set; }
        public int VehicleUnits { get; set; }
        public string LossRatio { get; set; }

        public decimal QuotedPremium { get; set; }
        public string QuotedPaper { get; set; }

        public string Broker { get; set; }
        public string BrokerContact { get; set; }
        public string BrokerCity { get; set; }
        public string BrokerState { get; set; }
        public string BrokerContactPhone { get; set; }
        public string BrokerContactEmail { get; set; }
    }
}
