using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class DriverTrainingOption : Enumeration<short>
    {
        public static DriverTrainingOption None = new DriverTrainingOption(1, "None");
        public static DriverTrainingOption CompanyRulesAndPolicies = new DriverTrainingOption(2, "Company rules and policies");
        public static DriverTrainingOption VehicleInspectionProcedures = new DriverTrainingOption(3, "Vehicle Inspection Procedures");
        public static DriverTrainingOption EquipmentFamiliarization = new DriverTrainingOption(4, "Equipment Familiarization");
        public static DriverTrainingOption RouteFamiliarization = new DriverTrainingOption(5, "Route familiarization");
        public static DriverTrainingOption EmergencyProcedures = new DriverTrainingOption(6, "Emergency procedures");
        public static DriverTrainingOption AccidentReportingProcedures = new DriverTrainingOption(7, "Accident reporting procedures");
        public static DriverTrainingOption ObservationPeriod = new DriverTrainingOption(8, "Observation Period");

        public List<RiskSpecificDriverHiringCriteria> RiskSpecificDriverHiringCriteria { get; set; }

        public DriverTrainingOption(short id, string name)
            : base(id, name)
        {
        }

        public DriverTrainingOption() { }
    }
}
