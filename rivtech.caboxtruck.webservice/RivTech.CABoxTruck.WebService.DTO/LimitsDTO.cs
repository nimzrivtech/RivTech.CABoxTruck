using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class LimitsDTO
    {
        public int Id { get; set; }
        public string LimitDisplay { get; set; }
        public string LimitValue { get; set; }
        public string SingleLimit { get; set; }
        public string PerPerson { get; set; }
        public string PerAccident { get; set; }
        public string StateCode { get; set; }
        public string LimitType { get; set; }
        public bool IsActive { get; set; }
        public string RaterVersionId { get; set; }
    }

    public class LimitListDTO
    {
        public List<LimitsDTO> AutoBILimits { get; set; }
        public List<LimitsDTO> AutoPDLimits { get; set; }
        public List<LimitsDTO> UMBILimits { get; set; }
        public List<LimitsDTO> UMPDLimits { get; set; }
        public List<LimitsDTO> UIMBILimits { get; set; }
        public List<LimitsDTO> UIMPDLimits { get; set; }
        public List<LimitsDTO> MedPayLimits { get; set; }
        public List<LimitsDTO> PipLimits { get; set; }
        public List<LimitsDTO> LiabilityDeductibleLimits { get; set; }
        public List<LimitsDTO> ComprehensiveLimits { get; set; }
        public List<LimitsDTO> CollisionLimits { get; set; }
        public List<LimitsDTO> GlLimits { get; set; }
        public List<LimitsDTO> CargoLimits { get; set; }
        public List<LimitsDTO> RefLimits { get; set; }
        public List<LimitsDTO> TrailerInterchangeLimits { get; set; }
        public List<LimitsDTO> NonOwnedTrailerLimits { get; set; }
        public List<LimitsDTO> TrailerTypeLimits { get; set; }
        public List<LimitsDTO> PollutionCovLimits { get; set; }

    }

    public class LimitType
    {
        public static string AutoBI = "ABI";
        public static string AutoPD = "APD";
        public static string UMBI = "UMBI";
        public static string UMPD = "UMPD";
        public static string UIMBI = "UIMBI";
        public static string UIMPD = "UIMPD";
        public static string MedPay = "MED";
        public static string PIP = "PIP";
        public static string LiabilityDeductible = "LIA";
        public static string Comprehensive = "COMP";
        public static string Collision = "COL";
        public static string GL = "GL";
        public static string Cargo = "CAR";
        public static string RefBreakdown = "REF";
        public static string TrailerInterchange = "TRI";
        public static string NonOwnedTrailer = "NOT";
        public static string TrailerType = "TRT";
        public static string PollutionCov = "POC";
    }
}
