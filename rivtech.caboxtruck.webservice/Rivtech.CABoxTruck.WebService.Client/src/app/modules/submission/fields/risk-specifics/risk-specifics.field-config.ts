import {Injectable} from '@angular/core';
import {SelectItem} from '../../../../shared/models/dynamic/select-item';
import {FieldConfig} from '../../../../shared/models/dynamic/field.interface';
import {Validators} from '@angular/forms';

const CHECKBOX: string = "checkbox";
const RADIOBUTTON: string = "radiobutton";
const SELECT: string = "select";
const YES: string = "YES";
const NO: string = "NO";
const NA: string = "N/A";

@Injectable()
export class RiskSpecificsFieldConfig {
  FieldConfigs = RISK_SPECIFICS_FIELDS;

  constructor() { }
}

export const RISK_SPECIFICS_FIELDS: FieldConfig[] = [
  {
      type: CHECKBOX,
      label: 'Is this a new venture? :',
      name: 'isNewVenture',
      value: true
  },
  {
    type: RADIOBUTTON,
    label: 'Any operational exposure over a 300 mile radius? :',
    name: 'operationalExposureOver300MileRadius',
    value: true,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },  
  { //highlighted red
    type: CHECKBOX,
    label: 'Do drivers carry cell phones? :',
    name: 'driversCarryCellphones',
    value: true,
  },  
  { //highlighted red
    type: CHECKBOX,
    label: 'Are cell phones hands free? :',
    name: 'cellphoneHandsFree',
    value: true,
  },  
  { //highlighted red
    type: CHECKBOX,
    label: 'App used for navigation? :',
    name: 'appUsedForNavigation',
    value: true,
  },
  {
    type: CHECKBOX,
    label: 'Is the business principal a driver on the policy? :',
    name: 'isBusinessPrincipalDriverOnPolicy',
    value: true,
  },   
  {
    type: RADIOBUTTON,
    label: 'Will vehicles be operated for personal use? :',
    name: 'vehicleOperatedForPersonalUse',
    value: false,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Are any volunteers used in your business? :',
    name: 'volunteersUsedInBusiness',
    value: false,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Do you hire from others for your use? :',
    name: 'hireFromOthersForUse',
    value: false,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Do you hire from others with a driver? :',
    name: 'hireFromOthersWithDriver',
    value: false,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Do you lease to others for their use? :',
    name: 'leaseToOthersForUse',
    value: false,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Do you lease to others without a driver? :',
    name: 'leaseToOthersWithoutDriver',
    value: false,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Is there assumed liability by contract/agreement? :',
    name: 'assumedLiabilityByContractAgreement',
    value: false,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Do drivers take vehicles home? :',
    name: 'driversTakeVehiclesHome',
    value: false,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Are the vehicles solely owned by the applicant? :',
    name: 'vehiclesOwnedByApplicant',
    value: true,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Will you be adding or deleting vehicles during the policy term? :',
    name: 'addOrDeleteVehiclesDuringPolicyTerm',
    value: false, //none
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Are Drivers considered independent contractors? :',
    name: 'driversConsideredIndependentContractors',
    value: false, //none
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },
  {
    type: RADIOBUTTON,
    label: 'Have all drivers been driving a similar vehicle commercially for 2+ years? :',
    name: 'driversDriveSimilarVehicleCommerciallyForMoreThan2Years',
    value: false, //none
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Do all drivers have at least 5 years U.S. driving experience? :',
    name: 'driversHave5YearsDrivingExperience',
    value: false, //none
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Do you agree to report all drivers to Rivington? :',
    name: 'agreeToReportAllDriversToRivington',
    value: true,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Are any family members under 21 primary drivers of a company auto? :',
    name: 'familyMembersUnder21PrimaryDriversOfCompanyAuto',
    value: false,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Are all drivers properly licensed and DOT Compliant? :',
    name: 'driversProperlyLicensedDOTcompliant',
    value: true,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Is disciplinary plan documented for all drivers? :',
    name: 'isDisciplinaryPlanDocumentedDrivers',
    value: false,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },   
  {
    type: RADIOBUTTON,
    label: 'Is there a driver incentive program? :',
    name: 'isThereDriverIncentiveProgram',
    value: false,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },
  {
    type: SELECT,
    label: 'Background Check Includes ',
    name: 'backgroundCheckIncludes'
  },    
  {
    type: SELECT,
    label: 'Driver Training Includes ',
    name: 'driverTrainingIncludes'
  },
  { //highlighted yellow
    type: CHECKBOX,
    label: 'Vehicle daily condition reports :',
    name: 'vehicleDailyConditionReports',
    value: true,
  },
  {
    type: RADIOBUTTON,
    label: 'Is your maintenance program managed by your company? :',
    name: 'isMaintenanceProgramManagedByCompany',
    value: true,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },  
  {
    type: RADIOBUTTON,
    label: 'Do you provide complete maintenance on all vehicles? :',
    name: 'provideCompleteMaintenanceOnVehicles',
    value: true,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },  
  {
    type: RADIOBUTTON,
    label: 'Are Driver Files available for review? :',
    name: 'driverFilesAvailableForReview',
    value: true,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },  
  {
    type: RADIOBUTTON,
    label: 'Are Accident Files available for review? :',
    name: 'accidentFilesAvailableForReview',
    value: true,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },  
  {
    type: RADIOBUTTON,
    label: 'Will all claims be reported directly to NARS? :',
    name: 'allClaimsReportedDirectlyToNARS',
    value: true,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },  
  {
    type: RADIOBUTTON,
    label: 'Does road supervision include Recording Devices? :',
    name: 'roadSupervisionIncludeRecordingDevices',
    value: true,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ]
  },  
  { //highlighted red
    type: CHECKBOX,
    label: 'Does road supervision include Radio Dispatch? :',
    name: 'roadSupervisionIncludeRadioDispatch',
    value: true,
  },  
  {
    type: SELECT,
    label: 'Select those Which Apply ',
    name: 'selectThoseWhichApply'
  },
  {
    type: CHECKBOX,
    label: 'Are you contractually required to carry cargo insurance? :',
    name: 'contractuallyRequiredToCarryCargoInsurance',
    value: false,
  },  
  {
    type: CHECKBOX,
    label: 'Are commodities ever stored in truck overnight? :',
    name: 'commoditiesStoredInTruckOvernight',
    value: false,
  },  
  {
    type: CHECKBOX,
    label: 'Do you haul any hazardous materials? :',
    name: 'haulAnyHazardousMaterials',
    value: false, //none
  },  
  {
    type: CHECKBOX,
    label: 'Do you have any refrigerated units? :',
    name: 'haveRefrigeratedUnits',
    value: false,
  },  
  {
    type: CHECKBOX,
    label: 'Do you require refrigeration breakdown coverage? :',
    name: 'requireRefrigerationBreakdownCoverage',
    value: false,
  },  
  {
    type: CHECKBOX,
    label: 'Has applicant ever had a cargo loss? :',
    name: 'applicantHadCargoLoss',
    value: false, //none
  }, //end
  {
    type: CHECKBOX,
    label: "Do you provide Worker's Compensation for all employees? :",
    name: 'provideWorkersCompensationForAllEmployees',
    value: false, //none
  },  
  {
    type: CHECKBOX,
    label: "Is all equipment operated under the applicant's authority scheduled on the applicant's driver and vehicle schedule? :",
    name: 'equipmentOperatedUnderApplicantsAuthorityScheduledOnApplicantsDriverVehicleSchedule',
    value: false, //none
  },  
  {
    type: CHECKBOX,
    label: 'During the past 4 years, has your insurance ever been obtained through an Assigned Risk Plan? :',
    name: 'inPast4yearsInsuranceObtainedAssignedRiskPlan',
    value: false,
  },  
  {
    type: CHECKBOX,
    label: 'Has any company provided notice of cancellation/non-renewal or otherwise canceled/refused to renew your insurance, including during the current term? :',
    name: 'companyProvidedNoticeOfCancellationNonrenewalOrCanceledRefusedRenew',
    value: false,
  },  
  {
    type: CHECKBOX,
    label: 'Have you ever filed for or contemplated filing for bankruptcy or had bankruptcy proceedings initiated against you by another party? :',
    name: 'filedOrContemplatedFilingBankruptcy?',
    value: false,
  },  
  {
    type: CHECKBOX,
    label: 'Has your operating authority ever been suspended or revoked or have you received notice of intent to suspend? :',
    name: 'operatingAuthoritySuspendedRevoked',
    value: false
  },  
  {
    type: CHECKBOX,
    label: 'I confirm that the answers to the above are correct to my knowledge :',
    name: 'riskSpecificQuestionConfirmation',
    value: false, //none
  }
];
