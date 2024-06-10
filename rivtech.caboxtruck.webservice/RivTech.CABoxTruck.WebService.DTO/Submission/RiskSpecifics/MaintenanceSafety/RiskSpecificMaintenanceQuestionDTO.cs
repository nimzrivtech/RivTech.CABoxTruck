using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.MaintenanceSafety
{
    public class RiskSpecificMaintenanceQuestionDTO
    {
        public Int16? SafetyMeetings { get; set; }
        public string PersonInCharge { get; set; }
        public List<SelectItemDTO> GaragingType { get; set; }
        public List<SelectItemDTO> VehicleMaintenance { get; set; }
        public bool? IsMaintenanceProgramManagedByCompany { get; set; }
        public bool? ProvideCompleteMaintenanceOnVehicles { get; set; }
        public bool? AreDriverFilesAvailableForReview { get; set; }
        public bool? AreAccidentFilesAvailableForReview { get; set; }
        public List<SelectItemDTO> CompanyPractice { get; set; }

        public RiskSpecificMaintenanceQuestionDTO()
        {
            GaragingType = new List<SelectItemDTO>();
            VehicleMaintenance = new List<SelectItemDTO>();
            CompanyPractice = new List<SelectItemDTO>();
        }
    }
}
