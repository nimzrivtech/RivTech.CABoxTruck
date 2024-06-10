using System;
using System.Collections.Generic;
using RivTech.CABoxTruck.WebService.DTO.Forms;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RiskFormOtherDTO
    {
        public Guid RiskDetailId { get; set; }

        public string FormId { get; set; }

        public decimal? Radius { get; set; }

        public decimal? EstimatedAdditionalPremium { get; set; }
        public decimal? VehicleDays { get; set; }
        public decimal? RatePerDay { get; set; }
        public decimal? GrossCombinedWeight { get; set; }
        public decimal? AccidentLimitPerVehicle { get; set; }
        public decimal? Deductible { get; set; }
        public decimal? PhysdamLimit { get; set; }

        public string Requirement { get; set; }

        public string States { get; set; }

        public decimal? CompreLimitOfInsurance { get; set; }
        public decimal? CompreDeductible { get; set; }
        public decimal? ComprePremium { get; set; }
        public decimal? LossLimitOfInsurance { get; set; }
        public decimal? LossDeductible { get; set; }
        public decimal? LossPremium { get; set; }
        public decimal? CollisionLimitOfInsurance { get; set; }
        public decimal? CollisionDeductible { get; set; }
        public decimal? CollisionPremium { get; set; }
        public decimal? FireLimitOfInsurance { get; set; }
        public decimal? FireDeductible { get; set; }
        public decimal? FirePremium { get; set; }
        public decimal? FireTheftLimitOfInsurance { get; set; }
        public decimal? FireTheftDeductible { get; set; }
        public decimal? FireTheftPremium { get; set; }

        public decimal? InjuryLimit { get; set; }
        public decimal? MedicalExpenses { get; set; }
        public decimal? DamagesRentedToYou { get; set; }
        public decimal? AggregateLimit { get; set; }

        public string UsDotNumber { get; set; }
        public DateTime DateReceived { get; set; }
        public string IssuedTo { get; set; }
        public string State { get; set; }
        public string DatedAt { get; set; }
        public int PolicyDateDay { get; set; }
        public string PolicyDateMonth { get; set; }
        public int PolicyDateYear { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string InsuranceCompany { get; set; }
        public string CounterSignedBy { get; set; }
        public decimal AccidentLimit { get; set; }
        public short? AutoBILimitId { get; set; }
        public short? UMBILimitId { get; set; }
        public short? GLBILimitId { get; set; }
        public short? PIPLimitId { get; set; }
        public short? MPLimitId { get; set; }
        public short? UMPDLimitId { get; set; }
        public short? UIMBILimitId { get; set; }
        public short? UIMPDLimitId { get; set; }
        public short? TrailerInterchangeLimitId { get; set; }
        public short? PollutionCovLimitId { get; set; }

        public bool addedFirstPartyBenefits { get; set; }
        public decimal? medicalExpenseBenefits { get; set; }
        public decimal? workLossUpTo { get; set; }
        public decimal? workLossPerMonth { get; set; }
        public decimal? funeralExpenseBenefits { get; set; }
        public decimal? accidentalDeathBenefits { get; set; }
        public bool combinationFirstPartyBenefits { get; set; }
        public decimal? maximumTotalLimitAllBenefits { get; set; }
        public decimal? combFuneralExpenseBenefits { get; set; }
        public decimal? combAccidentalDeathBenefits { get; set; }
        public string CoveredAutoText { get; set; }
        public string TextForEndorsement { get; set; }

        public List<FormDriverDTO> Drivers { get; set; }

        public string DescriptionOfAutos { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }

}