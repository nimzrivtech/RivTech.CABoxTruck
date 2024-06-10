export class ComponentField {
  fieldName: string;
  isParent: boolean;
  triggerToShow: boolean;
}

const componentFieldConstructor = (fieldName: string, isParent: boolean, triggerToShow?: boolean): ComponentField => ({
  fieldName: fieldName,
  isParent: isParent,
  triggerToShow: triggerToShow
});

export const ComponentListConstants = [
  {
    FormName: 'applicantForm',
    ComponentList: [
      componentFieldConstructor('renewal', false),
    ]
  },
  {
    FormName: 'businessDetailsForm',
    ComponentList: [
      componentFieldConstructor('newVentureSwitch', false),
      componentFieldConstructor('yearEstablishedInput', false),
      componentFieldConstructor('firstYearUnderCurrentInput', false),
      componentFieldConstructor('otherTranspoOpDropdown', false, true),
      componentFieldConstructor('yearBusinessInput', false),
      componentFieldConstructor('yearUnderCurrentInput', false),
      componentFieldConstructor('businessTypeDropdown', false),
      componentFieldConstructor('mainUseDropdown', false),
      componentFieldConstructor('accountCategoryDropdown', false),
      componentFieldConstructor('descOperationsInput', false),
      componentFieldConstructor('federalIdInput', false),
      componentFieldConstructor('naicsCodeInput', false),
      componentFieldConstructor('pucNumberInput', false),
      componentFieldConstructor('anySubsidiariesCompaniesSwitch', true),
      componentFieldConstructor('subsidiariesNameInput', false, true),
      componentFieldConstructor('typeOfBusinessDropdown', false, true),
      componentFieldConstructor('numVechSizeCompInput', false, true),
      componentFieldConstructor('relationshipInput', false, true),
      componentFieldConstructor('companyPastSwitch', true),
      componentFieldConstructor('nameYearsOperationInput', false, true),
    ]
  },
  {
    FormName: 'driverInformationForm',
    ComponentList: [
      componentFieldConstructor('isBusinessPrincipalDriverOnPolicy', false),
      componentFieldConstructor('vehicleOperatedForPersonalUse', false),
      componentFieldConstructor('volunteersUsedInBusiness', true),
      componentFieldConstructor('percentageOfStaff', false, true),
      componentFieldConstructor('hireFromOthersForUse', true),
      componentFieldConstructor('annualCostOfHireFromOthersForUse', false, true),
      componentFieldConstructor('hireFromOthersWithDriver', true),
      componentFieldConstructor('annualCostOfHireFromOthersWithDriver', false, true),
      componentFieldConstructor('leaseToOthersForUse', true),
      componentFieldConstructor('annualIncomeDerivedUse', false, true),
      componentFieldConstructor('leaseToOthersWithoutDriver', true),
      componentFieldConstructor('annualIncomeDerivedLease', false, true),
      componentFieldConstructor('assumedLiabilityByContractAgreement', false),
      componentFieldConstructor('driversTakeVehiclesHome', false),
      componentFieldConstructor('vehiclesOwnedByApplicant', false),
      componentFieldConstructor('addOrDeleteVehiclesDuringPolicyTerm', true),
      componentFieldConstructor('decsribeAddOrDeleteVehiclesDuringPolicyTerm', false, true),
      componentFieldConstructor('pastYearDriverHired', false),
    ]
  },
  {
    FormName: 'driverHiringCriteriaForm',
    ComponentList: [
      componentFieldConstructor('driversDriveSimilarVehicleCommerciallyForMoreThan2Years', false),
      componentFieldConstructor('driversHave5YearsDrivingExperience', false),
      componentFieldConstructor('agreeToReportAllDriversToRivington', false),
      componentFieldConstructor('familyMembersUnder21PrimaryDriversOfCompanyAuto', false),
      componentFieldConstructor('driversProperlyLicensedDOTcompliant', false),
      componentFieldConstructor('isDisciplinaryPlanDocumentedDrivers', false),
      componentFieldConstructor('isThereDriverIncentiveProgram', false),
      componentFieldConstructor('backgroundCheckIncludes', false)
    ]
  },
  {
    FormName: 'brokerForm',
    ComponentList: [
      componentFieldConstructor('isBrokeredAccount', false),
      componentFieldConstructor('isIncumbentAgency', false),
      componentFieldConstructor('isMidtermMove', false),
    ]
  },
  {
    FormName: 'filingsInformationForm',
    ComponentList: [
      componentFieldConstructor('isARatedCarrierRequired', false),
      componentFieldConstructor('isFilingRequired', false),
      componentFieldConstructor('isOthersAllowedToOperateCurrent', false),
      componentFieldConstructor('isOthersAllowedToOperatePast', false),
      componentFieldConstructor('isOperatorsWorksExclusivelyForInsured', false),
      componentFieldConstructor('isOperatorsWorksExclusivelyForInsured', false),
      componentFieldConstructor('isThereDriverIncentiveProgram', false),
      componentFieldConstructor('backgroundCheckIncludes', false)
    ]
  },
  {
    FormName: 'additionalInterestForm',
    ComponentList: [
      componentFieldConstructor('isPrimaryNonContributory', false),
      componentFieldConstructor('isWaiverOfSubrogation', false),
      componentFieldConstructor('isAutoLiability', false),
      componentFieldConstructor('isGeneralLiability', false),
      componentFieldConstructor('is30DayNotice', false),
    ]
  },
  {
    FormName: 'maintenanceForm',
    ComponentList: [
      componentFieldConstructor('isMaintenanceProgramManagedByCompany', false),
      componentFieldConstructor('provideCompleteMaintenanceOnVehicles', false),
      componentFieldConstructor('areDriverFilesAvailableForReview', false),
      componentFieldConstructor('areAccidentFilesAvailableForReview', false),
      componentFieldConstructor('companyPractice', false),
    ]
  },
  {
    FormName: 'safetyDevicesForm',
    ComponentList: [
      componentFieldConstructor('isInPlace', false),
      componentFieldConstructor('unitDescription', false)
    ]
  },
  {
    FormName: 'generalLiabilityForm',
    ComponentList: [
      componentFieldConstructor('contractuallyRequiredToCarryGeneralLiabilityInsurance', false),
      componentFieldConstructor('operationsAtStorageLotOrImpoundYard', false),
      componentFieldConstructor('anyStorageOfGoods', false),
      componentFieldConstructor('anyWarehousing', false),
      componentFieldConstructor('anyStorageOfVehiclesForOthers', false),
      componentFieldConstructor('anyLeasingSpaceToOthers', false),
      componentFieldConstructor('anyFreightForwarding', false),
      componentFieldConstructor('anyStorageOfFuelsAndOrChemicals', false),
      componentFieldConstructor('glAnnualPayroll', false),
      componentFieldConstructor('haveOperationsOtherThanTrucking', true),
      componentFieldConstructor('haveOperationsOtherThanTruckingExplain', false, true),
      componentFieldConstructor('hasApplicantHadGeneralLiabilityLoss', true),
      componentFieldConstructor('hasApplicantHadGeneralLiabilityLossExplain', false, true)
    ]
  },
  {
    FormName: 'cargoForm',
    ComponentList: [
      componentFieldConstructor('areRequiredToCarryCargoInsurance', false),
      componentFieldConstructor('requireRefrigerationBreakdownCoverage', false),
      componentFieldConstructor('haulHazardousMaterials', false),
      componentFieldConstructor('haveRefrigeratedUnits', false),
      componentFieldConstructor('areCommoditiesStoredInTruckOvernight', true),
      componentFieldConstructor('areCommoditiesStoredInTruckOvernightExplain', false, true),
      componentFieldConstructor('hadCargoLoss', true),
      componentFieldConstructor('hadCargoLossExplain', false, true)
    ]
  },
  {
    FormName: 'radiusOfOperationsForm',
    ComponentList: [
      componentFieldConstructor('anyDestinationToMexicoPlanned', false),
      componentFieldConstructor('anyNyc5BoroughsExposure', false)
    ]
  },
  {
    FormName: 'blackListedDriverForm',
    ComponentList: [
      componentFieldConstructor('isActive', true),
    ]
  },
  {
    FormName: 'driverInfoForm',
    ComponentList: [
      componentFieldConstructor('isHiringFromOthers', false),
      componentFieldConstructor('isHiringFromOthersWithoutDriver', false),
      componentFieldConstructor('isLeasingToOthers', false),
      componentFieldConstructor('isLeasingToOthersWithoutDriver', false),
      componentFieldConstructor('isThereAssumedLiabilityByContract', false),
      componentFieldConstructor('doDriversTakeVehiclesHome', false),
      componentFieldConstructor('areVehiclesSolelyOwnedByApplicant', false),
      componentFieldConstructor('willBeAddingDeletingVehiclesDuringTerm', false)
    ]
  },
  {
    FormName: 'driverHiringCriteriaForm',
    ComponentList: [
      componentFieldConstructor('areDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years', false),
      componentFieldConstructor('doDriversHave5YearsDrivingExperience', false),
      componentFieldConstructor('isAgreedToReportAllDriversToRivington', false),
      componentFieldConstructor('areFamilyMembersUnder21PrimaryDriversOfCompanyAuto', false),
      componentFieldConstructor('areDriversProperlyLicensedDotCompliant', false),
      componentFieldConstructor('isDisciplinaryPlanDocumented', false),
      componentFieldConstructor('isThereDriverIncentiveProgram', false)
    ]
  }, {
    FormName: 'underwritingQuestionsForm',
    ComponentList: [
      componentFieldConstructor('areAllEquipmentOperatedUnderApplicantsAuthority', false),
      componentFieldConstructor('hasInsuranceBeenObtainedThruAssignedRiskPlan', false),
      componentFieldConstructor('hasAnyCompanyProvidedNoticeOfCancellation', false),
      componentFieldConstructor('hasFiledForBankruptcy', false),
      componentFieldConstructor('hasOperatingAuthoritySuspended', false),
      componentFieldConstructor('hasVehicleCountBeenAffectedByCovid19', false),
    ]
  },
  {
    FormName: 'vehicleForm',
    ComponentList: [
      componentFieldConstructor('isAutoLiability', true),
      componentFieldConstructor('isCoverageFireTheft', false),
      componentFieldConstructor('isCoverageCollision', false),
      componentFieldConstructor('hasCoverageCargo', false),
      componentFieldConstructor('hasCoverageRefrigeration', false)
    ]
  },
  {
    FormName: 'driverinfoGrid',
    ComponentList: [
      componentFieldConstructor('driverRatingTab', true),
      componentFieldConstructor('accidentInfo', true),
    ]
  },
  {
    FormName: 'driverinfoGridDetails',
    ComponentList: [
      componentFieldConstructor('outStateDriver', false),
      componentFieldConstructor('isExcludeKO', false),
    ]
  },
  {
    FormName: 'dotInfoForm',
    ComponentList: [
      componentFieldConstructor('doesApplicantRequireDotNumber', false),
      componentFieldConstructor('doesApplicantPlanToStayInterstate', true),
      componentFieldConstructor('doesApplicantHaveInsterstateAuthority', true),
      componentFieldConstructor('isDotCurrentlyActive', true),
      componentFieldConstructor('isThereAnyPolicyLevelAccidents', false),
      componentFieldConstructor('isUnsafeDrivingChecked', false),
      componentFieldConstructor('isHoursOfServiceChecked', false),
      componentFieldConstructor('isDriverFitnessChecked', false),
      componentFieldConstructor('isControlledSubstanceChecked', false),
      componentFieldConstructor('isVehicleMaintenanceChecked', false),
      componentFieldConstructor('isCrashChecked', false),
    ]
  },
  {
    FormName: 'bindRequirements',
    ComponentList: [
      componentFieldConstructor('isPreBind', false),
    ]
  },
  {
    FormName: 'cancellationForm',
    ComponentList: [
      componentFieldConstructor('isShortRate', false),
    ]
  },
  {
    FormName: 'accountReceivableReportForm',
    ComponentList: [
      componentFieldConstructor('pastDueOnly', false),
      componentFieldConstructor('noPaymentsMade', false),
    ]
  },
];
