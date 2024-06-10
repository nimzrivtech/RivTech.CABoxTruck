using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RiskSummaryDto
    {
        public Guid Id { get; set; }
        public Guid CurrentRiskId { get; set; }
        public string SubmissionNumber { get; set; }
        public string PolicyNumber { get; set; }
        public string InsuredName { get; set; }
        public string Broker { get; set; }
        public Guid? AgencyId { get; set; }
        public DateTime? InceptionDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Status { get; set; }
        public string PolicyStatus { get; set; }
        public int NumberOfUnits { get; set; }
        public int CntPowerUnit { get; set; }
        public int CntTrailer { get; set; }
        public int CntOther { get; set; }
        public string State { get; set; }
        public string SubmissionType { get; set; }
        public string PolicyLimit { get; set; }
        public string NewVenture { get; set; }
        public string NeededBy { get; set; }
        public string BrokerContact { get; set; }
        public Guid? AgentId { get; set; }
        public string QAC { get; set; }
        public string MidTerm { get; set; }
        public DateTime? LastNoteAdded { get; set; }
        public string AUSpecialist { get; set; }
        public string AURep { get; set; }
        public string AUPSR { get; set; }
        public string BrokerZip { get; set; }
        public string BrokerState { get; set; }
        public string BrokerCity { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Underwriter { get; set; }
        public string ExpiringPolicy { get; set; }
        public short? UseClass { get; set; }
        public string LossRatio { get; set; }
        public Guid InsuredEntityId { get; set; }
        public Guid RiskId { get; set; }
        public string Owner { get; set; }
        public bool IsPolicy { get; set; }
        public string AssignedToId { get; set; }
        public string AssignedToName { get; set; }
        public bool IsRenewal { get; set; }
        public string InsuredDBA { get; set; }
    }
}
